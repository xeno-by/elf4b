// $ANTLR 3.1.1 Elf.g 2009-02-26 22:55:54
// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 168, 219
// Unreachable code detected.
#pragma warning disable 162
namespace  Elf.Syntax.Grammar 
{

using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;



using Antlr.Runtime.Tree;

public partial class ElfParser : Parser
{
    public static readonly string[] tokenNames = new string[] 
	{
        "<invalid>", 
		"<EOR>", 
		"<DOWN>", 
		"<UP>", 
		"DEF", 
		"RTIMPL", 
		"VAR", 
		"RET", 
		"IF", 
		"THEN", 
		"ELSE", 
		"END", 
		"POS", 
		"NEG", 
		"MUL", 
		"DIV", 
		"ADD", 
		"SUB", 
		"POW", 
		"LT", 
		"GT", 
		"LTE", 
		"GTE", 
		"EQ", 
		"NEQ", 
		"NOT", 
		"AND", 
		"OR", 
		"ASSIGN", 
		"SCRIPT", 
		"DECL", 
		"CLASS", 
		"FUNC", 
		"ARGS", 
		"BLOCK", 
		"PAREXPR", 
		"EXPR", 
		"CALL", 
		"INDEX", 
		"LPAREN", 
		"RPAREN", 
		"LBRACK", 
		"RBRACK", 
		"COMMA", 
		"SEMIC", 
		"DOT", 
		"Identifier", 
		"WhiteSpace", 
		"EOL", 
		"MultiLineComment", 
		"SingleLineComment", 
		"DecimalLiteral", 
		"StringLiteral", 
		"DecimalDigit", 
		"DecimalIntegerLiteral", 
		"BSLASH", 
		"DQUOTE", 
		"SQUOTE", 
		"IdentifierStart", 
		"IdentifierPart", 
		"Loophole"
    };

    public const int LT = 19;
    public const int CLASS = 31;
    public const int LBRACK = 41;
    public const int DEF = 4;
    public const int SingleLineComment = 50;
    public const int GTE = 22;
    public const int DQUOTE = 56;
    public const int SUB = 17;
    public const int NOT = 25;
    public const int DecimalDigit = 53;
    public const int AND = 26;
    public const int DecimalIntegerLiteral = 54;
    public const int EOF = -1;
    public const int LTE = 21;
    public const int Identifier = 46;
    public const int LPAREN = 39;
    public const int IF = 8;
    public const int INDEX = 38;
    public const int RET = 7;
    public const int RPAREN = 40;
    public const int EOL = 48;
    public const int EXPR = 36;
    public const int Loophole = 60;
    public const int POW = 18;
    public const int THEN = 9;
    public const int POS = 12;
    public const int COMMA = 43;
    public const int ARGS = 33;
    public const int WhiteSpace = 47;
    public const int VAR = 6;
    public const int EQ = 23;
    public const int DOT = 45;
    public const int IdentifierPart = 59;
    public const int MultiLineComment = 49;
    public const int ADD = 16;
    public const int RBRACK = 42;
    public const int SCRIPT = 29;
    public const int ELSE = 10;
    public const int IdentifierStart = 58;
    public const int SQUOTE = 57;
    public const int DecimalLiteral = 51;
    public const int MUL = 14;
    public const int StringLiteral = 52;
    public const int NEQ = 24;
    public const int PAREXPR = 35;
    public const int DECL = 30;
    public const int FUNC = 32;
    public const int BLOCK = 34;
    public const int NEG = 13;
    public const int OR = 27;
    public const int ASSIGN = 28;
    public const int GT = 20;
    public const int CALL = 37;
    public const int DIV = 15;
    public const int RTIMPL = 5;
    public const int SEMIC = 44;
    public const int END = 11;
    public const int BSLASH = 55;

    // delegates
    // delegators



        public ElfParser(ITokenStream input)
    		: this(input, new RecognizerSharedState()) {
        }

        public ElfParser(ITokenStream input, RecognizerSharedState state)
    		: base(input, state) {
            InitializeCyclicDFAs();

             
        }
        
    protected ITreeAdaptor adaptor = new CommonTreeAdaptor();

    public ITreeAdaptor TreeAdaptor
    {
        get { return this.adaptor; }
        set {
    	this.adaptor = value;
    	}
    }

    override public string[] TokenNames {
		get { return ElfParser.tokenNames; }
    }

    override public string GrammarFileName {
		get { return "Elf.g"; }
    }


    public override void ReportError(RecognitionException e)
    {
    	throw Elf.Exceptions.Parser.RecognitionExceptionHelper.Report(this, e);
    }

    private void PromoteEOL(ParserRuleReturnScope rule)
    {
    	// Get current token and its type (the possibly offending token).
    	IToken lt = input.LT(1);
    	int la = lt.Type;
    	
    	// We only need to promote an EOL when the current token is offending (not a SEMIC, EOF, RBRACE, EOL or MultiLineComment).
    	// EOL and MultiLineComment are not offending as they're already promoted in a previous call to this method.
    	// Promoting an EOL means switching it from off channel to on channel.
    	// A MultiLineComment gets promoted when it contains an EOL.
    	if (!(la == SEMIC || la == EOF || la == EOL || la == MultiLineComment))
    	{
    		// Start on the possition before the current token and scan backwards off channel tokens until the previous on channel token.
    		for (int ix = lt.TokenIndex - 1; ix > 0; ix--)
    		{
    			lt = input.Get(ix);
    			if (lt.Channel == Token.DEFAULT_CHANNEL)
    			{
    				// On channel token found: stop scanning.
    				break;
    			}
    			else if (lt.Type == EOL || (lt.Type == MultiLineComment && System.Text.RegularExpressions.Regex.IsMatch(lt.Text, @".*\r\n|\r|\n")))
    			{
    				// We found our EOL: promote the token to on channel, position the input on it and reset the rule start.
    				lt.Channel = Token.DEFAULT_CHANNEL;
    				input.Seek(lt.TokenIndex);
    				if (rule != null)
    				{
    					rule.Start = lt;
    				}
    				break;
    			}
    		}
    	}
    }


    public class token_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "token"
    // Elf.g:125:1: token : ( keyword | literal | Identifier | punctuator | oneOfOperators );
    public ElfParser.token_return token() // throws RecognitionException [1]
    {   
        ElfParser.token_return retval = new ElfParser.token_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken Identifier3 = null;
        ElfParser.keyword_return keyword1 = default(ElfParser.keyword_return);

        ElfParser.literal_return literal2 = default(ElfParser.literal_return);

        ElfParser.punctuator_return punctuator4 = default(ElfParser.punctuator_return);

        ElfParser.oneOfOperators_return oneOfOperators5 = default(ElfParser.oneOfOperators_return);


        object Identifier3_tree=null;

        try 
    	{
            // Elf.g:126:2: ( keyword | literal | Identifier | punctuator | oneOfOperators )
            int alt1 = 5;
            switch ( input.LA(1) ) 
            {
            case DEF:
            case RTIMPL:
            case VAR:
            case RET:
            case IF:
            case THEN:
            case ELSE:
            case END:
            	{
                alt1 = 1;
                }
                break;
            case DecimalLiteral:
            case StringLiteral:
            	{
                alt1 = 2;
                }
                break;
            case Identifier:
            	{
                alt1 = 3;
                }
                break;
            case LPAREN:
            case RPAREN:
            case LBRACK:
            case RBRACK:
            case COMMA:
            case SEMIC:
            	{
                alt1 = 4;
                }
                break;
            case POS:
            case NEG:
            case MUL:
            case DIV:
            case ADD:
            case SUB:
            case POW:
            case LT:
            case GT:
            case LTE:
            case GTE:
            case EQ:
            case NEQ:
            case NOT:
            case AND:
            case OR:
            case ASSIGN:
            	{
                alt1 = 5;
                }
                break;
            	default:
            	    NoViableAltException nvae_d1s0 =
            	        new NoViableAltException("", 1, 0, input);

            	    throw nvae_d1s0;
            }

            switch (alt1) 
            {
                case 1 :
                    // Elf.g:126:4: keyword
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_keyword_in_token455);
                    	keyword1 = keyword();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, keyword1.Tree);

                    }
                    break;
                case 2 :
                    // Elf.g:127:4: literal
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_literal_in_token460);
                    	literal2 = literal();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, literal2.Tree);

                    }
                    break;
                case 3 :
                    // Elf.g:128:4: Identifier
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	Identifier3=(IToken)Match(input,Identifier,FOLLOW_Identifier_in_token465); 
                    		Identifier3_tree = (object)adaptor.Create(Identifier3);
                    		adaptor.AddChild(root_0, Identifier3_tree);


                    }
                    break;
                case 4 :
                    // Elf.g:129:4: punctuator
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_punctuator_in_token470);
                    	punctuator4 = punctuator();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, punctuator4.Tree);

                    }
                    break;
                case 5 :
                    // Elf.g:130:4: oneOfOperators
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_oneOfOperators_in_token475);
                    	oneOfOperators5 = oneOfOperators();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, oneOfOperators5.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "token"

    public class punctuator_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "punctuator"
    // Elf.g:158:1: punctuator : ( LPAREN | RPAREN | LBRACK | RBRACK | COMMA | SEMIC );
    public ElfParser.punctuator_return punctuator() // throws RecognitionException [1]
    {   
        ElfParser.punctuator_return retval = new ElfParser.punctuator_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set6 = null;

        object set6_tree=null;

        try 
    	{
            // Elf.g:159:2: ( LPAREN | RPAREN | LBRACK | RBRACK | COMMA | SEMIC )
            // Elf.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set6 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= LPAREN && input.LA(1) <= SEMIC) ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, (object)adaptor.Create(set6));
            	    state.errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "punctuator"

    public class semic_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "semic"
    // Elf.g:171:1: semic : ( SEMIC | EOF | EOL | MultiLineComment );
    public ElfParser.semic_return semic() // throws RecognitionException [1]
    {   
        ElfParser.semic_return retval = new ElfParser.semic_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set7 = null;

        object set7_tree=null;


        	PromoteEOL(retval);

        try 
    	{
            // Elf.g:176:2: ( SEMIC | EOF | EOL | MultiLineComment )
            // Elf.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set7 = (IToken)input.LT(1);
            	if ( input.LA(1) == EOF || input.LA(1) == SEMIC || (input.LA(1) >= EOL && input.LA(1) <= MultiLineComment) ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, (object)adaptor.Create(set7));
            	    state.errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "semic"

    public class keyword_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "keyword"
    // Elf.g:186:1: keyword : ( DEF | RTIMPL | VAR | RET | IF | THEN | ELSE | END );
    public ElfParser.keyword_return keyword() // throws RecognitionException [1]
    {   
        ElfParser.keyword_return retval = new ElfParser.keyword_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set8 = null;

        object set8_tree=null;

        try 
    	{
            // Elf.g:187:2: ( DEF | RTIMPL | VAR | RET | IF | THEN | ELSE | END )
            // Elf.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set8 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= DEF && input.LA(1) <= END) ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, (object)adaptor.Create(set8));
            	    state.errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "keyword"

    public class oneOfOperators_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "oneOfOperators"
    // Elf.g:201:1: oneOfOperators : ( nonAssignOperator | ASSIGN );
    public ElfParser.oneOfOperators_return oneOfOperators() // throws RecognitionException [1]
    {   
        ElfParser.oneOfOperators_return retval = new ElfParser.oneOfOperators_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken ASSIGN10 = null;
        ElfParser.nonAssignOperator_return nonAssignOperator9 = default(ElfParser.nonAssignOperator_return);


        object ASSIGN10_tree=null;

        try 
    	{
            // Elf.g:202:2: ( nonAssignOperator | ASSIGN )
            int alt2 = 2;
            int LA2_0 = input.LA(1);

            if ( ((LA2_0 >= POS && LA2_0 <= OR)) )
            {
                alt2 = 1;
            }
            else if ( (LA2_0 == ASSIGN) )
            {
                alt2 = 2;
            }
            else 
            {
                NoViableAltException nvae_d2s0 =
                    new NoViableAltException("", 2, 0, input);

                throw nvae_d2s0;
            }
            switch (alt2) 
            {
                case 1 :
                    // Elf.g:202:4: nonAssignOperator
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_nonAssignOperator_in_oneOfOperators746);
                    	nonAssignOperator9 = nonAssignOperator();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, nonAssignOperator9.Tree);

                    }
                    break;
                case 2 :
                    // Elf.g:203:4: ASSIGN
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	ASSIGN10=(IToken)Match(input,ASSIGN,FOLLOW_ASSIGN_in_oneOfOperators751); 
                    		ASSIGN10_tree = (object)adaptor.Create(ASSIGN10);
                    		adaptor.AddChild(root_0, ASSIGN10_tree);


                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "oneOfOperators"

    public class nonAssignOperator_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "nonAssignOperator"
    // Elf.g:206:1: nonAssignOperator : ( POS | NEG | MUL | DIV | ADD | SUB | POW | LT | GT | LTE | GTE | EQ | NEQ | NOT | AND | OR );
    public ElfParser.nonAssignOperator_return nonAssignOperator() // throws RecognitionException [1]
    {   
        ElfParser.nonAssignOperator_return retval = new ElfParser.nonAssignOperator_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set11 = null;

        object set11_tree=null;

        try 
    	{
            // Elf.g:207:2: ( POS | NEG | MUL | DIV | ADD | SUB | POW | LT | GT | LTE | GTE | EQ | NEQ | NOT | AND | OR )
            // Elf.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set11 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= POS && input.LA(1) <= OR) ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, (object)adaptor.Create(set11));
            	    state.errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "nonAssignOperator"

    public class literal_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "literal"
    // Elf.g:229:1: literal : ( DecimalLiteral | StringLiteral );
    public ElfParser.literal_return literal() // throws RecognitionException [1]
    {   
        ElfParser.literal_return retval = new ElfParser.literal_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set12 = null;

        object set12_tree=null;

        try 
    	{
            // Elf.g:230:2: ( DecimalLiteral | StringLiteral )
            // Elf.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set12 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= DecimalLiteral && input.LA(1) <= StringLiteral) ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, (object)adaptor.Create(set12));
            	    state.errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "literal"

    public class expression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "expression"
    // Elf.g:311:1: expression : assignmentExpression ;
    public ElfParser.expression_return expression() // throws RecognitionException [1]
    {   
        ElfParser.expression_return retval = new ElfParser.expression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        ElfParser.assignmentExpression_return assignmentExpression13 = default(ElfParser.assignmentExpression_return);



        try 
    	{
            // Elf.g:312:2: ( assignmentExpression )
            // Elf.g:312:4: assignmentExpression
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_assignmentExpression_in_expression1206);
            	assignmentExpression13 = assignmentExpression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, assignmentExpression13.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "expression"

    public class primaryExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "primaryExpression"
    // Elf.g:317:1: primaryExpression : ( Identifier | literal | lpar= LPAREN expression RPAREN -> ^( PAREXPR[$lpar, \"PAREXPR\"] expression ) );
    public ElfParser.primaryExpression_return primaryExpression() // throws RecognitionException [1]
    {   
        ElfParser.primaryExpression_return retval = new ElfParser.primaryExpression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken lpar = null;
        IToken Identifier14 = null;
        IToken RPAREN17 = null;
        ElfParser.literal_return literal15 = default(ElfParser.literal_return);

        ElfParser.expression_return expression16 = default(ElfParser.expression_return);


        object lpar_tree=null;
        object Identifier14_tree=null;
        object RPAREN17_tree=null;
        RewriteRuleTokenStream stream_RPAREN = new RewriteRuleTokenStream(adaptor,"token RPAREN");
        RewriteRuleTokenStream stream_LPAREN = new RewriteRuleTokenStream(adaptor,"token LPAREN");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try 
    	{
            // Elf.g:318:2: ( Identifier | literal | lpar= LPAREN expression RPAREN -> ^( PAREXPR[$lpar, \"PAREXPR\"] expression ) )
            int alt3 = 3;
            switch ( input.LA(1) ) 
            {
            case Identifier:
            	{
                alt3 = 1;
                }
                break;
            case DecimalLiteral:
            case StringLiteral:
            	{
                alt3 = 2;
                }
                break;
            case LPAREN:
            	{
                alt3 = 3;
                }
                break;
            	default:
            	    NoViableAltException nvae_d3s0 =
            	        new NoViableAltException("", 3, 0, input);

            	    throw nvae_d3s0;
            }

            switch (alt3) 
            {
                case 1 :
                    // Elf.g:318:4: Identifier
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	Identifier14=(IToken)Match(input,Identifier,FOLLOW_Identifier_in_primaryExpression1219); 
                    		Identifier14_tree = (object)adaptor.Create(Identifier14);
                    		adaptor.AddChild(root_0, Identifier14_tree);


                    }
                    break;
                case 2 :
                    // Elf.g:319:4: literal
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_literal_in_primaryExpression1224);
                    	literal15 = literal();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, literal15.Tree);

                    }
                    break;
                case 3 :
                    // Elf.g:320:4: lpar= LPAREN expression RPAREN
                    {
                    	lpar=(IToken)Match(input,LPAREN,FOLLOW_LPAREN_in_primaryExpression1231);  
                    	stream_LPAREN.Add(lpar);

                    	PushFollow(FOLLOW_expression_in_primaryExpression1233);
                    	expression16 = expression();
                    	state.followingStackPointer--;

                    	stream_expression.Add(expression16.Tree);
                    	RPAREN17=(IToken)Match(input,RPAREN,FOLLOW_RPAREN_in_primaryExpression1235);  
                    	stream_RPAREN.Add(RPAREN17);



                    	// AST REWRITE
                    	// elements:          expression
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 320:34: -> ^( PAREXPR[$lpar, \"PAREXPR\"] expression )
                    	{
                    	    // Elf.g:320:37: ^( PAREXPR[$lpar, \"PAREXPR\"] expression )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(PAREXPR, lpar, "PAREXPR"), root_1);

                    	    adaptor.AddChild(root_1, stream_expression.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "primaryExpression"

    public class lhsExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "lhsExpression"
    // Elf.g:327:1: lhsExpression : ( primaryExpression -> primaryExpression ) ( arguments -> ^( CALL $lhsExpression arguments ) | LBRACK expression RBRACK -> ^( INDEX $lhsExpression expression ) )* ;
    public ElfParser.lhsExpression_return lhsExpression() // throws RecognitionException [1]
    {   
        ElfParser.lhsExpression_return retval = new ElfParser.lhsExpression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken LBRACK20 = null;
        IToken RBRACK22 = null;
        ElfParser.primaryExpression_return primaryExpression18 = default(ElfParser.primaryExpression_return);

        ElfParser.arguments_return arguments19 = default(ElfParser.arguments_return);

        ElfParser.expression_return expression21 = default(ElfParser.expression_return);


        object LBRACK20_tree=null;
        object RBRACK22_tree=null;
        RewriteRuleTokenStream stream_RBRACK = new RewriteRuleTokenStream(adaptor,"token RBRACK");
        RewriteRuleTokenStream stream_LBRACK = new RewriteRuleTokenStream(adaptor,"token LBRACK");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        RewriteRuleSubtreeStream stream_arguments = new RewriteRuleSubtreeStream(adaptor,"rule arguments");
        RewriteRuleSubtreeStream stream_primaryExpression = new RewriteRuleSubtreeStream(adaptor,"rule primaryExpression");
        try 
    	{
            // Elf.g:328:2: ( ( primaryExpression -> primaryExpression ) ( arguments -> ^( CALL $lhsExpression arguments ) | LBRACK expression RBRACK -> ^( INDEX $lhsExpression expression ) )* )
            // Elf.g:329:2: ( primaryExpression -> primaryExpression ) ( arguments -> ^( CALL $lhsExpression arguments ) | LBRACK expression RBRACK -> ^( INDEX $lhsExpression expression ) )*
            {
            	// Elf.g:329:2: ( primaryExpression -> primaryExpression )
            	// Elf.g:330:3: primaryExpression
            	{
            		PushFollow(FOLLOW_primaryExpression_in_lhsExpression1267);
            		primaryExpression18 = primaryExpression();
            		state.followingStackPointer--;

            		stream_primaryExpression.Add(primaryExpression18.Tree);


            		// AST REWRITE
            		// elements:          primaryExpression
            		// token labels:      
            		// rule labels:       retval
            		// token list labels: 
            		// rule list labels:  
            		retval.Tree = root_0;
            		RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));

            		root_0 = (object)adaptor.GetNilNode();
            		// 330:23: -> primaryExpression
            		{
            		    adaptor.AddChild(root_0, stream_primaryExpression.NextTree());

            		}

            		retval.Tree = root_0;retval.Tree = root_0;
            	}

            	// Elf.g:332:2: ( arguments -> ^( CALL $lhsExpression arguments ) | LBRACK expression RBRACK -> ^( INDEX $lhsExpression expression ) )*
            	do 
            	{
            	    int alt4 = 3;
            	    int LA4_0 = input.LA(1);

            	    if ( (LA4_0 == LPAREN) )
            	    {
            	        alt4 = 1;
            	    }
            	    else if ( (LA4_0 == LBRACK) )
            	    {
            	        alt4 = 2;
            	    }


            	    switch (alt4) 
            		{
            			case 1 :
            			    // Elf.g:333:3: arguments
            			    {
            			    	PushFollow(FOLLOW_arguments_in_lhsExpression1283);
            			    	arguments19 = arguments();
            			    	state.followingStackPointer--;

            			    	stream_arguments.Add(arguments19.Tree);


            			    	// AST REWRITE
            			    	// elements:          arguments, lhsExpression
            			    	// token labels:      
            			    	// rule labels:       retval
            			    	// token list labels: 
            			    	// rule list labels:  
            			    	retval.Tree = root_0;
            			    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));

            			    	root_0 = (object)adaptor.GetNilNode();
            			    	// 333:15: -> ^( CALL $lhsExpression arguments )
            			    	{
            			    	    // Elf.g:333:18: ^( CALL $lhsExpression arguments )
            			    	    {
            			    	    object root_1 = (object)adaptor.GetNilNode();
            			    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(CALL, "CALL"), root_1);

            			    	    adaptor.AddChild(root_1, stream_retval.NextTree());
            			    	    adaptor.AddChild(root_1, stream_arguments.NextTree());

            			    	    adaptor.AddChild(root_0, root_1);
            			    	    }

            			    	}

            			    	retval.Tree = root_0;retval.Tree = root_0;
            			    }
            			    break;
            			case 2 :
            			    // Elf.g:334:5: LBRACK expression RBRACK
            			    {
            			    	LBRACK20=(IToken)Match(input,LBRACK,FOLLOW_LBRACK_in_lhsExpression1304);  
            			    	stream_LBRACK.Add(LBRACK20);

            			    	PushFollow(FOLLOW_expression_in_lhsExpression1306);
            			    	expression21 = expression();
            			    	state.followingStackPointer--;

            			    	stream_expression.Add(expression21.Tree);
            			    	RBRACK22=(IToken)Match(input,RBRACK,FOLLOW_RBRACK_in_lhsExpression1308);  
            			    	stream_RBRACK.Add(RBRACK22);



            			    	// AST REWRITE
            			    	// elements:          lhsExpression, expression
            			    	// token labels:      
            			    	// rule labels:       retval
            			    	// token list labels: 
            			    	// rule list labels:  
            			    	retval.Tree = root_0;
            			    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));

            			    	root_0 = (object)adaptor.GetNilNode();
            			    	// 334:30: -> ^( INDEX $lhsExpression expression )
            			    	{
            			    	    // Elf.g:334:33: ^( INDEX $lhsExpression expression )
            			    	    {
            			    	    object root_1 = (object)adaptor.GetNilNode();
            			    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(INDEX, "INDEX"), root_1);

            			    	    adaptor.AddChild(root_1, stream_retval.NextTree());
            			    	    adaptor.AddChild(root_1, stream_expression.NextTree());

            			    	    adaptor.AddChild(root_0, root_1);
            			    	    }

            			    	}

            			    	retval.Tree = root_0;retval.Tree = root_0;
            			    }
            			    break;

            			default:
            			    goto loop4;
            	    }
            	} while (true);

            	loop4:
            		;	// Stops C# compiler whining that label 'loop4' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "lhsExpression"

    public class arguments_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "arguments"
    // Elf.g:338:1: arguments : LPAREN ( assignmentExpression ( COMMA assignmentExpression )* )? RPAREN -> ^( ARGS ( assignmentExpression )* ) ;
    public ElfParser.arguments_return arguments() // throws RecognitionException [1]
    {   
        ElfParser.arguments_return retval = new ElfParser.arguments_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken LPAREN23 = null;
        IToken COMMA25 = null;
        IToken RPAREN27 = null;
        ElfParser.assignmentExpression_return assignmentExpression24 = default(ElfParser.assignmentExpression_return);

        ElfParser.assignmentExpression_return assignmentExpression26 = default(ElfParser.assignmentExpression_return);


        object LPAREN23_tree=null;
        object COMMA25_tree=null;
        object RPAREN27_tree=null;
        RewriteRuleTokenStream stream_RPAREN = new RewriteRuleTokenStream(adaptor,"token RPAREN");
        RewriteRuleTokenStream stream_COMMA = new RewriteRuleTokenStream(adaptor,"token COMMA");
        RewriteRuleTokenStream stream_LPAREN = new RewriteRuleTokenStream(adaptor,"token LPAREN");
        RewriteRuleSubtreeStream stream_assignmentExpression = new RewriteRuleSubtreeStream(adaptor,"rule assignmentExpression");
        try 
    	{
            // Elf.g:339:2: ( LPAREN ( assignmentExpression ( COMMA assignmentExpression )* )? RPAREN -> ^( ARGS ( assignmentExpression )* ) )
            // Elf.g:339:4: LPAREN ( assignmentExpression ( COMMA assignmentExpression )* )? RPAREN
            {
            	LPAREN23=(IToken)Match(input,LPAREN,FOLLOW_LPAREN_in_arguments1337);  
            	stream_LPAREN.Add(LPAREN23);

            	// Elf.g:339:11: ( assignmentExpression ( COMMA assignmentExpression )* )?
            	int alt6 = 2;
            	int LA6_0 = input.LA(1);

            	if ( ((LA6_0 >= ADD && LA6_0 <= SUB) || LA6_0 == NOT || LA6_0 == LPAREN || LA6_0 == Identifier || (LA6_0 >= DecimalLiteral && LA6_0 <= StringLiteral)) )
            	{
            	    alt6 = 1;
            	}
            	switch (alt6) 
            	{
            	    case 1 :
            	        // Elf.g:339:13: assignmentExpression ( COMMA assignmentExpression )*
            	        {
            	        	PushFollow(FOLLOW_assignmentExpression_in_arguments1341);
            	        	assignmentExpression24 = assignmentExpression();
            	        	state.followingStackPointer--;

            	        	stream_assignmentExpression.Add(assignmentExpression24.Tree);
            	        	// Elf.g:339:34: ( COMMA assignmentExpression )*
            	        	do 
            	        	{
            	        	    int alt5 = 2;
            	        	    int LA5_0 = input.LA(1);

            	        	    if ( (LA5_0 == COMMA) )
            	        	    {
            	        	        alt5 = 1;
            	        	    }


            	        	    switch (alt5) 
            	        		{
            	        			case 1 :
            	        			    // Elf.g:339:36: COMMA assignmentExpression
            	        			    {
            	        			    	COMMA25=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_arguments1345);  
            	        			    	stream_COMMA.Add(COMMA25);

            	        			    	PushFollow(FOLLOW_assignmentExpression_in_arguments1347);
            	        			    	assignmentExpression26 = assignmentExpression();
            	        			    	state.followingStackPointer--;

            	        			    	stream_assignmentExpression.Add(assignmentExpression26.Tree);

            	        			    }
            	        			    break;

            	        			default:
            	        			    goto loop5;
            	        	    }
            	        	} while (true);

            	        	loop5:
            	        		;	// Stops C# compiler whining that label 'loop5' has no statements


            	        }
            	        break;

            	}

            	RPAREN27=(IToken)Match(input,RPAREN,FOLLOW_RPAREN_in_arguments1355);  
            	stream_RPAREN.Add(RPAREN27);



            	// AST REWRITE
            	// elements:          assignmentExpression
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));

            	root_0 = (object)adaptor.GetNilNode();
            	// 340:2: -> ^( ARGS ( assignmentExpression )* )
            	{
            	    // Elf.g:340:5: ^( ARGS ( assignmentExpression )* )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ARGS, "ARGS"), root_1);

            	    // Elf.g:340:13: ( assignmentExpression )*
            	    while ( stream_assignmentExpression.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_assignmentExpression.NextTree());

            	    }
            	    stream_assignmentExpression.Reset();

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "arguments"

    public class unaryExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "unaryExpression"
    // Elf.g:347:1: unaryExpression : ( lhsExpression | unaryOperator unaryExpression );
    public ElfParser.unaryExpression_return unaryExpression() // throws RecognitionException [1]
    {   
        ElfParser.unaryExpression_return retval = new ElfParser.unaryExpression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        ElfParser.lhsExpression_return lhsExpression28 = default(ElfParser.lhsExpression_return);

        ElfParser.unaryOperator_return unaryOperator29 = default(ElfParser.unaryOperator_return);

        ElfParser.unaryExpression_return unaryExpression30 = default(ElfParser.unaryExpression_return);



        try 
    	{
            // Elf.g:348:2: ( lhsExpression | unaryOperator unaryExpression )
            int alt7 = 2;
            int LA7_0 = input.LA(1);

            if ( (LA7_0 == LPAREN || LA7_0 == Identifier || (LA7_0 >= DecimalLiteral && LA7_0 <= StringLiteral)) )
            {
                alt7 = 1;
            }
            else if ( ((LA7_0 >= ADD && LA7_0 <= SUB) || LA7_0 == NOT) )
            {
                alt7 = 2;
            }
            else 
            {
                NoViableAltException nvae_d7s0 =
                    new NoViableAltException("", 7, 0, input);

                throw nvae_d7s0;
            }
            switch (alt7) 
            {
                case 1 :
                    // Elf.g:348:4: lhsExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_lhsExpression_in_unaryExpression1383);
                    	lhsExpression28 = lhsExpression();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, lhsExpression28.Tree);

                    }
                    break;
                case 2 :
                    // Elf.g:349:4: unaryOperator unaryExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_unaryOperator_in_unaryExpression1388);
                    	unaryOperator29 = unaryOperator();
                    	state.followingStackPointer--;

                    	root_0 = (object)adaptor.BecomeRoot(unaryOperator29.Tree, root_0);
                    	PushFollow(FOLLOW_unaryExpression_in_unaryExpression1391);
                    	unaryExpression30 = unaryExpression();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, unaryExpression30.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "unaryExpression"

    public class unaryOperator_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "unaryOperator"
    // Elf.g:352:1: unaryOperator : (op= ADD | op= SUB | NOT );
    public ElfParser.unaryOperator_return unaryOperator() // throws RecognitionException [1]
    {   
        ElfParser.unaryOperator_return retval = new ElfParser.unaryOperator_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken op = null;
        IToken NOT31 = null;

        object op_tree=null;
        object NOT31_tree=null;

        try 
    	{
            // Elf.g:353:2: (op= ADD | op= SUB | NOT )
            int alt8 = 3;
            switch ( input.LA(1) ) 
            {
            case ADD:
            	{
                alt8 = 1;
                }
                break;
            case SUB:
            	{
                alt8 = 2;
                }
                break;
            case NOT:
            	{
                alt8 = 3;
                }
                break;
            	default:
            	    NoViableAltException nvae_d8s0 =
            	        new NoViableAltException("", 8, 0, input);

            	    throw nvae_d8s0;
            }

            switch (alt8) 
            {
                case 1 :
                    // Elf.g:353:4: op= ADD
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	op=(IToken)Match(input,ADD,FOLLOW_ADD_in_unaryOperator1405); 
                    		op_tree = (object)adaptor.Create(op);
                    		adaptor.AddChild(root_0, op_tree);

                    	 op.Type = POS; 

                    }
                    break;
                case 2 :
                    // Elf.g:354:4: op= SUB
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	op=(IToken)Match(input,SUB,FOLLOW_SUB_in_unaryOperator1414); 
                    		op_tree = (object)adaptor.Create(op);
                    		adaptor.AddChild(root_0, op_tree);

                    	 op.Type = NEG; 

                    }
                    break;
                case 3 :
                    // Elf.g:355:4: NOT
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	NOT31=(IToken)Match(input,NOT,FOLLOW_NOT_in_unaryOperator1421); 
                    		NOT31_tree = (object)adaptor.Create(NOT31);
                    		adaptor.AddChild(root_0, NOT31_tree);


                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "unaryOperator"

    public class powerExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "powerExpression"
    // Elf.g:362:1: powerExpression : unaryExpression ( POW unaryExpression )* ;
    public ElfParser.powerExpression_return powerExpression() // throws RecognitionException [1]
    {   
        ElfParser.powerExpression_return retval = new ElfParser.powerExpression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken POW33 = null;
        ElfParser.unaryExpression_return unaryExpression32 = default(ElfParser.unaryExpression_return);

        ElfParser.unaryExpression_return unaryExpression34 = default(ElfParser.unaryExpression_return);


        object POW33_tree=null;

        try 
    	{
            // Elf.g:363:2: ( unaryExpression ( POW unaryExpression )* )
            // Elf.g:363:4: unaryExpression ( POW unaryExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_unaryExpression_in_powerExpression1436);
            	unaryExpression32 = unaryExpression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, unaryExpression32.Tree);
            	// Elf.g:363:20: ( POW unaryExpression )*
            	do 
            	{
            	    int alt9 = 2;
            	    int LA9_0 = input.LA(1);

            	    if ( (LA9_0 == POW) )
            	    {
            	        alt9 = 1;
            	    }


            	    switch (alt9) 
            		{
            			case 1 :
            			    // Elf.g:363:22: POW unaryExpression
            			    {
            			    	POW33=(IToken)Match(input,POW,FOLLOW_POW_in_powerExpression1440); 
            			    		POW33_tree = (object)adaptor.Create(POW33);
            			    		root_0 = (object)adaptor.BecomeRoot(POW33_tree, root_0);

            			    	PushFollow(FOLLOW_unaryExpression_in_powerExpression1443);
            			    	unaryExpression34 = unaryExpression();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, unaryExpression34.Tree);

            			    }
            			    break;

            			default:
            			    goto loop9;
            	    }
            	} while (true);

            	loop9:
            		;	// Stops C# compiler whining that label 'loop9' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "powerExpression"

    public class multiplicativeExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "multiplicativeExpression"
    // Elf.g:370:1: multiplicativeExpression : powerExpression ( ( MUL | DIV ) powerExpression )* ;
    public ElfParser.multiplicativeExpression_return multiplicativeExpression() // throws RecognitionException [1]
    {   
        ElfParser.multiplicativeExpression_return retval = new ElfParser.multiplicativeExpression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set36 = null;
        ElfParser.powerExpression_return powerExpression35 = default(ElfParser.powerExpression_return);

        ElfParser.powerExpression_return powerExpression37 = default(ElfParser.powerExpression_return);


        object set36_tree=null;

        try 
    	{
            // Elf.g:371:2: ( powerExpression ( ( MUL | DIV ) powerExpression )* )
            // Elf.g:371:4: powerExpression ( ( MUL | DIV ) powerExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_powerExpression_in_multiplicativeExpression1461);
            	powerExpression35 = powerExpression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, powerExpression35.Tree);
            	// Elf.g:371:20: ( ( MUL | DIV ) powerExpression )*
            	do 
            	{
            	    int alt10 = 2;
            	    int LA10_0 = input.LA(1);

            	    if ( ((LA10_0 >= MUL && LA10_0 <= DIV)) )
            	    {
            	        alt10 = 1;
            	    }


            	    switch (alt10) 
            		{
            			case 1 :
            			    // Elf.g:371:22: ( MUL | DIV ) powerExpression
            			    {
            			    	set36=(IToken)input.LT(1);
            			    	set36 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= MUL && input.LA(1) <= DIV) ) 
            			    	{
            			    	    input.Consume();
            			    	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set36), root_0);
            			    	    state.errorRecovery = false;
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_powerExpression_in_multiplicativeExpression1476);
            			    	powerExpression37 = powerExpression();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, powerExpression37.Tree);

            			    }
            			    break;

            			default:
            			    goto loop10;
            	    }
            	} while (true);

            	loop10:
            		;	// Stops C# compiler whining that label 'loop10' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "multiplicativeExpression"

    public class additiveExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "additiveExpression"
    // Elf.g:378:1: additiveExpression : multiplicativeExpression ( ( ADD | SUB ) multiplicativeExpression )* ;
    public ElfParser.additiveExpression_return additiveExpression() // throws RecognitionException [1]
    {   
        ElfParser.additiveExpression_return retval = new ElfParser.additiveExpression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set39 = null;
        ElfParser.multiplicativeExpression_return multiplicativeExpression38 = default(ElfParser.multiplicativeExpression_return);

        ElfParser.multiplicativeExpression_return multiplicativeExpression40 = default(ElfParser.multiplicativeExpression_return);


        object set39_tree=null;

        try 
    	{
            // Elf.g:379:2: ( multiplicativeExpression ( ( ADD | SUB ) multiplicativeExpression )* )
            // Elf.g:379:4: multiplicativeExpression ( ( ADD | SUB ) multiplicativeExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression1494);
            	multiplicativeExpression38 = multiplicativeExpression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, multiplicativeExpression38.Tree);
            	// Elf.g:379:29: ( ( ADD | SUB ) multiplicativeExpression )*
            	do 
            	{
            	    int alt11 = 2;
            	    int LA11_0 = input.LA(1);

            	    if ( ((LA11_0 >= ADD && LA11_0 <= SUB)) )
            	    {
            	        alt11 = 1;
            	    }


            	    switch (alt11) 
            		{
            			case 1 :
            			    // Elf.g:379:31: ( ADD | SUB ) multiplicativeExpression
            			    {
            			    	set39=(IToken)input.LT(1);
            			    	set39 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= ADD && input.LA(1) <= SUB) ) 
            			    	{
            			    	    input.Consume();
            			    	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set39), root_0);
            			    	    state.errorRecovery = false;
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression1509);
            			    	multiplicativeExpression40 = multiplicativeExpression();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, multiplicativeExpression40.Tree);

            			    }
            			    break;

            			default:
            			    goto loop11;
            	    }
            	} while (true);

            	loop11:
            		;	// Stops C# compiler whining that label 'loop11' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "additiveExpression"

    public class relationalExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "relationalExpression"
    // Elf.g:386:1: relationalExpression : additiveExpression ( ( LT | GT | LTE | GTE ) additiveExpression )* ;
    public ElfParser.relationalExpression_return relationalExpression() // throws RecognitionException [1]
    {   
        ElfParser.relationalExpression_return retval = new ElfParser.relationalExpression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set42 = null;
        ElfParser.additiveExpression_return additiveExpression41 = default(ElfParser.additiveExpression_return);

        ElfParser.additiveExpression_return additiveExpression43 = default(ElfParser.additiveExpression_return);


        object set42_tree=null;

        try 
    	{
            // Elf.g:387:2: ( additiveExpression ( ( LT | GT | LTE | GTE ) additiveExpression )* )
            // Elf.g:387:4: additiveExpression ( ( LT | GT | LTE | GTE ) additiveExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_additiveExpression_in_relationalExpression1528);
            	additiveExpression41 = additiveExpression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, additiveExpression41.Tree);
            	// Elf.g:387:23: ( ( LT | GT | LTE | GTE ) additiveExpression )*
            	do 
            	{
            	    int alt12 = 2;
            	    int LA12_0 = input.LA(1);

            	    if ( ((LA12_0 >= LT && LA12_0 <= GTE)) )
            	    {
            	        alt12 = 1;
            	    }


            	    switch (alt12) 
            		{
            			case 1 :
            			    // Elf.g:387:25: ( LT | GT | LTE | GTE ) additiveExpression
            			    {
            			    	set42=(IToken)input.LT(1);
            			    	set42 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= LT && input.LA(1) <= GTE) ) 
            			    	{
            			    	    input.Consume();
            			    	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set42), root_0);
            			    	    state.errorRecovery = false;
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_additiveExpression_in_relationalExpression1551);
            			    	additiveExpression43 = additiveExpression();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, additiveExpression43.Tree);

            			    }
            			    break;

            			default:
            			    goto loop12;
            	    }
            	} while (true);

            	loop12:
            		;	// Stops C# compiler whining that label 'loop12' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "relationalExpression"

    public class equalityExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "equalityExpression"
    // Elf.g:394:1: equalityExpression : relationalExpression ( ( EQ | NEQ ) relationalExpression )* ;
    public ElfParser.equalityExpression_return equalityExpression() // throws RecognitionException [1]
    {   
        ElfParser.equalityExpression_return retval = new ElfParser.equalityExpression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set45 = null;
        ElfParser.relationalExpression_return relationalExpression44 = default(ElfParser.relationalExpression_return);

        ElfParser.relationalExpression_return relationalExpression46 = default(ElfParser.relationalExpression_return);


        object set45_tree=null;

        try 
    	{
            // Elf.g:395:2: ( relationalExpression ( ( EQ | NEQ ) relationalExpression )* )
            // Elf.g:395:4: relationalExpression ( ( EQ | NEQ ) relationalExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_relationalExpression_in_equalityExpression1570);
            	relationalExpression44 = relationalExpression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, relationalExpression44.Tree);
            	// Elf.g:395:25: ( ( EQ | NEQ ) relationalExpression )*
            	do 
            	{
            	    int alt13 = 2;
            	    int LA13_0 = input.LA(1);

            	    if ( ((LA13_0 >= EQ && LA13_0 <= NEQ)) )
            	    {
            	        alt13 = 1;
            	    }


            	    switch (alt13) 
            		{
            			case 1 :
            			    // Elf.g:395:27: ( EQ | NEQ ) relationalExpression
            			    {
            			    	set45=(IToken)input.LT(1);
            			    	set45 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= EQ && input.LA(1) <= NEQ) ) 
            			    	{
            			    	    input.Consume();
            			    	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set45), root_0);
            			    	    state.errorRecovery = false;
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_relationalExpression_in_equalityExpression1585);
            			    	relationalExpression46 = relationalExpression();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, relationalExpression46.Tree);

            			    }
            			    break;

            			default:
            			    goto loop13;
            	    }
            	} while (true);

            	loop13:
            		;	// Stops C# compiler whining that label 'loop13' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "equalityExpression"

    public class logicalANDExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "logicalANDExpression"
    // Elf.g:402:1: logicalANDExpression : equalityExpression ( AND equalityExpression )* ;
    public ElfParser.logicalANDExpression_return logicalANDExpression() // throws RecognitionException [1]
    {   
        ElfParser.logicalANDExpression_return retval = new ElfParser.logicalANDExpression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken AND48 = null;
        ElfParser.equalityExpression_return equalityExpression47 = default(ElfParser.equalityExpression_return);

        ElfParser.equalityExpression_return equalityExpression49 = default(ElfParser.equalityExpression_return);


        object AND48_tree=null;

        try 
    	{
            // Elf.g:403:2: ( equalityExpression ( AND equalityExpression )* )
            // Elf.g:403:4: equalityExpression ( AND equalityExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_equalityExpression_in_logicalANDExpression1604);
            	equalityExpression47 = equalityExpression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, equalityExpression47.Tree);
            	// Elf.g:403:23: ( AND equalityExpression )*
            	do 
            	{
            	    int alt14 = 2;
            	    int LA14_0 = input.LA(1);

            	    if ( (LA14_0 == AND) )
            	    {
            	        alt14 = 1;
            	    }


            	    switch (alt14) 
            		{
            			case 1 :
            			    // Elf.g:403:25: AND equalityExpression
            			    {
            			    	AND48=(IToken)Match(input,AND,FOLLOW_AND_in_logicalANDExpression1608); 
            			    		AND48_tree = (object)adaptor.Create(AND48);
            			    		root_0 = (object)adaptor.BecomeRoot(AND48_tree, root_0);

            			    	PushFollow(FOLLOW_equalityExpression_in_logicalANDExpression1611);
            			    	equalityExpression49 = equalityExpression();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, equalityExpression49.Tree);

            			    }
            			    break;

            			default:
            			    goto loop14;
            	    }
            	} while (true);

            	loop14:
            		;	// Stops C# compiler whining that label 'loop14' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "logicalANDExpression"

    public class logicalORExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "logicalORExpression"
    // Elf.g:406:1: logicalORExpression : logicalANDExpression ( OR logicalANDExpression )* ;
    public ElfParser.logicalORExpression_return logicalORExpression() // throws RecognitionException [1]
    {   
        ElfParser.logicalORExpression_return retval = new ElfParser.logicalORExpression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken OR51 = null;
        ElfParser.logicalANDExpression_return logicalANDExpression50 = default(ElfParser.logicalANDExpression_return);

        ElfParser.logicalANDExpression_return logicalANDExpression52 = default(ElfParser.logicalANDExpression_return);


        object OR51_tree=null;

        try 
    	{
            // Elf.g:407:2: ( logicalANDExpression ( OR logicalANDExpression )* )
            // Elf.g:407:4: logicalANDExpression ( OR logicalANDExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_logicalANDExpression_in_logicalORExpression1626);
            	logicalANDExpression50 = logicalANDExpression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, logicalANDExpression50.Tree);
            	// Elf.g:407:25: ( OR logicalANDExpression )*
            	do 
            	{
            	    int alt15 = 2;
            	    int LA15_0 = input.LA(1);

            	    if ( (LA15_0 == OR) )
            	    {
            	        alt15 = 1;
            	    }


            	    switch (alt15) 
            		{
            			case 1 :
            			    // Elf.g:407:27: OR logicalANDExpression
            			    {
            			    	OR51=(IToken)Match(input,OR,FOLLOW_OR_in_logicalORExpression1630); 
            			    		OR51_tree = (object)adaptor.Create(OR51);
            			    		root_0 = (object)adaptor.BecomeRoot(OR51_tree, root_0);

            			    	PushFollow(FOLLOW_logicalANDExpression_in_logicalORExpression1633);
            			    	logicalANDExpression52 = logicalANDExpression();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, logicalANDExpression52.Tree);

            			    }
            			    break;

            			default:
            			    goto loop15;
            	    }
            	} while (true);

            	loop15:
            		;	// Stops C# compiler whining that label 'loop15' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "logicalORExpression"

    public class assignmentExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "assignmentExpression"
    // Elf.g:414:1: assignmentExpression : logicalORExpression ( ASSIGN assignmentExpression )? ;
    public ElfParser.assignmentExpression_return assignmentExpression() // throws RecognitionException [1]
    {   
        ElfParser.assignmentExpression_return retval = new ElfParser.assignmentExpression_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken ASSIGN54 = null;
        ElfParser.logicalORExpression_return logicalORExpression53 = default(ElfParser.logicalORExpression_return);

        ElfParser.assignmentExpression_return assignmentExpression55 = default(ElfParser.assignmentExpression_return);


        object ASSIGN54_tree=null;

        try 
    	{
            // Elf.g:415:2: ( logicalORExpression ( ASSIGN assignmentExpression )? )
            // Elf.g:415:4: logicalORExpression ( ASSIGN assignmentExpression )?
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_logicalORExpression_in_assignmentExpression1652);
            	logicalORExpression53 = logicalORExpression();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, logicalORExpression53.Tree);
            	// Elf.g:415:24: ( ASSIGN assignmentExpression )?
            	int alt16 = 2;
            	int LA16_0 = input.LA(1);

            	if ( (LA16_0 == ASSIGN) )
            	{
            	    alt16 = 1;
            	}
            	switch (alt16) 
            	{
            	    case 1 :
            	        // Elf.g:415:26: ASSIGN assignmentExpression
            	        {
            	        	ASSIGN54=(IToken)Match(input,ASSIGN,FOLLOW_ASSIGN_in_assignmentExpression1656); 
            	        		ASSIGN54_tree = (object)adaptor.Create(ASSIGN54);
            	        		root_0 = (object)adaptor.BecomeRoot(ASSIGN54_tree, root_0);

            	        	PushFollow(FOLLOW_assignmentExpression_in_assignmentExpression1659);
            	        	assignmentExpression55 = assignmentExpression();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, assignmentExpression55.Tree);

            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "assignmentExpression"

    public class block_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "block"
    // Elf.g:426:1: block : ( statement )* -> ^( BLOCK ( statement )* ) ;
    public ElfParser.block_return block() // throws RecognitionException [1]
    {   
        ElfParser.block_return retval = new ElfParser.block_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        ElfParser.statement_return statement56 = default(ElfParser.statement_return);


        RewriteRuleSubtreeStream stream_statement = new RewriteRuleSubtreeStream(adaptor,"rule statement");
        try 
    	{
            // Elf.g:427:2: ( ( statement )* -> ^( BLOCK ( statement )* ) )
            // Elf.g:427:4: ( statement )*
            {
            	// Elf.g:427:4: ( statement )*
            	do 
            	{
            	    int alt17 = 2;
            	    int LA17_0 = input.LA(1);

            	    if ( ((LA17_0 >= VAR && LA17_0 <= IF) || (LA17_0 >= ADD && LA17_0 <= SUB) || LA17_0 == NOT || LA17_0 == LPAREN || LA17_0 == SEMIC || LA17_0 == Identifier || (LA17_0 >= DecimalLiteral && LA17_0 <= StringLiteral)) )
            	    {
            	        alt17 = 1;
            	    }


            	    switch (alt17) 
            		{
            			case 1 :
            			    // Elf.g:427:4: statement
            			    {
            			    	PushFollow(FOLLOW_statement_in_block1684);
            			    	statement56 = statement();
            			    	state.followingStackPointer--;

            			    	stream_statement.Add(statement56.Tree);

            			    }
            			    break;

            			default:
            			    goto loop17;
            	    }
            	} while (true);

            	loop17:
            		;	// Stops C# compiler whining that label 'loop17' has no statements



            	// AST REWRITE
            	// elements:          statement
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));

            	root_0 = (object)adaptor.GetNilNode();
            	// 428:2: -> ^( BLOCK ( statement )* )
            	{
            	    // Elf.g:428:5: ^( BLOCK ( statement )* )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(BLOCK, "BLOCK"), root_1);

            	    // Elf.g:428:14: ( statement )*
            	    while ( stream_statement.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_statement.NextTree());

            	    }
            	    stream_statement.Reset();

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "block"

    public class statement_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "statement"
    // Elf.g:431:1: statement : ( variableStatement | emptyStatement | expressionStatement | ifStatement | returnStatement );
    public ElfParser.statement_return statement() // throws RecognitionException [1]
    {   
        ElfParser.statement_return retval = new ElfParser.statement_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        ElfParser.variableStatement_return variableStatement57 = default(ElfParser.variableStatement_return);

        ElfParser.emptyStatement_return emptyStatement58 = default(ElfParser.emptyStatement_return);

        ElfParser.expressionStatement_return expressionStatement59 = default(ElfParser.expressionStatement_return);

        ElfParser.ifStatement_return ifStatement60 = default(ElfParser.ifStatement_return);

        ElfParser.returnStatement_return returnStatement61 = default(ElfParser.returnStatement_return);



        try 
    	{
            // Elf.g:432:2: ( variableStatement | emptyStatement | expressionStatement | ifStatement | returnStatement )
            int alt18 = 5;
            switch ( input.LA(1) ) 
            {
            case VAR:
            	{
                alt18 = 1;
                }
                break;
            case SEMIC:
            	{
                alt18 = 2;
                }
                break;
            case ADD:
            case SUB:
            case NOT:
            case LPAREN:
            case Identifier:
            case DecimalLiteral:
            case StringLiteral:
            	{
                alt18 = 3;
                }
                break;
            case IF:
            	{
                alt18 = 4;
                }
                break;
            case RET:
            	{
                alt18 = 5;
                }
                break;
            	default:
            	    NoViableAltException nvae_d18s0 =
            	        new NoViableAltException("", 18, 0, input);

            	    throw nvae_d18s0;
            }

            switch (alt18) 
            {
                case 1 :
                    // Elf.g:432:4: variableStatement
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_variableStatement_in_statement1709);
                    	variableStatement57 = variableStatement();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, variableStatement57.Tree);

                    }
                    break;
                case 2 :
                    // Elf.g:433:4: emptyStatement
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_emptyStatement_in_statement1714);
                    	emptyStatement58 = emptyStatement();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, emptyStatement58.Tree);

                    }
                    break;
                case 3 :
                    // Elf.g:434:4: expressionStatement
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_expressionStatement_in_statement1719);
                    	expressionStatement59 = expressionStatement();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, expressionStatement59.Tree);

                    }
                    break;
                case 4 :
                    // Elf.g:435:4: ifStatement
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_ifStatement_in_statement1724);
                    	ifStatement60 = ifStatement();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, ifStatement60.Tree);

                    }
                    break;
                case 5 :
                    // Elf.g:436:4: returnStatement
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_returnStatement_in_statement1729);
                    	returnStatement61 = returnStatement();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, returnStatement61.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "statement"

    public class variableStatement_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "variableStatement"
    // Elf.g:441:1: variableStatement : VAR variableDeclaration ( COMMA variableDeclaration )* semic -> ^( VAR ( variableDeclaration )+ ) ;
    public ElfParser.variableStatement_return variableStatement() // throws RecognitionException [1]
    {   
        ElfParser.variableStatement_return retval = new ElfParser.variableStatement_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken VAR62 = null;
        IToken COMMA64 = null;
        ElfParser.variableDeclaration_return variableDeclaration63 = default(ElfParser.variableDeclaration_return);

        ElfParser.variableDeclaration_return variableDeclaration65 = default(ElfParser.variableDeclaration_return);

        ElfParser.semic_return semic66 = default(ElfParser.semic_return);


        object VAR62_tree=null;
        object COMMA64_tree=null;
        RewriteRuleTokenStream stream_VAR = new RewriteRuleTokenStream(adaptor,"token VAR");
        RewriteRuleTokenStream stream_COMMA = new RewriteRuleTokenStream(adaptor,"token COMMA");
        RewriteRuleSubtreeStream stream_variableDeclaration = new RewriteRuleSubtreeStream(adaptor,"rule variableDeclaration");
        RewriteRuleSubtreeStream stream_semic = new RewriteRuleSubtreeStream(adaptor,"rule semic");
        try 
    	{
            // Elf.g:442:2: ( VAR variableDeclaration ( COMMA variableDeclaration )* semic -> ^( VAR ( variableDeclaration )+ ) )
            // Elf.g:442:4: VAR variableDeclaration ( COMMA variableDeclaration )* semic
            {
            	VAR62=(IToken)Match(input,VAR,FOLLOW_VAR_in_variableStatement1742);  
            	stream_VAR.Add(VAR62);

            	PushFollow(FOLLOW_variableDeclaration_in_variableStatement1744);
            	variableDeclaration63 = variableDeclaration();
            	state.followingStackPointer--;

            	stream_variableDeclaration.Add(variableDeclaration63.Tree);
            	// Elf.g:442:28: ( COMMA variableDeclaration )*
            	do 
            	{
            	    int alt19 = 2;
            	    int LA19_0 = input.LA(1);

            	    if ( (LA19_0 == COMMA) )
            	    {
            	        alt19 = 1;
            	    }


            	    switch (alt19) 
            		{
            			case 1 :
            			    // Elf.g:442:30: COMMA variableDeclaration
            			    {
            			    	COMMA64=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_variableStatement1748);  
            			    	stream_COMMA.Add(COMMA64);

            			    	PushFollow(FOLLOW_variableDeclaration_in_variableStatement1750);
            			    	variableDeclaration65 = variableDeclaration();
            			    	state.followingStackPointer--;

            			    	stream_variableDeclaration.Add(variableDeclaration65.Tree);

            			    }
            			    break;

            			default:
            			    goto loop19;
            	    }
            	} while (true);

            	loop19:
            		;	// Stops C# compiler whining that label 'loop19' has no statements

            	PushFollow(FOLLOW_semic_in_variableStatement1755);
            	semic66 = semic();
            	state.followingStackPointer--;

            	stream_semic.Add(semic66.Tree);


            	// AST REWRITE
            	// elements:          VAR, variableDeclaration
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));

            	root_0 = (object)adaptor.GetNilNode();
            	// 443:2: -> ^( VAR ( variableDeclaration )+ )
            	{
            	    // Elf.g:443:5: ^( VAR ( variableDeclaration )+ )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot(stream_VAR.NextNode(), root_1);

            	    if ( !(stream_variableDeclaration.HasNext()) ) {
            	        throw new RewriteEarlyExitException();
            	    }
            	    while ( stream_variableDeclaration.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_variableDeclaration.NextTree());

            	    }
            	    stream_variableDeclaration.Reset();

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "variableStatement"

    public class variableDeclaration_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "variableDeclaration"
    // Elf.g:446:1: variableDeclaration : Identifier ( ASSIGN assignmentExpression )? ;
    public ElfParser.variableDeclaration_return variableDeclaration() // throws RecognitionException [1]
    {   
        ElfParser.variableDeclaration_return retval = new ElfParser.variableDeclaration_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken Identifier67 = null;
        IToken ASSIGN68 = null;
        ElfParser.assignmentExpression_return assignmentExpression69 = default(ElfParser.assignmentExpression_return);


        object Identifier67_tree=null;
        object ASSIGN68_tree=null;

        try 
    	{
            // Elf.g:447:2: ( Identifier ( ASSIGN assignmentExpression )? )
            // Elf.g:447:4: Identifier ( ASSIGN assignmentExpression )?
            {
            	root_0 = (object)adaptor.GetNilNode();

            	Identifier67=(IToken)Match(input,Identifier,FOLLOW_Identifier_in_variableDeclaration1778); 
            		Identifier67_tree = (object)adaptor.Create(Identifier67);
            		adaptor.AddChild(root_0, Identifier67_tree);

            	// Elf.g:447:15: ( ASSIGN assignmentExpression )?
            	int alt20 = 2;
            	int LA20_0 = input.LA(1);

            	if ( (LA20_0 == ASSIGN) )
            	{
            	    alt20 = 1;
            	}
            	switch (alt20) 
            	{
            	    case 1 :
            	        // Elf.g:447:17: ASSIGN assignmentExpression
            	        {
            	        	ASSIGN68=(IToken)Match(input,ASSIGN,FOLLOW_ASSIGN_in_variableDeclaration1782); 
            	        		ASSIGN68_tree = (object)adaptor.Create(ASSIGN68);
            	        		root_0 = (object)adaptor.BecomeRoot(ASSIGN68_tree, root_0);

            	        	PushFollow(FOLLOW_assignmentExpression_in_variableDeclaration1785);
            	        	assignmentExpression69 = assignmentExpression();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, assignmentExpression69.Tree);

            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "variableDeclaration"

    public class emptyStatement_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "emptyStatement"
    // Elf.g:454:1: emptyStatement : SEMIC ;
    public ElfParser.emptyStatement_return emptyStatement() // throws RecognitionException [1]
    {   
        ElfParser.emptyStatement_return retval = new ElfParser.emptyStatement_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken SEMIC70 = null;

        object SEMIC70_tree=null;

        try 
    	{
            // Elf.g:455:2: ( SEMIC )
            // Elf.g:455:4: SEMIC
            {
            	root_0 = (object)adaptor.GetNilNode();

            	SEMIC70=(IToken)Match(input,SEMIC,FOLLOW_SEMIC_in_emptyStatement1803); 

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "emptyStatement"

    public class expressionStatement_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "expressionStatement"
    // Elf.g:462:1: expressionStatement : expression semic -> ^( EXPR expression ) ;
    public ElfParser.expressionStatement_return expressionStatement() // throws RecognitionException [1]
    {   
        ElfParser.expressionStatement_return retval = new ElfParser.expressionStatement_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        ElfParser.expression_return expression71 = default(ElfParser.expression_return);

        ElfParser.semic_return semic72 = default(ElfParser.semic_return);


        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        RewriteRuleSubtreeStream stream_semic = new RewriteRuleSubtreeStream(adaptor,"rule semic");
        try 
    	{
            // Elf.g:463:2: ( expression semic -> ^( EXPR expression ) )
            // Elf.g:463:4: expression semic
            {
            	PushFollow(FOLLOW_expression_in_expressionStatement1819);
            	expression71 = expression();
            	state.followingStackPointer--;

            	stream_expression.Add(expression71.Tree);
            	PushFollow(FOLLOW_semic_in_expressionStatement1821);
            	semic72 = semic();
            	state.followingStackPointer--;

            	stream_semic.Add(semic72.Tree);


            	// AST REWRITE
            	// elements:          expression
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));

            	root_0 = (object)adaptor.GetNilNode();
            	// 464:2: -> ^( EXPR expression )
            	{
            	    // Elf.g:464:5: ^( EXPR expression )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(EXPR, "EXPR"), root_1);

            	    adaptor.AddChild(root_1, stream_expression.NextTree());

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "expressionStatement"

    public class ifStatement_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "ifStatement"
    // Elf.g:471:1: ifStatement : IF expression THEN block ( ELSE block )? END -> ^( IF expression ( block )+ ) ;
    public ElfParser.ifStatement_return ifStatement() // throws RecognitionException [1]
    {   
        ElfParser.ifStatement_return retval = new ElfParser.ifStatement_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken IF73 = null;
        IToken THEN75 = null;
        IToken ELSE77 = null;
        IToken END79 = null;
        ElfParser.expression_return expression74 = default(ElfParser.expression_return);

        ElfParser.block_return block76 = default(ElfParser.block_return);

        ElfParser.block_return block78 = default(ElfParser.block_return);


        object IF73_tree=null;
        object THEN75_tree=null;
        object ELSE77_tree=null;
        object END79_tree=null;
        RewriteRuleTokenStream stream_THEN = new RewriteRuleTokenStream(adaptor,"token THEN");
        RewriteRuleTokenStream stream_END = new RewriteRuleTokenStream(adaptor,"token END");
        RewriteRuleTokenStream stream_IF = new RewriteRuleTokenStream(adaptor,"token IF");
        RewriteRuleTokenStream stream_ELSE = new RewriteRuleTokenStream(adaptor,"token ELSE");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        RewriteRuleSubtreeStream stream_block = new RewriteRuleSubtreeStream(adaptor,"rule block");
        try 
    	{
            // Elf.g:472:2: ( IF expression THEN block ( ELSE block )? END -> ^( IF expression ( block )+ ) )
            // Elf.g:472:4: IF expression THEN block ( ELSE block )? END
            {
            	IF73=(IToken)Match(input,IF,FOLLOW_IF_in_ifStatement1847);  
            	stream_IF.Add(IF73);

            	PushFollow(FOLLOW_expression_in_ifStatement1849);
            	expression74 = expression();
            	state.followingStackPointer--;

            	stream_expression.Add(expression74.Tree);
            	THEN75=(IToken)Match(input,THEN,FOLLOW_THEN_in_ifStatement1851);  
            	stream_THEN.Add(THEN75);

            	PushFollow(FOLLOW_block_in_ifStatement1853);
            	block76 = block();
            	state.followingStackPointer--;

            	stream_block.Add(block76.Tree);
            	// Elf.g:472:29: ( ELSE block )?
            	int alt21 = 2;
            	int LA21_0 = input.LA(1);

            	if ( (LA21_0 == ELSE) )
            	{
            	    alt21 = 1;
            	}
            	switch (alt21) 
            	{
            	    case 1 :
            	        // Elf.g:472:31: ELSE block
            	        {
            	        	ELSE77=(IToken)Match(input,ELSE,FOLLOW_ELSE_in_ifStatement1857);  
            	        	stream_ELSE.Add(ELSE77);

            	        	PushFollow(FOLLOW_block_in_ifStatement1859);
            	        	block78 = block();
            	        	state.followingStackPointer--;

            	        	stream_block.Add(block78.Tree);

            	        }
            	        break;

            	}

            	END79=(IToken)Match(input,END,FOLLOW_END_in_ifStatement1864);  
            	stream_END.Add(END79);



            	// AST REWRITE
            	// elements:          block, expression, IF
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));

            	root_0 = (object)adaptor.GetNilNode();
            	// 473:2: -> ^( IF expression ( block )+ )
            	{
            	    // Elf.g:473:5: ^( IF expression ( block )+ )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot(stream_IF.NextNode(), root_1);

            	    adaptor.AddChild(root_1, stream_expression.NextTree());
            	    if ( !(stream_block.HasNext()) ) {
            	        throw new RewriteEarlyExitException();
            	    }
            	    while ( stream_block.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_block.NextTree());

            	    }
            	    stream_block.Reset();

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "ifStatement"

    public class returnStatement_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "returnStatement"
    // Elf.g:480:1: returnStatement : RET ( expression )? semic ;
    public ElfParser.returnStatement_return returnStatement() // throws RecognitionException [1]
    {   
        ElfParser.returnStatement_return retval = new ElfParser.returnStatement_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken RET80 = null;
        ElfParser.expression_return expression81 = default(ElfParser.expression_return);

        ElfParser.semic_return semic82 = default(ElfParser.semic_return);


        object RET80_tree=null;

        try 
    	{
            // Elf.g:481:2: ( RET ( expression )? semic )
            // Elf.g:481:4: RET ( expression )? semic
            {
            	root_0 = (object)adaptor.GetNilNode();

            	RET80=(IToken)Match(input,RET,FOLLOW_RET_in_returnStatement1894); 
            		RET80_tree = (object)adaptor.Create(RET80);
            		root_0 = (object)adaptor.BecomeRoot(RET80_tree, root_0);

            	// Elf.g:481:9: ( expression )?
            	int alt22 = 2;
            	int LA22_0 = input.LA(1);

            	if ( ((LA22_0 >= ADD && LA22_0 <= SUB) || LA22_0 == NOT || LA22_0 == LPAREN || LA22_0 == Identifier || (LA22_0 >= DecimalLiteral && LA22_0 <= StringLiteral)) )
            	{
            	    alt22 = 1;
            	}
            	switch (alt22) 
            	{
            	    case 1 :
            	        // Elf.g:481:9: expression
            	        {
            	        	PushFollow(FOLLOW_expression_in_returnStatement1897);
            	        	expression81 = expression();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, expression81.Tree);

            	        }
            	        break;

            	}

            	PushFollow(FOLLOW_semic_in_returnStatement1900);
            	semic82 = semic();
            	state.followingStackPointer--;


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "returnStatement"

    public class functionDefinition_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "functionDefinition"
    // Elf.g:492:1: functionDefinition : elfFunctionDefinition ;
    public ElfParser.functionDefinition_return functionDefinition() // throws RecognitionException [1]
    {   
        ElfParser.functionDefinition_return retval = new ElfParser.functionDefinition_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        ElfParser.elfFunctionDefinition_return elfFunctionDefinition83 = default(ElfParser.elfFunctionDefinition_return);



        try 
    	{
            // Elf.g:493:2: ( elfFunctionDefinition )
            // Elf.g:493:4: elfFunctionDefinition
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_elfFunctionDefinition_in_functionDefinition1922);
            	elfFunctionDefinition83 = elfFunctionDefinition();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, elfFunctionDefinition83.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "functionDefinition"

    public class elfFunctionDefinition_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "elfFunctionDefinition"
    // Elf.g:496:1: elfFunctionDefinition : DEF name= Identifier formalParameterList block END -> ^( FUNC ^( DECL $name formalParameterList ) block ) ;
    public ElfParser.elfFunctionDefinition_return elfFunctionDefinition() // throws RecognitionException [1]
    {   
        ElfParser.elfFunctionDefinition_return retval = new ElfParser.elfFunctionDefinition_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken name = null;
        IToken DEF84 = null;
        IToken END87 = null;
        ElfParser.formalParameterList_return formalParameterList85 = default(ElfParser.formalParameterList_return);

        ElfParser.block_return block86 = default(ElfParser.block_return);


        object name_tree=null;
        object DEF84_tree=null;
        object END87_tree=null;
        RewriteRuleTokenStream stream_DEF = new RewriteRuleTokenStream(adaptor,"token DEF");
        RewriteRuleTokenStream stream_END = new RewriteRuleTokenStream(adaptor,"token END");
        RewriteRuleTokenStream stream_Identifier = new RewriteRuleTokenStream(adaptor,"token Identifier");
        RewriteRuleSubtreeStream stream_block = new RewriteRuleSubtreeStream(adaptor,"rule block");
        RewriteRuleSubtreeStream stream_formalParameterList = new RewriteRuleSubtreeStream(adaptor,"rule formalParameterList");
        try 
    	{
            // Elf.g:497:2: ( DEF name= Identifier formalParameterList block END -> ^( FUNC ^( DECL $name formalParameterList ) block ) )
            // Elf.g:497:4: DEF name= Identifier formalParameterList block END
            {
            	DEF84=(IToken)Match(input,DEF,FOLLOW_DEF_in_elfFunctionDefinition1934);  
            	stream_DEF.Add(DEF84);

            	name=(IToken)Match(input,Identifier,FOLLOW_Identifier_in_elfFunctionDefinition1938);  
            	stream_Identifier.Add(name);

            	PushFollow(FOLLOW_formalParameterList_in_elfFunctionDefinition1940);
            	formalParameterList85 = formalParameterList();
            	state.followingStackPointer--;

            	stream_formalParameterList.Add(formalParameterList85.Tree);
            	PushFollow(FOLLOW_block_in_elfFunctionDefinition1942);
            	block86 = block();
            	state.followingStackPointer--;

            	stream_block.Add(block86.Tree);
            	END87=(IToken)Match(input,END,FOLLOW_END_in_elfFunctionDefinition1944);  
            	stream_END.Add(END87);



            	// AST REWRITE
            	// elements:          formalParameterList, block, name
            	// token labels:      name
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.Tree = root_0;
            	RewriteRuleTokenStream stream_name = new RewriteRuleTokenStream(adaptor, "token name", name);
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));

            	root_0 = (object)adaptor.GetNilNode();
            	// 498:2: -> ^( FUNC ^( DECL $name formalParameterList ) block )
            	{
            	    // Elf.g:498:5: ^( FUNC ^( DECL $name formalParameterList ) block )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(FUNC, "FUNC"), root_1);

            	    // Elf.g:498:13: ^( DECL $name formalParameterList )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(DECL, "DECL"), root_2);

            	    adaptor.AddChild(root_2, stream_name.NextNode());
            	    adaptor.AddChild(root_2, stream_formalParameterList.NextTree());

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    adaptor.AddChild(root_1, stream_block.NextTree());

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "elfFunctionDefinition"

    public class formalParameterList_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "formalParameterList"
    // Elf.g:501:1: formalParameterList : LPAREN (args+= Identifier ( COMMA args+= Identifier )* )? RPAREN -> ^( ARGS ( $args)* ) ;
    public ElfParser.formalParameterList_return formalParameterList() // throws RecognitionException [1]
    {   
        ElfParser.formalParameterList_return retval = new ElfParser.formalParameterList_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken LPAREN88 = null;
        IToken COMMA89 = null;
        IToken RPAREN90 = null;
        IToken args = null;
        IList list_args = null;

        object LPAREN88_tree=null;
        object COMMA89_tree=null;
        object RPAREN90_tree=null;
        object args_tree=null;
        RewriteRuleTokenStream stream_RPAREN = new RewriteRuleTokenStream(adaptor,"token RPAREN");
        RewriteRuleTokenStream stream_COMMA = new RewriteRuleTokenStream(adaptor,"token COMMA");
        RewriteRuleTokenStream stream_Identifier = new RewriteRuleTokenStream(adaptor,"token Identifier");
        RewriteRuleTokenStream stream_LPAREN = new RewriteRuleTokenStream(adaptor,"token LPAREN");

        try 
    	{
            // Elf.g:502:2: ( LPAREN (args+= Identifier ( COMMA args+= Identifier )* )? RPAREN -> ^( ARGS ( $args)* ) )
            // Elf.g:502:4: LPAREN (args+= Identifier ( COMMA args+= Identifier )* )? RPAREN
            {
            	LPAREN88=(IToken)Match(input,LPAREN,FOLLOW_LPAREN_in_formalParameterList1978);  
            	stream_LPAREN.Add(LPAREN88);

            	// Elf.g:502:11: (args+= Identifier ( COMMA args+= Identifier )* )?
            	int alt24 = 2;
            	int LA24_0 = input.LA(1);

            	if ( (LA24_0 == Identifier) )
            	{
            	    alt24 = 1;
            	}
            	switch (alt24) 
            	{
            	    case 1 :
            	        // Elf.g:502:13: args+= Identifier ( COMMA args+= Identifier )*
            	        {
            	        	args=(IToken)Match(input,Identifier,FOLLOW_Identifier_in_formalParameterList1984);  
            	        	stream_Identifier.Add(args);

            	        	if (list_args == null) list_args = new ArrayList();
            	        	list_args.Add(args);

            	        	// Elf.g:502:30: ( COMMA args+= Identifier )*
            	        	do 
            	        	{
            	        	    int alt23 = 2;
            	        	    int LA23_0 = input.LA(1);

            	        	    if ( (LA23_0 == COMMA) )
            	        	    {
            	        	        alt23 = 1;
            	        	    }


            	        	    switch (alt23) 
            	        		{
            	        			case 1 :
            	        			    // Elf.g:502:32: COMMA args+= Identifier
            	        			    {
            	        			    	COMMA89=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_formalParameterList1988);  
            	        			    	stream_COMMA.Add(COMMA89);

            	        			    	args=(IToken)Match(input,Identifier,FOLLOW_Identifier_in_formalParameterList1992);  
            	        			    	stream_Identifier.Add(args);

            	        			    	if (list_args == null) list_args = new ArrayList();
            	        			    	list_args.Add(args);


            	        			    }
            	        			    break;

            	        			default:
            	        			    goto loop23;
            	        	    }
            	        	} while (true);

            	        	loop23:
            	        		;	// Stops C# compiler whining that label 'loop23' has no statements


            	        }
            	        break;

            	}

            	RPAREN90=(IToken)Match(input,RPAREN,FOLLOW_RPAREN_in_formalParameterList2000);  
            	stream_RPAREN.Add(RPAREN90);



            	// AST REWRITE
            	// elements:          args
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: args
            	// rule list labels:  
            	retval.Tree = root_0;
            	RewriteRuleTokenStream stream_args = new RewriteRuleTokenStream(adaptor,"token args", list_args);
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));

            	root_0 = (object)adaptor.GetNilNode();
            	// 503:2: -> ^( ARGS ( $args)* )
            	{
            	    // Elf.g:503:5: ^( ARGS ( $args)* )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ARGS, "ARGS"), root_1);

            	    // Elf.g:503:13: ( $args)*
            	    while ( stream_args.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_args.NextNode());

            	    }
            	    stream_args.Reset();

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "formalParameterList"

    public class classDefinition_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "classDefinition"
    // Elf.g:512:1: classDefinition : DEF name= Identifier RTIMPL clrClassRef= Identifier ( functionDefinition )* END -> ^( CLASS ^( DECL $name ^( RTIMPL $clrClassRef) ) ( functionDefinition )* ) ;
    public ElfParser.classDefinition_return classDefinition() // throws RecognitionException [1]
    {   
        ElfParser.classDefinition_return retval = new ElfParser.classDefinition_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken name = null;
        IToken clrClassRef = null;
        IToken DEF91 = null;
        IToken RTIMPL92 = null;
        IToken END94 = null;
        ElfParser.functionDefinition_return functionDefinition93 = default(ElfParser.functionDefinition_return);


        object name_tree=null;
        object clrClassRef_tree=null;
        object DEF91_tree=null;
        object RTIMPL92_tree=null;
        object END94_tree=null;
        RewriteRuleTokenStream stream_DEF = new RewriteRuleTokenStream(adaptor,"token DEF");
        RewriteRuleTokenStream stream_RTIMPL = new RewriteRuleTokenStream(adaptor,"token RTIMPL");
        RewriteRuleTokenStream stream_END = new RewriteRuleTokenStream(adaptor,"token END");
        RewriteRuleTokenStream stream_Identifier = new RewriteRuleTokenStream(adaptor,"token Identifier");
        RewriteRuleSubtreeStream stream_functionDefinition = new RewriteRuleSubtreeStream(adaptor,"rule functionDefinition");
        try 
    	{
            // Elf.g:513:2: ( DEF name= Identifier RTIMPL clrClassRef= Identifier ( functionDefinition )* END -> ^( CLASS ^( DECL $name ^( RTIMPL $clrClassRef) ) ( functionDefinition )* ) )
            // Elf.g:513:4: DEF name= Identifier RTIMPL clrClassRef= Identifier ( functionDefinition )* END
            {
            	DEF91=(IToken)Match(input,DEF,FOLLOW_DEF_in_classDefinition2032);  
            	stream_DEF.Add(DEF91);

            	name=(IToken)Match(input,Identifier,FOLLOW_Identifier_in_classDefinition2036);  
            	stream_Identifier.Add(name);

            	RTIMPL92=(IToken)Match(input,RTIMPL,FOLLOW_RTIMPL_in_classDefinition2038);  
            	stream_RTIMPL.Add(RTIMPL92);

            	clrClassRef=(IToken)Match(input,Identifier,FOLLOW_Identifier_in_classDefinition2042);  
            	stream_Identifier.Add(clrClassRef);

            	// Elf.g:513:54: ( functionDefinition )*
            	do 
            	{
            	    int alt25 = 2;
            	    int LA25_0 = input.LA(1);

            	    if ( (LA25_0 == DEF) )
            	    {
            	        alt25 = 1;
            	    }


            	    switch (alt25) 
            		{
            			case 1 :
            			    // Elf.g:513:54: functionDefinition
            			    {
            			    	PushFollow(FOLLOW_functionDefinition_in_classDefinition2044);
            			    	functionDefinition93 = functionDefinition();
            			    	state.followingStackPointer--;

            			    	stream_functionDefinition.Add(functionDefinition93.Tree);

            			    }
            			    break;

            			default:
            			    goto loop25;
            	    }
            	} while (true);

            	loop25:
            		;	// Stops C# compiler whining that label 'loop25' has no statements

            	END94=(IToken)Match(input,END,FOLLOW_END_in_classDefinition2047);  
            	stream_END.Add(END94);



            	// AST REWRITE
            	// elements:          functionDefinition, clrClassRef, RTIMPL, name
            	// token labels:      name, clrClassRef
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.Tree = root_0;
            	RewriteRuleTokenStream stream_name = new RewriteRuleTokenStream(adaptor, "token name", name);
            	RewriteRuleTokenStream stream_clrClassRef = new RewriteRuleTokenStream(adaptor, "token clrClassRef", clrClassRef);
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));

            	root_0 = (object)adaptor.GetNilNode();
            	// 514:2: -> ^( CLASS ^( DECL $name ^( RTIMPL $clrClassRef) ) ( functionDefinition )* )
            	{
            	    // Elf.g:514:5: ^( CLASS ^( DECL $name ^( RTIMPL $clrClassRef) ) ( functionDefinition )* )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(CLASS, "CLASS"), root_1);

            	    // Elf.g:514:14: ^( DECL $name ^( RTIMPL $clrClassRef) )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(DECL, "DECL"), root_2);

            	    adaptor.AddChild(root_2, stream_name.NextNode());
            	    // Elf.g:514:28: ^( RTIMPL $clrClassRef)
            	    {
            	    object root_3 = (object)adaptor.GetNilNode();
            	    root_3 = (object)adaptor.BecomeRoot(stream_RTIMPL.NextNode(), root_3);

            	    adaptor.AddChild(root_3, stream_clrClassRef.NextNode());

            	    adaptor.AddChild(root_2, root_3);
            	    }

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // Elf.g:514:55: ( functionDefinition )*
            	    while ( stream_functionDefinition.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_functionDefinition.NextTree());

            	    }
            	    stream_functionDefinition.Reset();

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "classDefinition"

    public class script_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "script"
    // Elf.g:523:1: script : ( classDefinition )* -> ^( SCRIPT ( classDefinition )* ) ;
    public ElfParser.script_return script() // throws RecognitionException [1]
    {   
        ElfParser.script_return retval = new ElfParser.script_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        ElfParser.classDefinition_return classDefinition95 = default(ElfParser.classDefinition_return);


        RewriteRuleSubtreeStream stream_classDefinition = new RewriteRuleSubtreeStream(adaptor,"rule classDefinition");
        try 
    	{
            // Elf.g:524:2: ( ( classDefinition )* -> ^( SCRIPT ( classDefinition )* ) )
            // Elf.g:524:4: ( classDefinition )*
            {
            	// Elf.g:524:4: ( classDefinition )*
            	do 
            	{
            	    int alt26 = 2;
            	    int LA26_0 = input.LA(1);

            	    if ( (LA26_0 == DEF) )
            	    {
            	        alt26 = 1;
            	    }


            	    switch (alt26) 
            		{
            			case 1 :
            			    // Elf.g:524:4: classDefinition
            			    {
            			    	PushFollow(FOLLOW_classDefinition_in_script2097);
            			    	classDefinition95 = classDefinition();
            			    	state.followingStackPointer--;

            			    	stream_classDefinition.Add(classDefinition95.Tree);

            			    }
            			    break;

            			default:
            			    goto loop26;
            	    }
            	} while (true);

            	loop26:
            		;	// Stops C# compiler whining that label 'loop26' has no statements



            	// AST REWRITE
            	// elements:          classDefinition
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));

            	root_0 = (object)adaptor.GetNilNode();
            	// 525:2: -> ^( SCRIPT ( classDefinition )* )
            	{
            	    // Elf.g:525:5: ^( SCRIPT ( classDefinition )* )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(SCRIPT, "SCRIPT"), root_1);

            	    // Elf.g:525:15: ( classDefinition )*
            	    while ( stream_classDefinition.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_classDefinition.NextTree());

            	    }
            	    stream_classDefinition.Reset();

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "script"

    // Delegated rules


	private void InitializeCyclicDFAs()
	{
	}

 

    public static readonly BitSet FOLLOW_keyword_in_token455 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_literal_in_token460 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Identifier_in_token465 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_punctuator_in_token470 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_oneOfOperators_in_token475 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_punctuator0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_semic0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_keyword0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_nonAssignOperator_in_oneOfOperators746 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_oneOfOperators751 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_nonAssignOperator0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_literal0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_assignmentExpression_in_expression1206 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Identifier_in_primaryExpression1219 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_literal_in_primaryExpression1224 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LPAREN_in_primaryExpression1231 = new BitSet(new ulong[]{0x0018408002030000UL});
    public static readonly BitSet FOLLOW_expression_in_primaryExpression1233 = new BitSet(new ulong[]{0x0000010000000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_primaryExpression1235 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_primaryExpression_in_lhsExpression1267 = new BitSet(new ulong[]{0x0000028000000002UL});
    public static readonly BitSet FOLLOW_arguments_in_lhsExpression1283 = new BitSet(new ulong[]{0x0000028000000002UL});
    public static readonly BitSet FOLLOW_LBRACK_in_lhsExpression1304 = new BitSet(new ulong[]{0x0018408002030000UL});
    public static readonly BitSet FOLLOW_expression_in_lhsExpression1306 = new BitSet(new ulong[]{0x0000040000000000UL});
    public static readonly BitSet FOLLOW_RBRACK_in_lhsExpression1308 = new BitSet(new ulong[]{0x0000028000000002UL});
    public static readonly BitSet FOLLOW_LPAREN_in_arguments1337 = new BitSet(new ulong[]{0x0018418002030000UL});
    public static readonly BitSet FOLLOW_assignmentExpression_in_arguments1341 = new BitSet(new ulong[]{0x0000090000000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_arguments1345 = new BitSet(new ulong[]{0x0018408002030000UL});
    public static readonly BitSet FOLLOW_assignmentExpression_in_arguments1347 = new BitSet(new ulong[]{0x0000090000000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_arguments1355 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_lhsExpression_in_unaryExpression1383 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_unaryOperator_in_unaryExpression1388 = new BitSet(new ulong[]{0x0018408002030000UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_unaryExpression1391 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ADD_in_unaryOperator1405 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_SUB_in_unaryOperator1414 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_NOT_in_unaryOperator1421 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_powerExpression1436 = new BitSet(new ulong[]{0x0000000000040002UL});
    public static readonly BitSet FOLLOW_POW_in_powerExpression1440 = new BitSet(new ulong[]{0x0018408002030000UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_powerExpression1443 = new BitSet(new ulong[]{0x0000000000040002UL});
    public static readonly BitSet FOLLOW_powerExpression_in_multiplicativeExpression1461 = new BitSet(new ulong[]{0x000000000000C002UL});
    public static readonly BitSet FOLLOW_set_in_multiplicativeExpression1465 = new BitSet(new ulong[]{0x0018408002030000UL});
    public static readonly BitSet FOLLOW_powerExpression_in_multiplicativeExpression1476 = new BitSet(new ulong[]{0x000000000000C002UL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_additiveExpression1494 = new BitSet(new ulong[]{0x0000000000030002UL});
    public static readonly BitSet FOLLOW_set_in_additiveExpression1498 = new BitSet(new ulong[]{0x0018408002030000UL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_additiveExpression1509 = new BitSet(new ulong[]{0x0000000000030002UL});
    public static readonly BitSet FOLLOW_additiveExpression_in_relationalExpression1528 = new BitSet(new ulong[]{0x0000000000780002UL});
    public static readonly BitSet FOLLOW_set_in_relationalExpression1532 = new BitSet(new ulong[]{0x0018408002030000UL});
    public static readonly BitSet FOLLOW_additiveExpression_in_relationalExpression1551 = new BitSet(new ulong[]{0x0000000000780002UL});
    public static readonly BitSet FOLLOW_relationalExpression_in_equalityExpression1570 = new BitSet(new ulong[]{0x0000000001800002UL});
    public static readonly BitSet FOLLOW_set_in_equalityExpression1574 = new BitSet(new ulong[]{0x0018408002030000UL});
    public static readonly BitSet FOLLOW_relationalExpression_in_equalityExpression1585 = new BitSet(new ulong[]{0x0000000001800002UL});
    public static readonly BitSet FOLLOW_equalityExpression_in_logicalANDExpression1604 = new BitSet(new ulong[]{0x0000000004000002UL});
    public static readonly BitSet FOLLOW_AND_in_logicalANDExpression1608 = new BitSet(new ulong[]{0x0018408002030000UL});
    public static readonly BitSet FOLLOW_equalityExpression_in_logicalANDExpression1611 = new BitSet(new ulong[]{0x0000000004000002UL});
    public static readonly BitSet FOLLOW_logicalANDExpression_in_logicalORExpression1626 = new BitSet(new ulong[]{0x0000000008000002UL});
    public static readonly BitSet FOLLOW_OR_in_logicalORExpression1630 = new BitSet(new ulong[]{0x0018408002030000UL});
    public static readonly BitSet FOLLOW_logicalANDExpression_in_logicalORExpression1633 = new BitSet(new ulong[]{0x0000000008000002UL});
    public static readonly BitSet FOLLOW_logicalORExpression_in_assignmentExpression1652 = new BitSet(new ulong[]{0x0000000010000002UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_assignmentExpression1656 = new BitSet(new ulong[]{0x0018408002030000UL});
    public static readonly BitSet FOLLOW_assignmentExpression_in_assignmentExpression1659 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_statement_in_block1684 = new BitSet(new ulong[]{0x00185080020301C2UL});
    public static readonly BitSet FOLLOW_variableStatement_in_statement1709 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_emptyStatement_in_statement1714 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expressionStatement_in_statement1719 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ifStatement_in_statement1724 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_returnStatement_in_statement1729 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_VAR_in_variableStatement1742 = new BitSet(new ulong[]{0x0000400000000000UL});
    public static readonly BitSet FOLLOW_variableDeclaration_in_variableStatement1744 = new BitSet(new ulong[]{0x0003180000000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_variableStatement1748 = new BitSet(new ulong[]{0x0000400000000000UL});
    public static readonly BitSet FOLLOW_variableDeclaration_in_variableStatement1750 = new BitSet(new ulong[]{0x0003180000000000UL});
    public static readonly BitSet FOLLOW_semic_in_variableStatement1755 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Identifier_in_variableDeclaration1778 = new BitSet(new ulong[]{0x0000000010000002UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_variableDeclaration1782 = new BitSet(new ulong[]{0x0018408002030000UL});
    public static readonly BitSet FOLLOW_assignmentExpression_in_variableDeclaration1785 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_SEMIC_in_emptyStatement1803 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expression_in_expressionStatement1819 = new BitSet(new ulong[]{0x0003180000000000UL});
    public static readonly BitSet FOLLOW_semic_in_expressionStatement1821 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IF_in_ifStatement1847 = new BitSet(new ulong[]{0x0018408002030000UL});
    public static readonly BitSet FOLLOW_expression_in_ifStatement1849 = new BitSet(new ulong[]{0x0000000000000200UL});
    public static readonly BitSet FOLLOW_THEN_in_ifStatement1851 = new BitSet(new ulong[]{0x00185080020301C0UL});
    public static readonly BitSet FOLLOW_block_in_ifStatement1853 = new BitSet(new ulong[]{0x0000000000000C00UL});
    public static readonly BitSet FOLLOW_ELSE_in_ifStatement1857 = new BitSet(new ulong[]{0x00185080020301C0UL});
    public static readonly BitSet FOLLOW_block_in_ifStatement1859 = new BitSet(new ulong[]{0x0000000000000800UL});
    public static readonly BitSet FOLLOW_END_in_ifStatement1864 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_RET_in_returnStatement1894 = new BitSet(new ulong[]{0x001B588002030000UL});
    public static readonly BitSet FOLLOW_expression_in_returnStatement1897 = new BitSet(new ulong[]{0x0003180000000000UL});
    public static readonly BitSet FOLLOW_semic_in_returnStatement1900 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_elfFunctionDefinition_in_functionDefinition1922 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DEF_in_elfFunctionDefinition1934 = new BitSet(new ulong[]{0x0000400000000000UL});
    public static readonly BitSet FOLLOW_Identifier_in_elfFunctionDefinition1938 = new BitSet(new ulong[]{0x0000008000000000UL});
    public static readonly BitSet FOLLOW_formalParameterList_in_elfFunctionDefinition1940 = new BitSet(new ulong[]{0x00185080020301C0UL});
    public static readonly BitSet FOLLOW_block_in_elfFunctionDefinition1942 = new BitSet(new ulong[]{0x0000000000000800UL});
    public static readonly BitSet FOLLOW_END_in_elfFunctionDefinition1944 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LPAREN_in_formalParameterList1978 = new BitSet(new ulong[]{0x0000410000000000UL});
    public static readonly BitSet FOLLOW_Identifier_in_formalParameterList1984 = new BitSet(new ulong[]{0x0000090000000000UL});
    public static readonly BitSet FOLLOW_COMMA_in_formalParameterList1988 = new BitSet(new ulong[]{0x0000400000000000UL});
    public static readonly BitSet FOLLOW_Identifier_in_formalParameterList1992 = new BitSet(new ulong[]{0x0000090000000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_formalParameterList2000 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DEF_in_classDefinition2032 = new BitSet(new ulong[]{0x0000400000000000UL});
    public static readonly BitSet FOLLOW_Identifier_in_classDefinition2036 = new BitSet(new ulong[]{0x0000000000000020UL});
    public static readonly BitSet FOLLOW_RTIMPL_in_classDefinition2038 = new BitSet(new ulong[]{0x0000400000000000UL});
    public static readonly BitSet FOLLOW_Identifier_in_classDefinition2042 = new BitSet(new ulong[]{0x0000000000000810UL});
    public static readonly BitSet FOLLOW_functionDefinition_in_classDefinition2044 = new BitSet(new ulong[]{0x0000000000000810UL});
    public static readonly BitSet FOLLOW_END_in_classDefinition2047 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_classDefinition_in_script2097 = new BitSet(new ulong[]{0x0000000000000012UL});

}
}