namespace ObjectMeet.Tiller.Entities.Whit.Traits
{
	using System;
	using System.IO;
	using Service;

	public static class InteractionProviderTrait
	{
		public static bool IORetryCancel(this IInteractionProvider source, Action action, string operationName)
		{
			if (action == null) return false;
			do
			{
				try
				{
					action();
					break;
				}
				catch (IOException oops)
				{
					if (source == null) return false;
					if (!source.AskRetryCancel("Ошибка ввода-вывода", string.Format("Невозможно завершить операцию \"{0}\".{1}{1}Причина:{1}{2}", operationName, Environment.NewLine, oops.Message))) return false;
				}
				catch (ActionCancelledException)
				{
					return false;
				}
			} while (true);
			return true;
		}

		public static void AlertAndForceCancelAction(this IInteractionProvider source, string message)
		{
			if (source != null)
				source.Alert("Внимание", message);
			throw new ActionCancelledException();
		}
	}
}