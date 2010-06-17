using Elf.Core.Runtime;

namespace Elf.Core
{
    public static class ThreadExecutionHelper
    {
        public static object RunTillEnd(this IEntryPoint ep)
        {
            var thread = (IElfThread)ep.GetEnumerator();

            try
            {
                // we can't use foreach here since it implictly disposes the thread
                // when the execution leaves the scope of foreach
                while (thread.MoveNext()){}
                return thread.VM.Marshaller.Marshal(thread.ExecutionResult);
            }
            finally
            {
                thread.Dispose();
            }
        }
    }
}