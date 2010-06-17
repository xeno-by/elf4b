namespace ObjectMeet.Tiller.Entities.Legacy.V100
{
	using System;
	using System.ComponentModel;
	using DataVault.Api;
	using Whit;

	[Obsolete("Any given program, when running, is obsolete")]
	internal interface IConditionDeclaration
	{
		[Browsable(false)]
		[MetaInfo(Alias = "name", DefaultValue = "")]
		string Name { get; set; }

		[Browsable(false)]
		[MetaInfo(Alias = "text", DefaultValue = "")]
		string Text { get; set; }

		[Browsable(false)]
		[MetaInfo(Alias = "handler", DefaultValue = "")]
		string Handler { get; set; }
	}

	[Obsolete("Subject to be removed")]
	internal class ConditionDeclaration : IConditionDeclaration
	{
		[Browsable(false)]
		public IBranch Model { get; set; }

		[Browsable(false)]
		public string Name
		{
			get { return Model.GetOrCreateValue("name", "").ContentString; }

			set { Model.GetOrCreateValue("name", value).UpdateContent(value); }
		}

		[Browsable(false)]
		public string Text
		{
			get { return Model.GetOrCreateValue("text", "").ContentString; }

			set { Model.GetOrCreateValue("text", value).UpdateContent(value); }
		}

		[Browsable(false)]
		public string Handler
		{
			get { return Model.GetOrCreateValue("handler", "").ContentString; }

			set { Model.GetOrCreateValue("handler", value).UpdateContent(value); }
		}
	}
}