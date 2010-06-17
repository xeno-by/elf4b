namespace ObjectMeet.Tiller.Entities.Api
{
	using Pocso;
	using Whit;

	[DefaultImplementation(typeof (SourceDatumDeclaration))]
	public interface ISourceDatumDeclaration : IDeclaration
	{
		string MeasurementUnit { get; set; }

		string RepositoryValuePath { get; set; }

		string ValueForTesting { get; set; }
	}
}