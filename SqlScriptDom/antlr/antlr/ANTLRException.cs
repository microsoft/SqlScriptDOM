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
	
	using System;
using System.Runtime.Serialization;
	
	[Serializable]
	internal class ANTLRException : Exception
	{
		public ANTLRException() : base() 
		{
		}

		public ANTLRException(string s) : base(s) 
		{
		}

		public ANTLRException(string s, Exception inner) : base(s, inner)
		{
		}

        protected ANTLRException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
	}
}
