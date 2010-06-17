namespace ObjectMeet.Tiller.Entities.Api
{
	using Pocso;
	using Whit;

	[DefaultImplementation(typeof (FormulaDeclaration))]
	public interface IFormulaDeclaration : IDeclaration
	{
		string HumanSource { get; set; }

		string ElfSource { get; set; }
	}
}