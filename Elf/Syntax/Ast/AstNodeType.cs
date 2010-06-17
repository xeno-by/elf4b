namespace Elf.Syntax.Ast
{
    public enum AstNodeType
    {
        Script,
        ClassDef,
        FuncDef,

        Block,

        EmptyStatement,
        ExpressionStatement,
        VarStatement,
        IfStatement,
        ReturnStatement,

        LiteralExpression,
        VariableExpression,
        AssignmentExpression,
        InvocationExpression
    }
}