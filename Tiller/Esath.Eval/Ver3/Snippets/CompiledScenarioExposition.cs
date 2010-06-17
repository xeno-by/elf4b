using System;

namespace Esath.Eval.Ver3.Snippets
{
    public class CompiledScenarioExposition : IDisposable
    {
        public CompiledScenario Scenario { get; private set; }
        private readonly bool _oldExposed;

        public CompiledScenarioExposition(CompiledScenario scenario)
        {
            Scenario = scenario;

            lock (Scenario.ExpositionSyncRoot)
            {
                _oldExposed = Scenario.IsExposed;
                Scenario.IsExposed = true;
            }
        }

        public void Dispose()
        {
            lock (Scenario.ExpositionSyncRoot)
            {
                // ugly: see CompiledScenarioCache.GetFromPool
                Scenario.IsExposed = false;

                Scenario.FlushCaches();
            }
        }
    }
}