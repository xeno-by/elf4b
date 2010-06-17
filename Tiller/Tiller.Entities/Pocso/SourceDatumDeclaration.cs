namespace ObjectMeet.Tiller.Entities.Pocso
{
	using System;
	using System.ComponentModel;
	using Api;
	using Whit;

	internal abstract class SourceDatumDeclaration : TillerEntityBase, ISourceDatumDeclaration
	{
		[Browsable(false)]
		public abstract Guid ScenarioNodeId { get; protected internal set; }


		[Browsable(false)]
		public string DeclarationType
		{
			get { return Manifest.DeclarationTypeSourceDatum; }
		}

		#region public stuff

		[Browsable(true)]
		[DisplayName("������� ���������")]
		[Description("���������� ��� ������������, ���������� ���������� ������� ��������� ��������")]
		[Category(Manifest.ReportCreationCategoryName)]
		public abstract string MeasurementUnit { get; set; }

		[Browsable(true)]
		[DisplayName("���� �����������")]
		[Description("���� � ����� �����������, ����������� �������� ����")]
		[Category("��������� ������")]
		[Editor("DataVault.UI.Controls.ComponentModel.ValueEditor, DataVault.UI", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public abstract string RepositoryValuePath { get; set; }

		[Browsable(true)]
		[DisplayName("�������� ��������")]
		[Description("�������� ����, ����������� ��� ���������� ��������")]
		[Category(Manifest.ScenarioDevelopmentCategoryName)]
		[Editor("ObjectMeet.Tiller.Gui.SourceDatumPropertyEditor, ObjectMeet.Tiller.Gui", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public abstract string ValueForTesting { get; set; }

		[Browsable(false)]
		[MetaInfo(DefaultValue = Manifest.DataTypeString)]
		public abstract string DataType { get; set; }

		[Browsable(true)]
		[DisplayName("����������� ��������")]
		[Description("��������� ������� ����� �� ����������� �������� ������� ��������� � �����������")]
		[Category(Manifest.ScenarioDevelopmentCategoryName)]
		public abstract string IsExportable { get; set; }

		[Browsable(true)]
		[DisplayName("���")]
		[Description("�������������� ������� ��������� �������� �������. ����������, ����� ������� �������� ������������� � ������")]
		[Category(Manifest.ReportGenerationCategoryName)]
		public string HumanType
		{
			get { return Manifest.GetHumanTextFromDataTypeToken(DataType); }
		}

		#endregion
	}
}