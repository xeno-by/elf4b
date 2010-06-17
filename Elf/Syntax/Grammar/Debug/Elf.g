grammar Elf ;

options
{
	output = AST ;
	language = Java ;
}

tokens
{

// Basic concepts of the language
	DEF		= 'def' ;
	RTIMPL		= 'rtimpl' ;
	VAR 		= 'var' ;
	RET 		= 'ret' ;
	
// Flow control structures (loops are TBD)
	IF 		= 'if' ;
	THEN		= 'then' ;
	ELSE		= 'else' ;
	END		= 'end' ;

// Language operators
	POS ;
	NEG ;
	MUL		= '*' ;
	DIV		= '/' ;
	ADD		= '+' ;
	SUB		= '-' ;
	LT		= '<' ;
	GT		= '>' ;
	LTE		= '<=' ;
	GTE		= '>=' ;
	EQ		= '==' ;
	NEQ		= '!=' ;
	NOT		= '!' ;
	AND		= '&&' ;
	OR		= '||' ;
	ASSIGN		= '=' ;
	
// Auxiliary nodes used solely for building AST
	SCRIPT ;
	DECL ;
	CLASS ;
	FUNC ;
	ARGS ;
	BLOCK ;
	PAREXPR ;
	EXPR ;
	CALL ;
	INDEX ;
	
// Punctuators	
	LPAREN		= '(' ;
	RPAREN		= ')' ;
	LBRACK		= '[' ;
	RBRACK		= ']' ;
	COMMA		= ',' ;
	SEMIC		= ';' ;
	DOT		= '.' ;
	
}

@parser::members
{
private final void promoteEOL(ParserRuleReturnScope rule)
{
	// Get current token and its type (the possibly offending token).
	Token lt = input.LT(1);
	int la = lt.getType();
	
	// We only need to promote an EOL when the current token is offending (not a SEMIC, EOF, RBRACE, EOL or MultiLineComment).
	// EOL and MultiLineComment are not offending as they're already promoted in a previous call to this method.
	// Promoting an EOL means switching it from off channel to on channel.
	// A MultiLineComment gets promoted when it contains an EOL.
	if (!(la == SEMIC || la == EOF || la == EOL || la == MultiLineComment))
	{
		// Start on the possition before the current token and scan backwards off channel tokens until the previous on channel token.
		for (int ix = lt.getTokenIndex() - 1; ix > 0; ix--)
		{
			lt = input.get(ix);
			if (lt.getChannel() == Token.DEFAULT_CHANNEL)
			{
				// On channel token found: stop scanning.
				break;
			}
			else if (lt.getType() == EOL || (lt.getType() == MultiLineComment && lt.getText().matches("/.*\r\n|\r|\n")))
			{
				// We found our EOL: promote the token to on channel, position the input on it and reset the rule start.
				lt.setChannel(Token.DEFAULT_CHANNEL);
				input.seek(lt.getTokenIndex());
				if (rule != null)
				{
					rule.start = lt;
				}
				break;
			}
		}
	}
}
}

//
// $<	Lexical Grammar
//

token
	: keyword
	| literal
	| Identifier
	| punctuator
	| oneOfOperators
	;
	
// $<	Whitespaces

WhiteSpace
	: ( '\u0009' | '\u000b' | '\u000c' | '\u0020' )+ { $channel = HIDDEN; }
	;
	
EOL
	: ( ( '\n' '\r'? ) | '\r' ) { $channel = HIDDEN; }
	;
// $>

// $<	Comments

MultiLineComment
	: '/*' ( options { greedy = false; } : . )* '*/' { $channel = HIDDEN; }
	;

SingleLineComment
	: '//' ( ~( '\n' | '\r' ) )* { $channel = HIDDEN; }
	;

// $>

// $<	Punctuators

punctuator
	: LPAREN
	| RPAREN
	| LBRACK
	| RBRACK
	| COMMA
	| SEMIC
	;

// $>

// $<Semicolon

semic
@init
{
	promoteEOL(retval);
}
	: SEMIC
	| EOF
	| EOL 
	| MultiLineComment // (with EOL in it)
	;

// $>

// $<	Keywords

keyword
	: DEF
	| RTIMPL
	| VAR
	| RET
	| IF
	| THEN
	| ELSE
	| END
	;

// $>

// $<	Operators

oneOfOperators
	: nonAssignOperator
	| ASSIGN
	;
	
nonAssignOperator	
	: POS
	| NEG
	| MUL
	| DIV
	| ADD
	| SUB
	| LT
	| GT
	| LTE
	| GTE
	| EQ
	| NEQ
	| NOT
	| AND
	| OR
	;

// $>

// $<	Literals

literal
	: DecimalLiteral
	| StringLiteral
	;

// $<	Numeric literals

fragment DecimalDigit
	: '0'..'9'
	;

fragment DecimalIntegerLiteral
	: '0'
	| '1'..'9' DecimalDigit*
	;

DecimalLiteral
	: DecimalIntegerLiteral '.' DecimalDigit*
	| DecimalIntegerLiteral
	;

// $>

// $<	String literals

fragment BSLASH
	: '\\'
	;
	
fragment DQUOTE
	: '"'
	;
	
fragment SQUOTE
	: '\''
	;

StringLiteral
	: SQUOTE ( ~( SQUOTE | BSLASH | '\n' | '\r' ) | ( BSLASH ( SQUOTE | BSLASH ) ) )* SQUOTE
	| DQUOTE ( ~( DQUOTE | BSLASH | '\n' | '\r' ) | ( BSLASH ( DQUOTE | BSLASH ) ) )* DQUOTE
	;

// $>

// $>
	
// $<	Identifiers

fragment IdentifierStart
	: 'a'..'z' | 'A'..'Z'
	| '_'
	;

fragment IdentifierPart
	: DecimalDigit
	| IdentifierStart
	| DOT
	;
	
fragment Loophole
	: '?'
	;

Identifier
	: Loophole
	| IdentifierStart IdentifierPart*
	;

// $>

// $>

// $>

//
// $<	Expressions
//

expression
	: assignmentExpression
	;

// $<Primary expressions

primaryExpression
	: Identifier
	| literal
	| lpar=LPAREN expression RPAREN -> ^( PAREXPR[$lpar, "PAREXPR"] expression )
	;

// $>

// $<LHS expressions
	
lhsExpression
	:
	(
		primaryExpression 		-> primaryExpression
	)
	(
		arguments			-> ^( CALL $lhsExpression arguments )
		| LBRACK expression RBRACK	-> ^( INDEX $lhsExpression expression )
	)*
	;
	
arguments
	: LPAREN ( assignmentExpression ( COMMA assignmentExpression )* )? RPAREN
	-> ^( ARGS assignmentExpression* )
	;
	
// $>

// $<Unary operators

unaryExpression
	: lhsExpression
	| unaryOperator^ unaryExpression
	;
	
unaryOperator
	: op=ADD { $op.setType(POS); }
	| op=SUB { $op.setType(NEG); }
	| NOT
	;

// $>

// $<Multiplicative operators

multiplicativeExpression
	: unaryExpression ( ( MUL | DIV )^ unaryExpression )*
	;

// $>

// $<Additive operators

additiveExpression
	: multiplicativeExpression ( ( ADD | SUB )^ multiplicativeExpression )*
	;

// $>
	
// $<Relational operators

relationalExpression
	: additiveExpression ( ( LT | GT | LTE | GTE )^ additiveExpression )*
	;

// $>
	
// $<Equality operators

equalityExpression
	: relationalExpression ( ( EQ | NEQ )^ relationalExpression )*
	;

// $>
	
// $<Logical operators

logicalANDExpression
	: equalityExpression ( AND^ equalityExpression )*
	;
	
logicalORExpression
	: logicalANDExpression ( OR^ logicalANDExpression )*
	;
	
// $>

// $<Assignment operators

assignmentExpression
	: logicalORExpression ( ASSIGN^ assignmentExpression )?	
	;
	
// $>

// $>
	
//
// $<	Statements
//

block
	: statement*
	-> ^( BLOCK statement* )
	;
	
statement
	: variableStatement
	| emptyStatement
	| expressionStatement
	| ifStatement
	| returnStatement
	;

// $<Variable statement

variableStatement
	: VAR variableDeclaration ( COMMA variableDeclaration )* semic
	-> ^( VAR variableDeclaration+ )
	;

variableDeclaration
	: Identifier ( ASSIGN^ assignmentExpression )?
	;

// $>

// $<Empty statement

emptyStatement
	: SEMIC!
	;

// $>

// $<Expression statement

expressionStatement
	: expression semic
	-> ^( EXPR expression )
	;

// $>

// $<If statement

ifStatement
	: IF expression THEN block ( ELSE block )? END
	-> ^( IF expression block+ )
	;

// $>
	
// $<Return statement

returnStatement
	: RET^ expression? semic!
	;

// $>
	
// $>
	
//
// $<	Functions
//

functionDefinition
	: elfFunctionDefinition
	;
	
elfFunctionDefinition
	: DEF name=Identifier formalParameterList block END
	-> ^( FUNC ^( DECL $name formalParameterList ) block )
	;
	
formalParameterList
	: LPAREN ( args+=Identifier ( COMMA args+=Identifier )* )? RPAREN
	-> ^( ARGS $args* )
	;
	
// $>
	
//
// $<	Classes
//

classDefinition
	: DEF name=Identifier RTIMPL clrClassRef=Identifier functionDefinition* END
	-> ^( CLASS ^( DECL $name ^( RTIMPL $clrClassRef ) ) functionDefinition* )
	;
	
// $>
		
//
// $<	Scripts
//

script
	: classDefinition*
	-> ^( SCRIPT classDefinition* )
	;

// $>
