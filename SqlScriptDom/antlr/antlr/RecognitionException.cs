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
	internal class RecognitionException : ANTLRException
	{
		public string fileName; // not used by treeparsers
		public int line; // not used by treeparsers
		public int column; // not used by treeparsers
		
		public RecognitionException() : base("parsing error")
		{
			fileName = null;
			line = - 1;
			column = - 1;
		}
		
		/*
		* RecognitionException constructor comment.
		* @param s java.lang.String
		*/
		public RecognitionException(string s) : base(s)
		{
			fileName = null;
			line = - 1;
			column = - 1;
		}
		
		/*
		* RecognitionException constructor comment.
		* @param s java.lang.String
		*/
		public RecognitionException(string s, string fileName_, int line_, int column_) : base(s)
		{
			fileName = fileName_;
			line = line_;
			column = column_;
		}
		
        protected RecognitionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

		public virtual string getFilename()
		{
			return fileName;
		}
		
		public virtual int getLine()
		{
			return line;
		}
		
		public virtual int getColumn()
		{
			return column;
		}
				
		override public string ToString()
		{
			return FileLineFormatter.getFormatter().getFormatString(fileName, line, column) + Message;
		}
	}
}