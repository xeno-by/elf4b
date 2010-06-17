namespace ObjectMeet.Tiller.Entities.Api
{
	using System;

	public class ScenarioEventArgs : EventArgs
	{
		public IScenarioNode ScenarioNode { get; internal set; }
		public NodeChangeVariant ChangeVariant { get; internal set; }
	}
}