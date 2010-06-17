namespace Elf.Exceptions
{
    public enum ElfExceptionType
    {
        UnexpectedElf,
        UnexpectedRtimpl,

        PrematureEndOfScript,
        SyntaxError,
        InvalidAssignmentLhs,
        LoopholesAreNowDisallowed,

        DuplicateClassLoaded,
        ClassRtimplNotFound,
        DuplicateFuncLoaded,

        UsingVoidValue,
        ConditionNotBoolean,
        CannotResolveInvocation,
        CannotResolveVariable,
        BadVariableName,
        DuplicateVariableName,

        //RtimplLogicsError,
        DivisionByZero,
        OperandsDontSuitMethod,
    }
}