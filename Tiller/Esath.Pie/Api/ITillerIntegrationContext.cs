using DataVault.Core.Api;

namespace Esath.Pie.Api
{
    public interface ITillerIntegrationContext
    {
        IBranch Common { get; }
        IBranch FormulaBeingEdited { get; }

        VarItem SelectVarFromScenario();
        VarItem SelectNodeFromScenario();
    }
}