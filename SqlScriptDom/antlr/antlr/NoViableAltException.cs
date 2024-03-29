using System;
using System.Runtime.Serialization;

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
	
	[Serializable]
	internal class NoViableAltException : RecognitionException
	{
		public IToken token;
	
		public NoViableAltException(IToken t, string fileName_) : 
					base("NoViableAlt", fileName_, t.getLine(), t.getColumn())
		{
			token = t;
		}
		
        protected NoViableAltException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

		/*
		* Returns a clean error message (no line number/column information)
		*/
		override public string Message
		{
			get 
			{
                if (token != null)
                {
                    //return "unexpected token: " + token.getText();
                    return "unexpected token: " + token.ToString();
                }
                else
                    return "unexpected token: (null)";
 			}
		}
	}
}