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
	internal class SemanticException : RecognitionException
	{
		public SemanticException(string s) : base(s)
		{
		}
		
		public SemanticException(string s, string fileName, int line, int column) :
					base(s, fileName, line, column)
		{
		}

        protected SemanticException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
	}
}