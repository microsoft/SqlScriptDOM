using System;
using IOException = System.IO.IOException;

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
	* Wrap an IOException in a CharStreamException
	*/

    // NOTE, olegr: We don't want wrapper exceptions - IOException is totally fine!
    //[Serializable]
    //internal class CharStreamIOException : CharStreamException
    //{
    //    public IOException io;
		
    //    public CharStreamIOException(IOException io) : base(io.Message)
    //    {
    //        this.io = io;
    //    }
    //}
}