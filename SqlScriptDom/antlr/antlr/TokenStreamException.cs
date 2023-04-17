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

	/*
	* Anything that goes wrong while generating a stream of tokens.
	*/

	[Serializable]
	internal class TokenStreamException : ANTLRException
	{
		public TokenStreamException()
		{
		}
		public TokenStreamException(string s) : base(s)
		{
		}

        protected TokenStreamException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
	}
}