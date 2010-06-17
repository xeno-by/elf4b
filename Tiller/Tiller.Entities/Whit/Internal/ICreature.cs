namespace ObjectMeet.Tiller.Entities.Whit.Internal
{
	using DataVault.Api;

	[WhitIgnorable]
	internal interface ICreature
	{
		IBranch Model { get; set; }

		ICreature CreateYourself(IBranch branch);

		bool IsShadow { get; }
		
		void Intersect(object creature);
	}
}