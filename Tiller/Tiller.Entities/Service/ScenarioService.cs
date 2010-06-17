namespace ObjectMeet.Tiller.Entities.Service
{
	using System;
	using System.IO;
	using System.Linq;
	using Api;
	using DataVault.Api;
	using Pocso;
	using Whit;
	using Whit.Internal;
	using Whit.Traits;

	public class ScenarioService : IScenarioService
	{
		public IInteractionProvider InteractionProvider { get; set; }

		public bool CreateScenario(out IScenario scenario)
		{
			var item = scenario = null;
			if (!InteractionProvider.IORetryCancel(() => item = InternalCreateScenario(), "Создание нового сценария")) return false;
			scenario = item;
			return true;
		}

		private IScenario InternalCreateScenario()
		{
			FileInfo file;
			for (var i = 1;;)
				if ((file = new FileInfo(Path.Combine(Manifest.DefaultDocumentDirectory.FullName, Manifest.NewScenarioName + i++))).Exists) break;

			IVault vault = null;

			try
			{
				vault = VaultApi.OpenZip(file.FullName);
				var entity = VaultWhit.New<Scenario>(vault);
				entity.ScenarioId = Guid.NewGuid();
				entity.StructureVersion = Manifest.StructureVersion;
				entity.ContainingFile = file;
				entity.IsDirty = true;
				entity.Revision = 1;

				var common = entity.Create(x => x.ScenarioNodes, InitializeCommonRootShadow(entity));
				var particular = entity.Create(x => x.ScenarioNodes, InitializeParticularRootShadow(entity));

				entity.CommonRootNodeId = common.Id;
				entity.ParticularRootNodeId = particular.Id;

				return entity;
			}
			catch
			{
				if (vault != null) vault.Dispose();
				throw;
			}
		}

		private IScenarioNode InitializeCommonRootShadow(IScenario scenario)
		{
			var node = VaultWhit.New<ScenarioNode>();
			node.SiblingWeight = 0;
			node.Level = 1;
			node.IsManagedByTool = true;
			node.Name = "Общие Характеристики";
			node.NodeVariation = ScenarioNodeVariation.Common;
			node.IsUnderCommonRootNode = true;
			node.IsUnderParticularRootNode = false;
			node.Scenario = scenario;
			return node;
		}

		private IScenarioNode InitializeParticularRootShadow(IScenario scenario)
		{
			var node = VaultWhit.New<ScenarioNode>();
			node.SiblingWeight = 1;
			node.Level = 1;
			node.IsManagedByTool = true;
			node.Name = "Характеристики Объекта Оценки";
			node.NodeVariation = ScenarioNodeVariation.Particular;
			node.IsUnderCommonRootNode = false;
			node.IsUnderParticularRootNode = true;
			node.Scenario = scenario;
			return node;
		}

		public bool LoadScenario(FileInfo fileInfo, out IScenario scenario)
		{
			if (fileInfo == null) throw new ArgumentNullException("fileInfo");
			var item = scenario = null;
			if (!InteractionProvider.IORetryCancel(() => item = InternalLoadScenario(fileInfo), "Загрузка сценария")) return false;
			scenario = item;
			return scenario != null;
		}

		private IScenario InternalLoadScenario(FileInfo fileInfo)
		{
			while (true)
			{
				var vault = VaultApi.OpenZip(fileInfo.FullName);
				var entity = VaultWhit.New<Scenario>(vault);

				if (entity.StructureVersion > Manifest.StructureVersion) // too new
					InteractionProvider.AlertAndForceCancelAction("Загружаемый сценарий был сохранен в более поздней версии системы. Текущая версия системы устарела. Обратитесь к техническому персоналу за поддержкой.");
				else if (entity.StructureVersion.Major < Manifest.StructureVersion.Major)
				{
// ReSharper disable AccessToModifiedClosure
					// convertation required
					var converter = (from item in new ModelConvertationService().Converters
					                 where
					                 	item.ToVersion.ToString(2) == Manifest.StructureVersion.ToString(2) &&
					                 	item.FromVersions.Where(x => entity.StructureVersion.Matches(x)).Count() == 1
					                 select item).FirstOrDefault();
// ReSharper restore AccessToModifiedClosure
					if (converter == null) InteractionProvider.AlertAndForceCancelAction("Загружаемый сценарий был сохранен в более ранней версии системы. К сожалению текущая версия системы не обрабатывает данный формат. Обратитесь к техническому персоналу за поддержкой.");
					converter.ScenarioService = this;
					var suffix = string.Format(".(V{0})", entity.StructureVersion.ToString(2));
					entity = null;
					vault = null;
					GC.Collect();
					GC.WaitForPendingFinalizers();
					//if (InteractionProvider != null) if (!InteractionProvider.AskConfirmation("Загрузка сценария", "Загружаемый сценарий был сохранен в более ранней версии системы. Текущая версия системы использует более совершенный формат хранения данных. Обновить сценарий до текущей версии? (При этом его невозможно будет открыть в ранней версии системы).")) throw new ActionCancelledException();
					var newFile = fileInfo.MoveAndAddSuffix(suffix);
					if (converter.Convert(newFile, fileInfo))
						continue;
					throw new ActionCancelledException();
				}
				else
				{
					// default way
					return entity;
				}
			}
		}

		private Exception UnpackEntity<TEntity, TModel>(object publicEntity, out TEntity entity, out TModel model) where TEntity : class where TModel : IBranch
		{
			entity = null;
			model = default(TModel);

			if (publicEntity == null) return new ArgumentNullException();
			var creature = publicEntity as ICreature;
			if (creature == null) return new ArgumentException("Creature expected");
			if (creature.IsShadow) return new BusinessRuleViolationException(4, 10);
			if (creature.Model == null) return new ArgumentException("Model is invalid");

			try
			{
				entity = (TEntity) publicEntity;
				model = (TModel) creature.Model;
			}
			catch (InvalidCastException)
			{
				return new ApplicationException("Invalid structure detected");
			}

			return null;
		}


		public bool SaveScenario(IScenario scenario)
		{
			return InteractionProvider.IORetryCancel(() => InternalSaveScenario(scenario), "Сохранение сценария");
		}

		private void InternalSaveScenario(IScenario scenario)
		{
			Scenario entity;
			IVault model;
			var oops = UnpackEntity(scenario, out entity, out model);
			if (oops != null) throw oops;

			if (entity.Revision < 2) throw new BusinessRuleViolationException(16, 1);

			var originalRevision = entity.Revision;

			try
			{
				model.Backup();
				model.Save();
			}
			catch
			{
				entity.Revision = originalRevision;
				throw;
			}
			entity.Revision++;
			entity.IsDirty = false;
		}

		public bool SaveScenarioAs(IScenario scenario, FileInfo newFile)
		{
			if (newFile.Exists && InteractionProvider != null)
			{
				if (!InteractionProvider.AskConfirmation("Перезапись файла", "Файл с таким именем уже существует. Перезаписать?")) return false;
				if (!InteractionProvider.IORetryCancel(() => newFile.Delete(), "Удаление файла")) return false;
			}
			return InteractionProvider.IORetryCancel(() => InternalSaveScenarioAs(scenario, newFile), "Сохранение сценария");
		}

		private void InternalSaveScenarioAs(IScenario scenario, FileInfo newFile)
		{
			Scenario entity;
			IVault model;
			var oops = UnpackEntity(scenario, out entity, out model);
			if (oops != null) throw oops;

			var originalId = entity.ScenarioId;
			var originalRevision = entity.Revision;

			entity.ScenarioId = Guid.NewGuid();
			entity.Revision = 1;
			try
			{
				model.SaveAs(newFile.FullName);
			}
			catch
			{
				entity.ScenarioId = originalId;
				entity.Revision = originalRevision;
				throw;
			}
			entity.Revision++;
			entity.IsDirty = false;
			entity.ContainingFile = newFile;
		}

		public IScenarioNode NewNode()
		{
			return VaultWhit.New<ScenarioNode>();
		}

		public bool AttachNode(IScenarioNode parentNode, ref IScenarioNode scenarioNode)
		{
			var parentCreature = parentNode as ICreature;
			var childCreature = scenarioNode as ICreature;

			if (parentCreature == null || childCreature == null) return false;
			if (parentCreature.IsShadow || !childCreature.IsShadow) return false;
			var parent = parentCreature as ScenarioNode;
			var child = scenarioNode as ScenarioNode;
			if (parent == null || parent.Scenario as Scenario == null || child == null) return false;

			child.ParentNodeId = parent.Id;
			child.IsUnderCommonRootNode = parent.IsUnderCommonRootNode;
			child.IsUnderParticularRootNode = parent.IsUnderParticularRootNode;
			child.SiblingWeight = parent.ChildNodeCount;
			child.Level = parent.Level + 1;
			scenarioNode = ((Scenario) parent.Scenario).Create(s => s.ScenarioNodes, scenarioNode);
			parent.ChildNodeCount++;

			return true;
		}

		public ISourceDatumDeclaration NewSourceDatumDeclaration()
		{
			return VaultWhit.New<ISourceDatumDeclaration>();
		}

		public bool AttachSourceDatumDeclaration(IScenarioNode parentNode, ref ISourceDatumDeclaration sourceDatumDeclaration)
		{
			var parentCreature = parentNode as ICreature;
			var childCreature = sourceDatumDeclaration as ICreature;

			if (parentCreature == null || childCreature == null) return false;
			if (parentCreature.IsShadow || !childCreature.IsShadow) return false;
			var parent = parentCreature as ScenarioNode;
			var child = sourceDatumDeclaration as SourceDatumDeclaration;
			if (parent == null || parent.Scenario as Scenario == null || child == null) return false;

			child.ScenarioNodeId = parent.Id;
			child.IsUnderCommonRootNode = parent.IsUnderCommonRootNode;
			child.IsUnderParticularRootNode = parent.IsUnderParticularRootNode;
			child.SiblingWeight = parent.SourceDatumDeclarationCount;
			sourceDatumDeclaration = ((Scenario) parent.Scenario).Create(s => s.SourceDatumDeclarations, sourceDatumDeclaration);
			parent.SourceDatumDeclarationCount++;

			return true;
		}

		public IFormulaDeclaration NewFormulaDeclaration()
		{
			return VaultWhit.New<IFormulaDeclaration>();
		}

		public bool AttachFormulaDeclaration(IScenarioNode parentNode, ref IFormulaDeclaration formulaDeclaration)
		{
			var parentCreature = parentNode as ICreature;
			var childCreature = formulaDeclaration as ICreature;

			if (parentCreature == null || childCreature == null) return false;
			if (parentCreature.IsShadow || !childCreature.IsShadow) return false;
			var parent = parentCreature as ScenarioNode;
			var child = formulaDeclaration as FormulaDeclaration;
			if (parent == null || parent.Scenario as Scenario == null || child == null) return false;

			child.ScenarioNodeId = parent.Id;
			child.IsUnderCommonRootNode = parent.IsUnderCommonRootNode;
			child.IsUnderParticularRootNode = parent.IsUnderParticularRootNode;
			child.SiblingWeight = parent.FormulaDeclarationCount;
			formulaDeclaration = ((Scenario) parent.Scenario).Create(s => s.FormulaDeclarations, formulaDeclaration);
			parent.FormulaDeclarationCount++;

			return true;
		}
	}
}