namespace ObjectMeet.Tiller.Entities.Service
{
	public interface IInteractionProvider
	{
		bool AskRetryCancel(string title, string message);

		bool AskConfirmation(string title, string message);

		void Alert(string title, string message);
	}
}