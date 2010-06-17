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


public partial class ElfLexer : Lexer {
    public const int LT = 19;
    public const int CLASS = 31;
    public const int LBRACK = 41;
    public const int DEF = 4;
    public const int SingleLineComment = 50;
    public const int GTE = 22;
    public const int DQUOTE = 56;
    public const int SUB = 17;
    public const int NOT = 25;
    public const int AND = 26;
    public const int DecimalDigit = 53;
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
    public const int PAREXPR = 35;
    public const int NEQ = 24;
    public const int FUNC = 32;
    public const int DECL = 30;
    public const int BLOCK = 34;
    public const int OR = 27;
    public const int NEG = 13;
    public const int ASSIGN = 28;
    public const int GT = 20;
    public const int CALL = 37;
    public const int RTIMPL = 5;
    public const int DIV = 15;
    public const int END = 11;
    public const int SEMIC = 44;
    public const int BSLASH = 55;

    public override void ReportError(RecognitionException e)
    {
    	throw Elf.Exceptions.Parser.RecognitionExceptionHelper.Report(this, e);
    }


    // delegates
    // delegators

    public ElfLexer() 
    {
		InitializeCyclicDFAs();
    }
    public ElfLexer(ICharStream input)
		: this(input, null) {
    }
    public ElfLexer(ICharStream input, RecognizerSharedState state)
		: base(input, state) {
		InitializeCyclicDFAs(); 

    }
    
    override public string GrammarFileName
    {
    	get { return "Elf.g";} 
    }

    // $ANTLR start "DEF"
    public void mDEF() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DEF;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:15:5: ( 'def' )
            // Elf.g:15:7: 'def'
            {
            	Match("def"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DEF"

    // $ANTLR start "RTIMPL"
    public void mRTIMPL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = RTIMPL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:16:8: ( 'rtimpl' )
            // Elf.g:16:10: 'rtimpl'
            {
            	Match("rtimpl"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "RTIMPL"

    // $ANTLR start "VAR"
    public void mVAR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = VAR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:17:5: ( 'var' )
            // Elf.g:17:7: 'var'
            {
            	Match("var"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "VAR"

    // $ANTLR start "RET"
    public void mRET() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = RET;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:18:5: ( 'ret' )
            // Elf.g:18:7: 'ret'
            {
            	Match("ret"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "RET"

    // $ANTLR start "IF"
    public void mIF() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = IF;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:19:4: ( 'if' )
            // Elf.g:19:6: 'if'
            {
            	Match("if"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "IF"

    // $ANTLR start "THEN"
    public void mTHEN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = THEN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:20:6: ( 'then' )
            // Elf.g:20:8: 'then'
            {
            	Match("then"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "THEN"

    // $ANTLR start "ELSE"
    public void mELSE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ELSE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:21:6: ( 'else' )
            // Elf.g:21:8: 'else'
            {
            	Match("else"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ELSE"

    // $ANTLR start "END"
    public void mEND() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = END;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:22:5: ( 'end' )
            // Elf.g:22:7: 'end'
            {
            	Match("end"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "END"

    // $ANTLR start "MUL"
    public void mMUL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MUL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:23:5: ( '*' )
            // Elf.g:23:7: '*'
            {
            	Match('*'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MUL"

    // $ANTLR start "DIV"
    public void mDIV() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DIV;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:24:5: ( '/' )
            // Elf.g:24:7: '/'
            {
            	Match('/'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DIV"

    // $ANTLR start "ADD"
    public void mADD() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ADD;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:25:5: ( '+' )
            // Elf.g:25:7: '+'
            {
            	Match('+'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ADD"

    // $ANTLR start "SUB"
    public void mSUB() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SUB;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:26:5: ( '-' )
            // Elf.g:26:7: '-'
            {
            	Match('-'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SUB"

    // $ANTLR start "POW"
    public void mPOW() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = POW;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:27:5: ( '^' )
            // Elf.g:27:7: '^'
            {
            	Match('^'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "POW"

    // $ANTLR start "LT"
    public void mLT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:28:4: ( '<' )
            // Elf.g:28:6: '<'
            {
            	Match('<'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LT"

    // $ANTLR start "GT"
    public void mGT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = GT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:29:4: ( '>' )
            // Elf.g:29:6: '>'
            {
            	Match('>'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "GT"

    // $ANTLR start "LTE"
    public void mLTE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LTE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:30:5: ( '<=' )
            // Elf.g:30:7: '<='
            {
            	Match("<="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LTE"

    // $ANTLR start "GTE"
    public void mGTE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = GTE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:31:5: ( '>=' )
            // Elf.g:31:7: '>='
            {
            	Match(">="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "GTE"

    // $ANTLR start "EQ"
    public void mEQ() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = EQ;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:32:4: ( '==' )
            // Elf.g:32:6: '=='
            {
            	Match("=="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "EQ"

    // $ANTLR start "NEQ"
    public void mNEQ() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = NEQ;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:33:5: ( '!=' )
            // Elf.g:33:7: '!='
            {
            	Match("!="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "NEQ"

    // $ANTLR start "NOT"
    public void mNOT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = NOT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:34:5: ( '!' )
            // Elf.g:34:7: '!'
            {
            	Match('!'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "NOT"

    // $ANTLR start "AND"
    public void mAND() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = AND;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:35:5: ( '&&' )
            // Elf.g:35:7: '&&'
            {
            	Match("&&"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "AND"

    // $ANTLR start "OR"
    public void mOR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = OR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:36:4: ( '||' )
            // Elf.g:36:6: '||'
            {
            	Match("||"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "OR"

    // $ANTLR start "ASSIGN"
    public void mASSIGN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ASSIGN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:37:8: ( '=' )
            // Elf.g:37:10: '='
            {
            	Match('='); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ASSIGN"

    // $ANTLR start "LPAREN"
    public void mLPAREN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LPAREN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:38:8: ( '(' )
            // Elf.g:38:10: '('
            {
            	Match('('); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LPAREN"

    // $ANTLR start "RPAREN"
    public void mRPAREN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = RPAREN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:39:8: ( ')' )
            // Elf.g:39:10: ')'
            {
            	Match(')'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "RPAREN"

    // $ANTLR start "LBRACK"
    public void mLBRACK() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LBRACK;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:40:8: ( '[' )
            // Elf.g:40:10: '['
            {
            	Match('['); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LBRACK"

    // $ANTLR start "RBRACK"
    public void mRBRACK() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = RBRACK;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:41:8: ( ']' )
            // Elf.g:41:10: ']'
            {
            	Match(']'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "RBRACK"

    // $ANTLR start "COMMA"
    public void mCOMMA() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COMMA;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:42:7: ( ',' )
            // Elf.g:42:9: ','
            {
            	Match(','); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "COMMA"

    // $ANTLR start "SEMIC"
    public void mSEMIC() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SEMIC;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:43:7: ( ';' )
            // Elf.g:43:9: ';'
            {
            	Match(';'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SEMIC"

    // $ANTLR start "DOT"
    public void mDOT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DOT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:44:5: ( '.' )
            // Elf.g:44:7: '.'
            {
            	Match('.'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DOT"

    // $ANTLR start "WhiteSpace"
    public void mWhiteSpace() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = WhiteSpace;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:136:2: ( ( '\\u0009' | '\\u000b' | '\\u000c' | '\\u0020' )+ )
            // Elf.g:136:4: ( '\\u0009' | '\\u000b' | '\\u000c' | '\\u0020' )+
            {
            	// Elf.g:136:4: ( '\\u0009' | '\\u000b' | '\\u000c' | '\\u0020' )+
            	int cnt1 = 0;
            	do 
            	{
            	    int alt1 = 2;
            	    int LA1_0 = input.LA(1);

            	    if ( (LA1_0 == '\t' || (LA1_0 >= '\u000B' && LA1_0 <= '\f') || LA1_0 == ' ') )
            	    {
            	        alt1 = 1;
            	    }


            	    switch (alt1) 
            		{
            			case 1 :
            			    // Elf.g:
            			    {
            			    	if ( input.LA(1) == '\t' || (input.LA(1) >= '\u000B' && input.LA(1) <= '\f') || input.LA(1) == ' ' ) 
            			    	{
            			    	    input.Consume();

            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    Recover(mse);
            			    	    throw mse;}


            			    }
            			    break;

            			default:
            			    if ( cnt1 >= 1 ) goto loop1;
            		            EarlyExitException eee =
            		                new EarlyExitException(1, input);
            		            throw eee;
            	    }
            	    cnt1++;
            	} while (true);

            	loop1:
            		;	// Stops C# compiler whinging that label 'loop1' has no statements

            	 _channel = HIDDEN; 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "WhiteSpace"

    // $ANTLR start "EOL"
    public void mEOL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = EOL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:140:2: ( ( ( '\\n' ( '\\r' )? ) | '\\r' ) )
            // Elf.g:140:4: ( ( '\\n' ( '\\r' )? ) | '\\r' )
            {
            	// Elf.g:140:4: ( ( '\\n' ( '\\r' )? ) | '\\r' )
            	int alt3 = 2;
            	int LA3_0 = input.LA(1);

            	if ( (LA3_0 == '\n') )
            	{
            	    alt3 = 1;
            	}
            	else if ( (LA3_0 == '\r') )
            	{
            	    alt3 = 2;
            	}
            	else 
            	{
            	    NoViableAltException nvae_d3s0 =
            	        new NoViableAltException("", 3, 0, input);

            	    throw nvae_d3s0;
            	}
            	switch (alt3) 
            	{
            	    case 1 :
            	        // Elf.g:140:6: ( '\\n' ( '\\r' )? )
            	        {
            	        	// Elf.g:140:6: ( '\\n' ( '\\r' )? )
            	        	// Elf.g:140:8: '\\n' ( '\\r' )?
            	        	{
            	        		Match('\n'); 
            	        		// Elf.g:140:13: ( '\\r' )?
            	        		int alt2 = 2;
            	        		int LA2_0 = input.LA(1);

            	        		if ( (LA2_0 == '\r') )
            	        		{
            	        		    alt2 = 1;
            	        		}
            	        		switch (alt2) 
            	        		{
            	        		    case 1 :
            	        		        // Elf.g:140:13: '\\r'
            	        		        {
            	        		        	Match('\r'); 

            	        		        }
            	        		        break;

            	        		}


            	        	}


            	        }
            	        break;
            	    case 2 :
            	        // Elf.g:140:23: '\\r'
            	        {
            	        	Match('\r'); 

            	        }
            	        break;

            	}

            	 _channel = HIDDEN; 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "EOL"

    // $ANTLR start "MultiLineComment"
    public void mMultiLineComment() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MultiLineComment;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:147:2: ( '/*' ( options {greedy=false; } : . )* '*/' )
            // Elf.g:147:4: '/*' ( options {greedy=false; } : . )* '*/'
            {
            	Match("/*"); 

            	// Elf.g:147:9: ( options {greedy=false; } : . )*
            	do 
            	{
            	    int alt4 = 2;
            	    int LA4_0 = input.LA(1);

            	    if ( (LA4_0 == '*') )
            	    {
            	        int LA4_1 = input.LA(2);

            	        if ( (LA4_1 == '/') )
            	        {
            	            alt4 = 2;
            	        }
            	        else if ( ((LA4_1 >= '\u0000' && LA4_1 <= '.') || (LA4_1 >= '0' && LA4_1 <= '\uFFFF')) )
            	        {
            	            alt4 = 1;
            	        }


            	    }
            	    else if ( ((LA4_0 >= '\u0000' && LA4_0 <= ')') || (LA4_0 >= '+' && LA4_0 <= '\uFFFF')) )
            	    {
            	        alt4 = 1;
            	    }


            	    switch (alt4) 
            		{
            			case 1 :
            			    // Elf.g:147:41: .
            			    {
            			    	MatchAny(); 

            			    }
            			    break;

            			default:
            			    goto loop4;
            	    }
            	} while (true);

            	loop4:
            		;	// Stops C# compiler whining that label 'loop4' has no statements

            	Match("*/"); 

            	 _channel = HIDDEN; 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MultiLineComment"

    // $ANTLR start "SingleLineComment"
    public void mSingleLineComment() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SingleLineComment;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:151:2: ( '//' (~ ( '\\n' | '\\r' ) )* )
            // Elf.g:151:4: '//' (~ ( '\\n' | '\\r' ) )*
            {
            	Match("//"); 

            	// Elf.g:151:9: (~ ( '\\n' | '\\r' ) )*
            	do 
            	{
            	    int alt5 = 2;
            	    int LA5_0 = input.LA(1);

            	    if ( ((LA5_0 >= '\u0000' && LA5_0 <= '\t') || (LA5_0 >= '\u000B' && LA5_0 <= '\f') || (LA5_0 >= '\u000E' && LA5_0 <= '\uFFFF')) )
            	    {
            	        alt5 = 1;
            	    }


            	    switch (alt5) 
            		{
            			case 1 :
            			    // Elf.g:151:11: ~ ( '\\n' | '\\r' )
            			    {
            			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\f') || (input.LA(1) >= '\u000E' && input.LA(1) <= '\uFFFF') ) 
            			    	{
            			    	    input.Consume();

            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    Recover(mse);
            			    	    throw mse;}


            			    }
            			    break;

            			default:
            			    goto loop5;
            	    }
            	} while (true);

            	loop5:
            		;	// Stops C# compiler whining that label 'loop5' has no statements

            	 _channel = HIDDEN; 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SingleLineComment"

    // $ANTLR start "DecimalDigit"
    public void mDecimalDigit() // throws RecognitionException [2]
    {
    		try
    		{
            // Elf.g:237:2: ( '0' .. '9' )
            // Elf.g:237:4: '0' .. '9'
            {
            	MatchRange('0','9'); 

            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "DecimalDigit"

    // $ANTLR start "DecimalIntegerLiteral"
    public void mDecimalIntegerLiteral() // throws RecognitionException [2]
    {
    		try
    		{
            // Elf.g:241:2: ( '0' | '1' .. '9' ( DecimalDigit )* )
            int alt7 = 2;
            int LA7_0 = input.LA(1);

            if ( (LA7_0 == '0') )
            {
                alt7 = 1;
            }
            else if ( ((LA7_0 >= '1' && LA7_0 <= '9')) )
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
                    // Elf.g:241:4: '0'
                    {
                    	Match('0'); 

                    }
                    break;
                case 2 :
                    // Elf.g:242:4: '1' .. '9' ( DecimalDigit )*
                    {
                    	MatchRange('1','9'); 
                    	// Elf.g:242:13: ( DecimalDigit )*
                    	do 
                    	{
                    	    int alt6 = 2;
                    	    int LA6_0 = input.LA(1);

                    	    if ( ((LA6_0 >= '0' && LA6_0 <= '9')) )
                    	    {
                    	        alt6 = 1;
                    	    }


                    	    switch (alt6) 
                    		{
                    			case 1 :
                    			    // Elf.g:242:13: DecimalDigit
                    			    {
                    			    	mDecimalDigit(); 

                    			    }
                    			    break;

                    			default:
                    			    goto loop6;
                    	    }
                    	} while (true);

                    	loop6:
                    		;	// Stops C# compiler whining that label 'loop6' has no statements


                    }
                    break;

            }
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DecimalIntegerLiteral"

    // $ANTLR start "DecimalLiteral"
    public void mDecimalLiteral() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DecimalLiteral;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:246:2: ( DecimalIntegerLiteral '.' ( DecimalDigit )* | DecimalIntegerLiteral )
            int alt9 = 2;
            alt9 = dfa9.Predict(input);
            switch (alt9) 
            {
                case 1 :
                    // Elf.g:246:4: DecimalIntegerLiteral '.' ( DecimalDigit )*
                    {
                    	mDecimalIntegerLiteral(); 
                    	Match('.'); 
                    	// Elf.g:246:30: ( DecimalDigit )*
                    	do 
                    	{
                    	    int alt8 = 2;
                    	    int LA8_0 = input.LA(1);

                    	    if ( ((LA8_0 >= '0' && LA8_0 <= '9')) )
                    	    {
                    	        alt8 = 1;
                    	    }


                    	    switch (alt8) 
                    		{
                    			case 1 :
                    			    // Elf.g:246:30: DecimalDigit
                    			    {
                    			    	mDecimalDigit(); 

                    			    }
                    			    break;

                    			default:
                    			    goto loop8;
                    	    }
                    	} while (true);

                    	loop8:
                    		;	// Stops C# compiler whining that label 'loop8' has no statements


                    }
                    break;
                case 2 :
                    // Elf.g:247:4: DecimalIntegerLiteral
                    {
                    	mDecimalIntegerLiteral(); 

                    }
                    break;

            }
            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DecimalLiteral"

    // $ANTLR start "BSLASH"
    public void mBSLASH() // throws RecognitionException [2]
    {
    		try
    		{
            // Elf.g:255:2: ( '\\\\' )
            // Elf.g:255:4: '\\\\'
            {
            	Match('\\'); 

            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "BSLASH"

    // $ANTLR start "DQUOTE"
    public void mDQUOTE() // throws RecognitionException [2]
    {
    		try
    		{
            // Elf.g:259:2: ( '\"' )
            // Elf.g:259:4: '\"'
            {
            	Match('\"'); 

            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "DQUOTE"

    // $ANTLR start "SQUOTE"
    public void mSQUOTE() // throws RecognitionException [2]
    {
    		try
    		{
            // Elf.g:263:2: ( '\\'' )
            // Elf.g:263:4: '\\''
            {
            	Match('\''); 

            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "SQUOTE"

    // $ANTLR start "StringLiteral"
    public void mStringLiteral() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = StringLiteral;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:267:2: ( SQUOTE (~ ( SQUOTE | BSLASH | '\\n' | '\\r' ) | ( BSLASH ( SQUOTE | BSLASH ) ) )* SQUOTE | DQUOTE (~ ( DQUOTE | BSLASH | '\\n' | '\\r' ) | ( BSLASH ( DQUOTE | BSLASH ) ) )* DQUOTE )
            int alt12 = 2;
            int LA12_0 = input.LA(1);

            if ( (LA12_0 == '\'') )
            {
                alt12 = 1;
            }
            else if ( (LA12_0 == '\"') )
            {
                alt12 = 2;
            }
            else 
            {
                NoViableAltException nvae_d12s0 =
                    new NoViableAltException("", 12, 0, input);

                throw nvae_d12s0;
            }
            switch (alt12) 
            {
                case 1 :
                    // Elf.g:267:4: SQUOTE (~ ( SQUOTE | BSLASH | '\\n' | '\\r' ) | ( BSLASH ( SQUOTE | BSLASH ) ) )* SQUOTE
                    {
                    	mSQUOTE(); 
                    	// Elf.g:267:11: (~ ( SQUOTE | BSLASH | '\\n' | '\\r' ) | ( BSLASH ( SQUOTE | BSLASH ) ) )*
                    	do 
                    	{
                    	    int alt10 = 3;
                    	    int LA10_0 = input.LA(1);

                    	    if ( ((LA10_0 >= '\u0000' && LA10_0 <= '\t') || (LA10_0 >= '\u000B' && LA10_0 <= '\f') || (LA10_0 >= '\u000E' && LA10_0 <= '&') || (LA10_0 >= '(' && LA10_0 <= '[') || (LA10_0 >= ']' && LA10_0 <= '\uFFFF')) )
                    	    {
                    	        alt10 = 1;
                    	    }
                    	    else if ( (LA10_0 == '\\') )
                    	    {
                    	        alt10 = 2;
                    	    }


                    	    switch (alt10) 
                    		{
                    			case 1 :
                    			    // Elf.g:267:13: ~ ( SQUOTE | BSLASH | '\\n' | '\\r' )
                    			    {
                    			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\f') || (input.LA(1) >= '\u000E' && input.LA(1) <= '&') || (input.LA(1) >= '(' && input.LA(1) <= '[') || (input.LA(1) >= ']' && input.LA(1) <= '\uFFFF') ) 
                    			    	{
                    			    	    input.Consume();

                    			    	}
                    			    	else 
                    			    	{
                    			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
                    			    	    Recover(mse);
                    			    	    throw mse;}


                    			    }
                    			    break;
                    			case 2 :
                    			    // Elf.g:267:50: ( BSLASH ( SQUOTE | BSLASH ) )
                    			    {
                    			    	// Elf.g:267:50: ( BSLASH ( SQUOTE | BSLASH ) )
                    			    	// Elf.g:267:52: BSLASH ( SQUOTE | BSLASH )
                    			    	{
                    			    		mBSLASH(); 
                    			    		if ( input.LA(1) == '\'' || input.LA(1) == '\\' ) 
                    			    		{
                    			    		    input.Consume();

                    			    		}
                    			    		else 
                    			    		{
                    			    		    MismatchedSetException mse = new MismatchedSetException(null,input);
                    			    		    Recover(mse);
                    			    		    throw mse;}


                    			    	}


                    			    }
                    			    break;

                    			default:
                    			    goto loop10;
                    	    }
                    	} while (true);

                    	loop10:
                    		;	// Stops C# compiler whining that label 'loop10' has no statements

                    	mSQUOTE(); 

                    }
                    break;
                case 2 :
                    // Elf.g:268:4: DQUOTE (~ ( DQUOTE | BSLASH | '\\n' | '\\r' ) | ( BSLASH ( DQUOTE | BSLASH ) ) )* DQUOTE
                    {
                    	mDQUOTE(); 
                    	// Elf.g:268:11: (~ ( DQUOTE | BSLASH | '\\n' | '\\r' ) | ( BSLASH ( DQUOTE | BSLASH ) ) )*
                    	do 
                    	{
                    	    int alt11 = 3;
                    	    int LA11_0 = input.LA(1);

                    	    if ( ((LA11_0 >= '\u0000' && LA11_0 <= '\t') || (LA11_0 >= '\u000B' && LA11_0 <= '\f') || (LA11_0 >= '\u000E' && LA11_0 <= '!') || (LA11_0 >= '#' && LA11_0 <= '[') || (LA11_0 >= ']' && LA11_0 <= '\uFFFF')) )
                    	    {
                    	        alt11 = 1;
                    	    }
                    	    else if ( (LA11_0 == '\\') )
                    	    {
                    	        alt11 = 2;
                    	    }


                    	    switch (alt11) 
                    		{
                    			case 1 :
                    			    // Elf.g:268:13: ~ ( DQUOTE | BSLASH | '\\n' | '\\r' )
                    			    {
                    			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\f') || (input.LA(1) >= '\u000E' && input.LA(1) <= '!') || (input.LA(1) >= '#' && input.LA(1) <= '[') || (input.LA(1) >= ']' && input.LA(1) <= '\uFFFF') ) 
                    			    	{
                    			    	    input.Consume();

                    			    	}
                    			    	else 
                    			    	{
                    			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
                    			    	    Recover(mse);
                    			    	    throw mse;}


                    			    }
                    			    break;
                    			case 2 :
                    			    // Elf.g:268:50: ( BSLASH ( DQUOTE | BSLASH ) )
                    			    {
                    			    	// Elf.g:268:50: ( BSLASH ( DQUOTE | BSLASH ) )
                    			    	// Elf.g:268:52: BSLASH ( DQUOTE | BSLASH )
                    			    	{
                    			    		mBSLASH(); 
                    			    		if ( input.LA(1) == '\"' || input.LA(1) == '\\' ) 
                    			    		{
                    			    		    input.Consume();

                    			    		}
                    			    		else 
                    			    		{
                    			    		    MismatchedSetException mse = new MismatchedSetException(null,input);
                    			    		    Recover(mse);
                    			    		    throw mse;}


                    			    	}


                    			    }
                    			    break;

                    			default:
                    			    goto loop11;
                    	    }
                    	} while (true);

                    	loop11:
                    		;	// Stops C# compiler whining that label 'loop11' has no statements

                    	mDQUOTE(); 

                    }
                    break;

            }
            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "StringLiteral"

    // $ANTLR start "IdentifierStart"
    public void mIdentifierStart() // throws RecognitionException [2]
    {
    		try
    		{
            // Elf.g:278:2: ( 'a' .. 'z' | 'A' .. 'Z' | '\\u0410' .. '\\u042f' | '\\u0430' .. '\\u044f' | '\\u0401' | '\\u0451' | '_' )
            // Elf.g:
            {
            	if ( (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') || input.LA(1) == '\u0401' || (input.LA(1) >= '\u0410' && input.LA(1) <= '\u044F') || input.LA(1) == '\u0451' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "IdentifierStart"

    // $ANTLR start "IdentifierPart"
    public void mIdentifierPart() // throws RecognitionException [2]
    {
    		try
    		{
            // Elf.g:286:2: ( DecimalDigit | IdentifierStart | DOT | '$' )
            // Elf.g:
            {
            	if ( input.LA(1) == '$' || input.LA(1) == '.' || (input.LA(1) >= '0' && input.LA(1) <= '9') || (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') || input.LA(1) == '\u0401' || (input.LA(1) >= '\u0410' && input.LA(1) <= '\u044F') || input.LA(1) == '\u0451' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "IdentifierPart"

    // $ANTLR start "Loophole"
    public void mLoophole() // throws RecognitionException [2]
    {
    		try
    		{
            // Elf.g:293:2: ( '?' )
            // Elf.g:293:4: '?'
            {
            	Match('?'); 

            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "Loophole"

    // $ANTLR start "Identifier"
    public void mIdentifier() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Identifier;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Elf.g:297:2: ( Loophole | IdentifierStart ( IdentifierPart )* )
            int alt14 = 2;
            int LA14_0 = input.LA(1);

            if ( (LA14_0 == '?') )
            {
                alt14 = 1;
            }
            else if ( ((LA14_0 >= 'A' && LA14_0 <= 'Z') || LA14_0 == '_' || (LA14_0 >= 'a' && LA14_0 <= 'z') || LA14_0 == '\u0401' || (LA14_0 >= '\u0410' && LA14_0 <= '\u044F') || LA14_0 == '\u0451') )
            {
                alt14 = 2;
            }
            else 
            {
                NoViableAltException nvae_d14s0 =
                    new NoViableAltException("", 14, 0, input);

                throw nvae_d14s0;
            }
            switch (alt14) 
            {
                case 1 :
                    // Elf.g:297:4: Loophole
                    {
                    	mLoophole(); 

                    }
                    break;
                case 2 :
                    // Elf.g:298:4: IdentifierStart ( IdentifierPart )*
                    {
                    	mIdentifierStart(); 
                    	// Elf.g:298:20: ( IdentifierPart )*
                    	do 
                    	{
                    	    int alt13 = 2;
                    	    int LA13_0 = input.LA(1);

                    	    if ( (LA13_0 == '$' || LA13_0 == '.' || (LA13_0 >= '0' && LA13_0 <= '9') || (LA13_0 >= 'A' && LA13_0 <= 'Z') || LA13_0 == '_' || (LA13_0 >= 'a' && LA13_0 <= 'z') || LA13_0 == '\u0401' || (LA13_0 >= '\u0410' && LA13_0 <= '\u044F') || LA13_0 == '\u0451') )
                    	    {
                    	        alt13 = 1;
                    	    }


                    	    switch (alt13) 
                    		{
                    			case 1 :
                    			    // Elf.g:298:20: IdentifierPart
                    			    {
                    			    	mIdentifierPart(); 

                    			    }
                    			    break;

                    			default:
                    			    goto loop13;
                    	    }
                    	} while (true);

                    	loop13:
                    		;	// Stops C# compiler whining that label 'loop13' has no statements


                    }
                    break;

            }
            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "Identifier"

    override public void mTokens() // throws RecognitionException 
    {
        // Elf.g:1:8: ( DEF | RTIMPL | VAR | RET | IF | THEN | ELSE | END | MUL | DIV | ADD | SUB | POW | LT | GT | LTE | GTE | EQ | NEQ | NOT | AND | OR | ASSIGN | LPAREN | RPAREN | LBRACK | RBRACK | COMMA | SEMIC | DOT | WhiteSpace | EOL | MultiLineComment | SingleLineComment | DecimalLiteral | StringLiteral | Identifier )
        int alt15 = 37;
        alt15 = dfa15.Predict(input);
        switch (alt15) 
        {
            case 1 :
                // Elf.g:1:10: DEF
                {
                	mDEF(); 

                }
                break;
            case 2 :
                // Elf.g:1:14: RTIMPL
                {
                	mRTIMPL(); 

                }
                break;
            case 3 :
                // Elf.g:1:21: VAR
                {
                	mVAR(); 

                }
                break;
            case 4 :
                // Elf.g:1:25: RET
                {
                	mRET(); 

                }
                break;
            case 5 :
                // Elf.g:1:29: IF
                {
                	mIF(); 

                }
                break;
            case 6 :
                // Elf.g:1:32: THEN
                {
                	mTHEN(); 

                }
                break;
            case 7 :
                // Elf.g:1:37: ELSE
                {
                	mELSE(); 

                }
                break;
            case 8 :
                // Elf.g:1:42: END
                {
                	mEND(); 

                }
                break;
            case 9 :
                // Elf.g:1:46: MUL
                {
                	mMUL(); 

                }
                break;
            case 10 :
                // Elf.g:1:50: DIV
                {
                	mDIV(); 

                }
                break;
            case 11 :
                // Elf.g:1:54: ADD
                {
                	mADD(); 

                }
                break;
            case 12 :
                // Elf.g:1:58: SUB
                {
                	mSUB(); 

                }
                break;
            case 13 :
                // Elf.g:1:62: POW
                {
                	mPOW(); 

                }
                break;
            case 14 :
                // Elf.g:1:66: LT
                {
                	mLT(); 

                }
                break;
            case 15 :
                // Elf.g:1:69: GT
                {
                	mGT(); 

                }
                break;
            case 16 :
                // Elf.g:1:72: LTE
                {
                	mLTE(); 

                }
                break;
            case 17 :
                // Elf.g:1:76: GTE
                {
                	mGTE(); 

                }
                break;
            case 18 :
                // Elf.g:1:80: EQ
                {
                	mEQ(); 

                }
                break;
            case 19 :
                // Elf.g:1:83: NEQ
                {
                	mNEQ(); 

                }
                break;
            case 20 :
                // Elf.g:1:87: NOT
                {
                	mNOT(); 

                }
                break;
            case 21 :
                // Elf.g:1:91: AND
                {
                	mAND(); 

                }
                break;
            case 22 :
                // Elf.g:1:95: OR
                {
                	mOR(); 

                }
                break;
            case 23 :
                // Elf.g:1:98: ASSIGN
                {
                	mASSIGN(); 

                }
                break;
            case 24 :
                // Elf.g:1:105: LPAREN
                {
                	mLPAREN(); 

                }
                break;
            case 25 :
                // Elf.g:1:112: RPAREN
                {
                	mRPAREN(); 

                }
                break;
            case 26 :
                // Elf.g:1:119: LBRACK
                {
                	mLBRACK(); 

                }
                break;
            case 27 :
                // Elf.g:1:126: RBRACK
                {
                	mRBRACK(); 

                }
                break;
            case 28 :
                // Elf.g:1:133: COMMA
                {
                	mCOMMA(); 

                }
                break;
            case 29 :
                // Elf.g:1:139: SEMIC
                {
                	mSEMIC(); 

                }
                break;
            case 30 :
                // Elf.g:1:145: DOT
                {
                	mDOT(); 

                }
                break;
            case 31 :
                // Elf.g:1:149: WhiteSpace
                {
                	mWhiteSpace(); 

                }
                break;
            case 32 :
                // Elf.g:1:160: EOL
                {
                	mEOL(); 

                }
                break;
            case 33 :
                // Elf.g:1:164: MultiLineComment
                {
                	mMultiLineComment(); 

                }
                break;
            case 34 :
                // Elf.g:1:181: SingleLineComment
                {
                	mSingleLineComment(); 

                }
                break;
            case 35 :
                // Elf.g:1:199: DecimalLiteral
                {
                	mDecimalLiteral(); 

                }
                break;
            case 36 :
                // Elf.g:1:214: StringLiteral
                {
                	mStringLiteral(); 

                }
                break;
            case 37 :
                // Elf.g:1:228: Identifier
                {
                	mIdentifier(); 

                }
                break;

        }

    }


    protected DFA9 dfa9;
    protected DFA15 dfa15;
	private void InitializeCyclicDFAs()
	{
	    this.dfa9 = new DFA9(this);
	    this.dfa15 = new DFA15(this);


	}

    const string DFA9_eotS =
        "\x01\uffff\x02\x03\x02\uffff\x01\x03";
    const string DFA9_eofS =
        "\x06\uffff";
    const string DFA9_minS =
        "\x01\x30\x02\x2e\x02\uffff\x01\x2e";
    const string DFA9_maxS =
        "\x01\x39\x01\x2e\x01\x39\x02\uffff\x01\x39";
    const string DFA9_acceptS =
        "\x03\uffff\x01\x02\x01\x01\x01\uffff";
    const string DFA9_specialS =
        "\x06\uffff}>";
    static readonly string[] DFA9_transitionS = {
            "\x01\x01\x09\x02",
            "\x01\x04",
            "\x01\x04\x01\uffff\x0a\x05",
            "",
            "",
            "\x01\x04\x01\uffff\x0a\x05"
    };

    static readonly short[] DFA9_eot = DFA.UnpackEncodedString(DFA9_eotS);
    static readonly short[] DFA9_eof = DFA.UnpackEncodedString(DFA9_eofS);
    static readonly char[] DFA9_min = DFA.UnpackEncodedStringToUnsignedChars(DFA9_minS);
    static readonly char[] DFA9_max = DFA.UnpackEncodedStringToUnsignedChars(DFA9_maxS);
    static readonly short[] DFA9_accept = DFA.UnpackEncodedString(DFA9_acceptS);
    static readonly short[] DFA9_special = DFA.UnpackEncodedString(DFA9_specialS);
    static readonly short[][] DFA9_transition = DFA.UnpackEncodedStringArray(DFA9_transitionS);

    protected class DFA9 : DFA
    {
        public DFA9(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 9;
            this.eot = DFA9_eot;
            this.eof = DFA9_eof;
            this.min = DFA9_min;
            this.max = DFA9_max;
            this.accept = DFA9_accept;
            this.special = DFA9_special;
            this.transition = DFA9_transition;

        }

        override public string Description
        {
            get { return "245:1: DecimalLiteral : ( DecimalIntegerLiteral '.' ( DecimalDigit )* | DecimalIntegerLiteral );"; }
        }

    }

    const string DFA15_eotS =
        "\x01\uffff\x06\x1d\x01\uffff\x01\x28\x03\uffff\x01\x2a\x01\x2c"+
        "\x01\x2e\x01\x30\x0e\uffff\x04\x1d\x01\x35\x03\x1d\x0b\uffff\x01"+
        "\x39\x01\x1d\x01\x3b\x01\x3c\x01\uffff\x02\x1d\x01\x3f\x01\uffff"+
        "\x01\x1d\x02\uffff\x01\x41\x01\x42\x01\uffff\x01\x1d\x02\uffff\x01"+
        "\x44\x01\uffff";
    const string DFA15_eofS =
        "\x45\uffff";
    const string DFA15_minS =
        "\x01\x09\x02\x65\x01\x61\x01\x66\x01\x68\x01\x6c\x01\uffff\x01"+
        "\x2a\x03\uffff\x04\x3d\x0e\uffff\x01\x66\x01\x69\x01\x74\x01\x72"+
        "\x01\x24\x01\x65\x01\x73\x01\x64\x0b\uffff\x01\x24\x01\x6d\x02\x24"+
        "\x01\uffff\x01\x6e\x01\x65\x01\x24\x01\uffff\x01\x70\x02\uffff\x02"+
        "\x24\x01\uffff\x01\x6c\x02\uffff\x01\x24\x01\uffff";
    const string DFA15_maxS =
        "\x01\u0451\x01\x65\x01\x74\x01\x61\x01\x66\x01\x68\x01\x6e\x01"+
        "\uffff\x01\x2f\x03\uffff\x04\x3d\x0e\uffff\x01\x66\x01\x69\x01\x74"+
        "\x01\x72\x01\u0451\x01\x65\x01\x73\x01\x64\x0b\uffff\x01\u0451\x01"+
        "\x6d\x02\u0451\x01\uffff\x01\x6e\x01\x65\x01\u0451\x01\uffff\x01"+
        "\x70\x02\uffff\x02\u0451\x01\uffff\x01\x6c\x02\uffff\x01\u0451\x01"+
        "\uffff";
    const string DFA15_acceptS =
        "\x07\uffff\x01\x09\x01\uffff\x01\x0b\x01\x0c\x01\x0d\x04\uffff"+
        "\x01\x15\x01\x16\x01\x18\x01\x19\x01\x1a\x01\x1b\x01\x1c\x01\x1d"+
        "\x01\x1e\x01\x1f\x01\x20\x01\x23\x01\x24\x01\x25\x08\uffff\x01\x21"+
        "\x01\x22\x01\x0a\x01\x10\x01\x0e\x01\x11\x01\x0f\x01\x12\x01\x17"+
        "\x01\x13\x01\x14\x04\uffff\x01\x05\x03\uffff\x01\x01\x01\uffff\x01"+
        "\x04\x01\x03\x02\uffff\x01\x08\x01\uffff\x01\x06\x01\x07\x01\uffff"+
        "\x01\x02";
    const string DFA15_specialS =
        "\x45\uffff}>";
    static readonly string[] DFA15_transitionS = {
            "\x01\x19\x01\x1a\x02\x19\x01\x1a\x12\uffff\x01\x19\x01\x0f"+
            "\x01\x1c\x03\uffff\x01\x10\x01\x1c\x01\x12\x01\x13\x01\x07\x01"+
            "\x09\x01\x16\x01\x0a\x01\x18\x01\x08\x0a\x1b\x01\uffff\x01\x17"+
            "\x01\x0c\x01\x0e\x01\x0d\x01\x1d\x01\uffff\x1a\x1d\x01\x14\x01"+
            "\uffff\x01\x15\x01\x0b\x01\x1d\x01\uffff\x03\x1d\x01\x01\x01"+
            "\x06\x03\x1d\x01\x04\x08\x1d\x01\x02\x01\x1d\x01\x05\x01\x1d"+
            "\x01\x03\x04\x1d\x01\uffff\x01\x11\u0384\uffff\x01\x1d\x0e\uffff"+
            "\x40\x1d\x01\uffff\x01\x1d",
            "\x01\x1e",
            "\x01\x20\x0e\uffff\x01\x1f",
            "\x01\x21",
            "\x01\x22",
            "\x01\x23",
            "\x01\x24\x01\uffff\x01\x25",
            "",
            "\x01\x26\x04\uffff\x01\x27",
            "",
            "",
            "",
            "\x01\x29",
            "\x01\x2b",
            "\x01\x2d",
            "\x01\x2f",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x31",
            "\x01\x32",
            "\x01\x33",
            "\x01\x34",
            "\x01\x1d\x09\uffff\x01\x1d\x01\uffff\x0a\x1d\x07\uffff\x1a"+
            "\x1d\x04\uffff\x01\x1d\x01\uffff\x1a\x1d\u0386\uffff\x01\x1d"+
            "\x0e\uffff\x40\x1d\x01\uffff\x01\x1d",
            "\x01\x36",
            "\x01\x37",
            "\x01\x38",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x1d\x09\uffff\x01\x1d\x01\uffff\x0a\x1d\x07\uffff\x1a"+
            "\x1d\x04\uffff\x01\x1d\x01\uffff\x1a\x1d\u0386\uffff\x01\x1d"+
            "\x0e\uffff\x40\x1d\x01\uffff\x01\x1d",
            "\x01\x3a",
            "\x01\x1d\x09\uffff\x01\x1d\x01\uffff\x0a\x1d\x07\uffff\x1a"+
            "\x1d\x04\uffff\x01\x1d\x01\uffff\x1a\x1d\u0386\uffff\x01\x1d"+
            "\x0e\uffff\x40\x1d\x01\uffff\x01\x1d",
            "\x01\x1d\x09\uffff\x01\x1d\x01\uffff\x0a\x1d\x07\uffff\x1a"+
            "\x1d\x04\uffff\x01\x1d\x01\uffff\x1a\x1d\u0386\uffff\x01\x1d"+
            "\x0e\uffff\x40\x1d\x01\uffff\x01\x1d",
            "",
            "\x01\x3d",
            "\x01\x3e",
            "\x01\x1d\x09\uffff\x01\x1d\x01\uffff\x0a\x1d\x07\uffff\x1a"+
            "\x1d\x04\uffff\x01\x1d\x01\uffff\x1a\x1d\u0386\uffff\x01\x1d"+
            "\x0e\uffff\x40\x1d\x01\uffff\x01\x1d",
            "",
            "\x01\x40",
            "",
            "",
            "\x01\x1d\x09\uffff\x01\x1d\x01\uffff\x0a\x1d\x07\uffff\x1a"+
            "\x1d\x04\uffff\x01\x1d\x01\uffff\x1a\x1d\u0386\uffff\x01\x1d"+
            "\x0e\uffff\x40\x1d\x01\uffff\x01\x1d",
            "\x01\x1d\x09\uffff\x01\x1d\x01\uffff\x0a\x1d\x07\uffff\x1a"+
            "\x1d\x04\uffff\x01\x1d\x01\uffff\x1a\x1d\u0386\uffff\x01\x1d"+
            "\x0e\uffff\x40\x1d\x01\uffff\x01\x1d",
            "",
            "\x01\x43",
            "",
            "",
            "\x01\x1d\x09\uffff\x01\x1d\x01\uffff\x0a\x1d\x07\uffff\x1a"+
            "\x1d\x04\uffff\x01\x1d\x01\uffff\x1a\x1d\u0386\uffff\x01\x1d"+
            "\x0e\uffff\x40\x1d\x01\uffff\x01\x1d",
            ""
    };

    static readonly short[] DFA15_eot = DFA.UnpackEncodedString(DFA15_eotS);
    static readonly short[] DFA15_eof = DFA.UnpackEncodedString(DFA15_eofS);
    static readonly char[] DFA15_min = DFA.UnpackEncodedStringToUnsignedChars(DFA15_minS);
    static readonly char[] DFA15_max = DFA.UnpackEncodedStringToUnsignedChars(DFA15_maxS);
    static readonly short[] DFA15_accept = DFA.UnpackEncodedString(DFA15_acceptS);
    static readonly short[] DFA15_special = DFA.UnpackEncodedString(DFA15_specialS);
    static readonly short[][] DFA15_transition = DFA.UnpackEncodedStringArray(DFA15_transitionS);

    protected class DFA15 : DFA
    {
        public DFA15(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 15;
            this.eot = DFA15_eot;
            this.eof = DFA15_eof;
            this.min = DFA15_min;
            this.max = DFA15_max;
            this.accept = DFA15_accept;
            this.special = DFA15_special;
            this.transition = DFA15_transition;

        }

        override public string Description
        {
            get { return "1:1: Tokens : ( DEF | RTIMPL | VAR | RET | IF | THEN | ELSE | END | MUL | DIV | ADD | SUB | POW | LT | GT | LTE | GTE | EQ | NEQ | NOT | AND | OR | ASSIGN | LPAREN | RPAREN | LBRACK | RBRACK | COMMA | SEMIC | DOT | WhiteSpace | EOL | MultiLineComment | SingleLineComment | DecimalLiteral | StringLiteral | Identifier );"; }
        }

    }

 
    
}
}