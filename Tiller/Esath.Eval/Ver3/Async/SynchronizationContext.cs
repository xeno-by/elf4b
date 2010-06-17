using System;

namespace Esath.Eval.Ver3.Async
{
    public class SynchronizationContext
    {
        public VaultCompiler Compiler { get; private set; }
        public int WorkerSeq { get; private set; }
        public int LastSuccessfulWorkerSeq { get; private set; }
        public UInt64 Revision { get; private set; }

        protected SynchronizationContext(VaultCompiler compiler)
        {
            lock (compiler._barrier)
            {
                Compiler = compiler;
                WorkerSeq = compiler._nextWorkerSeq++;
                LastSuccessfulWorkerSeq = compiler._lastSuccessfulWorkerSeq;
                Revision = compiler.Vault.Revision;
            }
        }
    }
}