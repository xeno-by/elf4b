using System;
using DataVault.Core.Api;
using Elf.Core.Runtime.Contexts;
using Elf.Core.Runtime.Impl;
using Elf.Core.TypeSystem;
using Esath.Data;
using Esath.Data.Core;
using Esath.Pie.Helpers;

namespace Esath.Eval.Ver2
{
    public class EvalSessionScriptHostScopeResolver : DefaultScopeResolver
    {
        private IVault Vault(RuntimeContext ctx) { return (IVault)ctx.VM.Context["vault"]; }
        private EvalSession Session(RuntimeContext ctx) { return (EvalSession)ctx.VM.Context["evalSession"]; }
        private IBranch Node(RuntimeContext ctx) { return (IBranch)ctx.VM.Context["nodeInProgress"]; }

        public override IElfObject Get(RuntimeContext ctx, string name, IElfObject @this)
        {
            var b = Vault(ctx).GetBranch(name.FromElfIdentifier());
            if (b == null)
            {
                throw new ReferencedBranchDoesNotExistException(Node(ctx), name);
            }
            else
            {
                if (b.IsFov())
                {
                    var eval = Session(ctx).Eval(b);

                    var typeToken = b.GetValue("type").ContentString;
                    var val = typeToken == "percent" ? ((double)eval) * 100 : eval;

                    var obj_t = typeToken.GetTypeFromToken();
                    var obj = (IElfObject)Activator.CreateInstance(obj_t, val);
                    obj.Bind(ctx.VM);
                    return obj;
                }
                else
                {
                    return new ScenarioNode(b);
                }
            }
        }

        public override void Set(RuntimeContext ctx, string name, IElfObject value, IElfObject @this)
        {
            // todo. temporarily not implemented
        }
    }
}