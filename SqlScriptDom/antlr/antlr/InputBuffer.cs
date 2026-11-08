namespace antlr
{
	using System;
	using StringBuilder	= System.Text.StringBuilder;

	/*ANTLR Translator Generator
	* Project led by Terence Parr at http://www.jGuru.com
	* Software rights: http://www.antlr.org/license.html
	*/

	//
	// ANTLR C# Code Generator by Micheal Jordan
	//                            Kunle Odutola       : kunle UNDERSCORE odutola AT hotmail DOT com
	//                            Anthony Oguntimehin
	//
	// With many thanks to Eric V. Smith from the ANTLR list.
	//

	// SAS: Added this class to genericise the input buffers for scanners
	//      This allows a scanner to use a binary (FileInputStream) or
	//      text (FileReader) stream of data; the generated scanner
	//      subclass will define the input stream
	//      There are two subclasses to this: CharBuffer and ByteBuffer
	
	/// <summary>
	/// Represents a stream of characters fed to the lexer from that can be rewound 
	/// via mark()/rewind() methods.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A dynamic array is used to buffer up all the input characters.  Normally,
	/// "k" characters are stored in the buffer.  More characters may be stored 
	/// during guess mode (testing syntactic predicate), or when LT(i>k) is referenced.
	/// Consumption of characters is deferred.  In other words, reading the next
	/// character is not done by conume(), but deferred until needed by LA or LT.
	/// </para>
	/// </remarks>
	internal abstract class InputBuffer
	{
		// Number of active markers
		protected internal int nMarkers = 0;
		
		// Additional offset used when markers are active
		protected internal int markerOffset = 0;
		
		// Number of calls to consume() since last LA() or LT() call
		protected internal int numToConsume = 0;
		
		// Circular queue
		protected CharQueue queue;

		private const int CompactionThreshold = 1024;
		
		/*Create an input buffer */
		public InputBuffer()
		{
			queue = new CharQueue(16);
		}
		
		/*This method updates the state of the input buffer so that
		*  the text matched since the most recent mark() is no longer
		*  held by the buffer.  So, you either do a mark/rewind for
		*  failed predicate or mark/commit to keep on parsing without
		*  rewinding the input.
		*/
		public virtual void  commit()
		{
			nMarkers--;
		}
		
		/*Mark another character for deferred consumption */
		public virtual int consume()
		{
			numToConsume++;
			return LA(1);
		}
		
		/*Ensure that the input buffer is sufficiently full */
		public abstract void  fill(int amount);
		
		public virtual string getLAChars()
		{
			StringBuilder la = new StringBuilder();

			for (int i = markerOffset; i < queue.nbrEntries; i++)
			{
				la.Append((char)queue.elementAt(i));
			}

			return la.ToString();
		}
		
		public virtual string getMarkedChars()
		{
			StringBuilder marked = new StringBuilder();

			for (int i = markerOffset; i < queue.nbrEntries; i++)
			{
				marked.Append((char)queue.elementAt(i));
			}

			return marked.ToString();
		}
		
		public virtual bool isMarked()
		{
			return (nMarkers != 0);
		}
		
		/*Get a lookahead character */
		public virtual int LA(int i)
		{
			fill(i);
			// Entries are boxed char values for real input and a boxed int sentinel
			// (CharScanner.EOF_CHAR) for end-of-input; Convert.ToInt32 handles both so a
			// real U+FFFF (65535) stays distinct from the EOF sentinel (65536).
			return queue.elementAt(markerOffset + i - 1);
		}
		
		/*Return an integer marker that can be used to rewind the buffer to
		* its current state.
		*/
		public virtual int mark()
		{
			syncConsume();
			nMarkers++;
			return markerOffset;
		}
		
		/*Rewind the character buffer to a marker.
		* @param mark Marker returned previously from mark()
		*/
		public virtual void  rewind(int mark)
		{
			syncConsume();
			markerOffset = mark;
			nMarkers--;
		}
		
		/*Reset the input buffer
		*/
		public virtual void  reset()
		{
			nMarkers = 0;
			markerOffset = 0;
			numToConsume = 0;
			queue.reset();
		}
		
		/*Sync up deferred consumption */
		protected internal virtual void  syncConsume()
		{
			if (numToConsume == 0)
			{
				return;
			}

			markerOffset += numToConsume;
			numToConsume = 0;

			if (nMarkers == 0 && markerOffset >= CompactionThreshold)
			{
				queue.remove(markerOffset);
				markerOffset = 0;
			}
		}
	}
}
