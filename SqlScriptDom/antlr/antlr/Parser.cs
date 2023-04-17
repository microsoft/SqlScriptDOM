using System;
using EventHandlerList			= System.ComponentModel.EventHandlerList;

using BitSet					= antlr.collections.impl.BitSet;

/*
	private Vector messageListeners;
	private Vector newLineListeners;
	private Vector matchListeners;
	private Vector tokenListeners;
	private Vector semPredListeners;
	private Vector synPredListeners;
	private Vector traceListeners;
*/
	
namespace antlr
{
	/*ANTLR Translator Generator
	* Project led by Terence Parr at http://www.jGuru.com
	* Software rights: http://www.antlr.org/license.html
	*
	* $Id:$
	*/

	//
	// ANTLR C# Code Generator by Micheal Jordan
	//                            Kunle Odutola       : kunle UNDERSCORE odutola AT hotmail DOT com
	//                            Anthony Oguntimehin
	//
	// With many thanks to Eric V. Smith from the ANTLR list.
	//

	internal abstract class Parser
	{
        // NOTE, olegr: Appears to be not used...
		// Used to store event delegates
        //private EventHandlerList events_ = new EventHandlerList();

        //protected internal EventHandlerList Events 
        //{
        //    get	{ return events_;	}
        //}

		// The unique keys for each event that Parser [objects] can generate
		internal static readonly object EnterRuleEventKey		= new object();
		internal static readonly object ExitRuleEventKey			= new object();
		internal static readonly object DoneEventKey				= new object();
		internal static readonly object ReportErrorEventKey		= new object();
		internal static readonly object ReportWarningEventKey	= new object();
		internal static readonly object NewLineEventKey			= new object();
		internal static readonly object MatchEventKey			= new object();
		internal static readonly object MatchNotEventKey			= new object();
		internal static readonly object MisMatchEventKey			= new object();
		internal static readonly object MisMatchNotEventKey		= new object();
		internal static readonly object ConsumeEventKey			= new object();
		internal static readonly object LAEventKey				= new object();
		internal static readonly object SemPredEvaluatedEventKey	= new object();
		internal static readonly object SynPredStartedEventKey	= new object();
		internal static readonly object SynPredFailedEventKey	= new object();
		internal static readonly object SynPredSucceededEventKey	= new object();

		protected internal ParserSharedInputState inputState;
		
		/*Nesting level of registered handlers */
		// protected int exceptionLevel = 0;
		
		/*Table of token type to token names */
		protected internal string[] tokenNames;

		private bool ignoreInvalidDebugCalls = false;
		
		/*Used to keep track of indentdepth for traceIn/Out */
		protected internal int traceDepth = 0;
		
		public Parser()
		{
			inputState = new ParserSharedInputState();
		}
		
		public Parser(ParserSharedInputState state)
		{
			inputState = state;
		}
		
		/// <summary>
		/// 
		/// </summary>
				
		/*Get another token object from the token stream */
		public abstract void  consume();
		/*Consume tokens until one matches the given token */
		public virtual void  consumeUntil(int tokenType)
		{
			while (LA(1) != Token.EOF_TYPE && LA(1) != tokenType)
			{
				consume();
			}
		}
		/*Consume tokens until one matches the given token set */
		public virtual void  consumeUntil(BitSet bset)
		{
			while (LA(1) != Token.EOF_TYPE && !bset.member(LA(1)))
			{
				consume();
			}
		}
		protected internal virtual void  defaultDebuggingSetup(TokenStream lexer, TokenBuffer tokBuf)
		{
			// by default, do nothing -- we're not debugging
		}

		public virtual string getFilename()
		{
			return inputState.filename;
		}
		
		public virtual ParserSharedInputState getInputState()
		{
			return inputState;
		}
		
		public virtual void  setInputState(ParserSharedInputState state)
		{
			inputState = state;
		}
		
		public virtual void resetState()
		{
			traceDepth = 0;
			inputState.reset();
		}

		public virtual string getTokenName(int num)
		{
			return tokenNames[num];
		}
		public virtual string[] getTokenNames()
		{
			return tokenNames;
		}
		public virtual bool isDebugMode()
		{
			return false;
		}
		/*Return the token type of the ith token of lookahead where i=1
		* is the current token being examined by the parser (i.e., it
		* has not been matched yet).
		*/
		public abstract int LA(int i);
		/*Return the ith token of lookahead */
		public abstract IToken LT(int i);
		// Forwarded to TokenBuffer
		public virtual int mark()
		{
			return inputState.input.mark();
		}
		/*Make sure current lookahead symbol matches token type <tt>t</tt>.
		* Throw an exception upon mismatch, which is catch by either the
		* error handler or by the syntactic predicate.
		*/
		public virtual void  match(int t)
		{
			if (LA(1) != t)
				throw new MismatchedTokenException(tokenNames, LT(1), t, false, getFilename());
			else
				consume();
		}
		/*Make sure current lookahead symbol matches the given set
		* Throw an exception upon mismatch, which is catch by either the
		* error handler or by the syntactic predicate.
		*/
		public virtual void  match(BitSet b)
		{
			if (!b.member(LA(1)))
				throw new MismatchedTokenException(tokenNames, LT(1), b, false, getFilename());
			else
				consume();
		}
		public virtual void  matchNot(int t)
		{
			if (LA(1) == t)
				throw new MismatchedTokenException(tokenNames, LT(1), t, true, getFilename());
			else
				consume();
		}

		/*Parser error-reporting function can be overridden in subclass */
		public virtual void reportError(RecognitionException ex)
		{
			Console.Error.WriteLine(ex);
		}
		
		/*Parser error-reporting function can be overridden in subclass */
		public virtual void reportError(string s)
		{
			if (getFilename() == null)
			{
				Console.Error.WriteLine("error: " + s);
			}
			else
			{
				Console.Error.WriteLine(getFilename() + ": error: " + s);
			}
		}
		
		/*Parser warning-reporting function can be overridden in subclass */
		public virtual void  reportWarning(string s)
		{
			if (getFilename() == null)
			{
				Console.Error.WriteLine("warning: " + s);
			}
			else
			{
				Console.Error.WriteLine(getFilename() + ": warning: " + s);
			}
		}
		
		public virtual void recover(RecognitionException ex, BitSet tokenSet)
		{
			consume();
			consumeUntil(tokenSet);
		}
		
		public virtual void  rewind(int pos)
		{
			inputState.input.rewind(pos);
		}

		public virtual void  setDebugMode(bool debugMode)
		{
			if (!ignoreInvalidDebugCalls)
				throw new ANTLRException("setDebugMode() only valid if parser built for debugging");
		}
		public virtual void  setFilename(string f)
		{
			inputState.filename = f;
		}
		public virtual void  setIgnoreInvalidDebugCalls(bool Value)
		{
			ignoreInvalidDebugCalls = Value;
		}
		/*Set or change the input token buffer */
		public virtual void  setTokenBuffer(TokenBuffer t)
		{
			inputState.input = t;
		}
		
		public virtual void  traceIndent()
		{
			 for (int i = 0; i < traceDepth; i++)
				Console.Out.Write(" ");
		}
		public virtual void  traceIn(string rname)
		{
			traceDepth += 1;
			traceIndent();
			Console.Out.WriteLine("> " + rname + "; LA(1)==" + LT(1).getText() + ((inputState.guessing > 0)?" [guessing]":""));
		}
		public virtual void  traceOut(string rname)
		{
			traceIndent();
			Console.Out.WriteLine("< " + rname + "; LA(1)==" + LT(1).getText() + ((inputState.guessing > 0)?" [guessing]":""));
			traceDepth -= 1;
		}
	}
}
