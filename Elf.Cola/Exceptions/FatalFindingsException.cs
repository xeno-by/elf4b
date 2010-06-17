using Elf.Cola.Analysis;

namespace Elf.Cola.Exceptions
{
    // todo. no prettyprint here so far
    public class FatalFindingsException : CocacolaException
    {
        public Analysis.Findings Findings { get; private set; }

        public FatalFindingsException(Findings findings) 
            : base(CocacolaExceptionType.FatalFindings) 
        {
            Findings = findings;
        }
    }
}