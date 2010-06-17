namespace ObjectMeet.Tiller.Entities.Pocso
{
	using System;
	using System.ComponentModel;
	using Api;
	using Whit;

	internal abstract class TillerEntityBase : ITillerEntity
	{
		[Browsable(false)]
		[MetaInfo(IsPrimaryKey = true)]
		public abstract Guid Id { get; }

		[Browsable(false)]
		public abstract int SiblingWeight { get; protected internal set; }

		[Browsable(false)]
		public abstract bool IsManagedByTool { get; protected internal set; }

		[Browsable(false)]
		public abstract bool IsUnderCommonRootNode { get; protected internal set; }

		[Browsable(false)]
		public abstract bool IsUnderParticularRootNode { get; protected internal set; }

		[Browsable(false)]
		[MetaInfo(IsParentProperty = true)]
		[WhitIgnorable]
		public abstract IScenario Scenario { get; protected internal set; }

		[Browsable(true)]
		[DisplayName("Имя текущего объекта")]
		[Description("Имя текущего объекта сценария. Используется как идентификатор при разработке шаблона и работе над отчетом")]
		[Category(Manifest.ScenarioDevelopmentCategoryName)]
		public abstract string Name { get; set; }

		[Browsable(true)]
		[DisplayName("Коментарий")]
		[Description("Текст необязательного коментария для текущего объекта")]
		[Category(Manifest.ScenarioDevelopmentCategoryName)]
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public abstract string Comment { get; set; }


		public override sealed bool Equals(object obj)
		{
			var kin = obj as TillerEntityBase;
			if (kin == null) return false;
			return Id.Equals(kin.Id);
		}

		public override sealed int GetHashCode()
		{
			return Id.GetHashCode();
		}

	}
}