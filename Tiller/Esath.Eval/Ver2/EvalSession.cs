using System;
using System.Collections.Generic;
using System.Linq;
using DataVault.Core.Api;
using Elf.Core;
using Elf.Exceptions;
using Elf.Exceptions.Runtime;
using Elf.Helpers;
using DataVault.Core.Helpers;
using DataVault.Core.Helpers.Assertions;

namespace Esath.Eval.Ver2
{
	using DataVault.Core.Helpers.Assertions;

	public class EvalSession : IDisposable
	{
		private VirtualMachine VM { get; set; }

		private IVault Vault
		{
			get { return (IVault) VM.Context["vault"]; }
		}

		private IVault Repository
		{
			get { return (IVault) VM.Context["repository"]; }
		}

		private readonly List<IDisposable> _expositions = new List<IDisposable>();

		private Dictionary<VPath, Object> Cache
		{
			get { return (Dictionary<VPath, Object>) VM.Context["evalCache"]; }
		}

		private HashSet<VPath> NodesInProgress
		{
			get { return (HashSet<VPath>) VM.Context["nodesInProgress"]; }
		}

		private IBranch NodeInProgress
		{
			get { return (IBranch) VM.Context["nodeInProgress"]; }
			set { VM.Context["nodeInProgress"] = value; }
		}

		private bool DesignTime
		{
			get { return (bool) VM.Context["designTime"]; }
		}


		private readonly HashSet<Guid> _checkedNodes;


		public EvalSession(IVault vault, IVault repository, IEnumerable<Guid> checkedNodes)
			: this(vault, repository)
		{
			_checkedNodes = new HashSet<Guid>(checkedNodes ?? new Guid[0]);
		}

		protected EvalSession(IVault vault, IVault repository)
		{
			AppDomain.CurrentDomain.Load("Esath.Data");

			_expositions.Add(vault.AssertNotNull().ExposeReadOnly());
			if (repository != null)
			{
				_expositions.Add(repository.AssertNotNull().ExposeReadOnly());
			}

			VM = new VirtualMachine();
			VM.Context.Add("vault", vault);
			VM.Context.Add("repository", repository);
			VM.Context.Add("designTime", repository == null);
			VM.Context.Add("evalCache", new Dictionary<VPath, Object>());
			VM.Context.Add("evalSession", this);
			VM.Context.Add("nodesInProgress", new HashSet<VPath>());
			VM.Context.Add("nodeInProgress", null);
		}

		public object Eval(IBranch b)
		{
			return Eval(b, null);
		}

		public object Eval(IBranch b, string childScript)
		{
			try
			{
				if (!Cache.ContainsKey(b.VPath))
				{
					if (b.GetOrCreateValue("declarationType", "source").ContentString == "source")
					{
						String val;
						IValue v;

						val = (v = b.GetValue("valueForTesting")) == null ? "" : v.ContentString;
						if (string.IsNullOrEmpty(val))
						{
							var repositoryVal = (v = b.GetValue("repositoryValue")) == null ? null : v.ContentString;
							if (!string.IsNullOrEmpty(repositoryVal))
							{
								val = Repository.GetValue(repositoryVal).ContentString;
							}
						}


						var type = (v = b.GetValue("type")) == null ? "string" : v.ContentString;
						var script = String.Format("'[[{0}]]{1}'", type, val);
						Cache.Add(b.VPath, EvalScript(script));
					}
					else
					{
						if (NodesInProgress.Contains(b.VPath))
						{
							throw new EvalStackOverflowException(b, NodesInProgress.Select(vp => Vault.GetBranch(vp)));
						}
						else
						{
							object result;

							var old = NodeInProgress;
							NodeInProgress = b;
							NodesInProgress.Add(b.VPath);

							try
							{
								var source = b.GetValue("elfCode").ContentString;
								if (!string.IsNullOrEmpty(source))
								{
									IBranch conditions = null;
									// in case of a conditional node, we replace its formula body with the corresponding body of checked child node
									if (_checkedNodes != null && (conditions = b.Parent.Parent.GetBranch("_conditions")) != null && conditions.GetBranches().Length > 0)
									{
										var index = 0;
										if ((index = source.IndexOf('=')) > 0) // is subject to replace?
										{
											// look for the very first checked child
											foreach (var child in b.Parent.Parent.GetBranches())
											{
												if (!_checkedNodes.Contains(child.Id)) continue;
												var varName = source.Substring(0, index);
												var formulasBranch = child.GetBranch("_formulaDeclarations");
												if (formulasBranch == null) continue;
												foreach (var formula in formulasBranch.GetBranches())
												{
													var formulaBody = "";
													// look for the very first formula body having $varName within
													if (string.IsNullOrEmpty(formulaBody = formula.GetValue("elfCode").ContentString) || formulaBody.IndexOf(varName) < 0) continue;

													var lines = formulaBody.Split('\n');
													lines[lines.Length - 1] = "ret " + lines[lines.Length - 1];
													source = string.Join(Environment.NewLine, lines);
													break;
												}
												break;
											}
										}
									}
									if (!string.IsNullOrEmpty(childScript))
										source = string.Concat(source, Environment.NewLine, childScript);
								}
								result = EvalScript(source);
							}
							finally
							{
								NodeInProgress = old;
								NodesInProgress.Remove(b.VPath);
							}

							Cache.Add(b.VPath, result);
						}
					}
				}

				return Cache[b.VPath];
			}
			catch (BaseEvalException)
			{
				throw;
			}
			catch (ErroneousScriptRuntimeException esex)
			{
				if (esex.Type == ElfExceptionType.OperandsDontSuitMethod)
				{
					throw new ArgsDontSuitTheFunctionException(b, esex.Thread.RuntimeContext.PendingClrCall, esex);
				}
				else
				{
					throw new UnexpectedErrorException(b, esex);
				}
			}
			catch (Exception ex)
			{
				if (ex.InnerException is FormatException)
				{
					throw new BadFormatOfSerializedStringException(b, ex);
				}
				else
				{
					throw new UnexpectedErrorException(b, ex);
				}
			}
		}

		private String WrapInteractiveElf(String interactive)
		{
			var funcBody = interactive;
			if (funcBody.SelectLines().Length == 1 && !funcBody.Contains(";")) funcBody = "ret " + funcBody;

			var funcDef = String.Format("def Main(){0}{1}{0}end",
			                            Environment.NewLine, funcBody.Indent(1));

			var className = ("EvalSession_" + Guid.NewGuid()).Where(c => c != '-').ToArray();
			return String.Format("def {1} rtimpl EvalSessionScriptHost{0}{2}{0}end",
			                     Environment.NewLine, new String(className), funcDef.Indent(1));
		}

		private Object EvalScript(String elfLight)
		{
			var wrapped = WrapInteractiveElf(elfLight);
			VM.Load(wrapped);

			var className = wrapped.Substring(0, wrapped.NthIndexOf(" ", 2)).Substring(4);
			return VM.CreateEntryPoint(className, "Main").RunTillEnd();
		}

		public void Dispose()
		{
			_expositions.ForEach(e => e.Dispose());
		}
	}
}