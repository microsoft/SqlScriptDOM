//------------------------------------------------------------------------------
// <copyright file="TSqlParseErrorException.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
 
using System;
using System.Runtime.Serialization;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// This class is a real internal class to parser, 
    /// therefore it is not in the spec tree.
    /// It is used to pass the parse error to the stack level
    /// that can recover the error.
    /// </summary> 
    [Serializable()]
    
    internal sealed class TSqlParseErrorException : Exception
    {
        private ParseError _parseError;
        private bool _doNotLog;

        /// <summary>
        /// If this is true, do not log, i.e. put the error in to the error list.
        /// </summary>
        public bool DoNotLog
        {
            get { return _doNotLog; }
        }

        /// <summary>
        /// The parse error.
        /// </summary>
        public ParseError ParseError
        {
            get { return _parseError; }
        }

        public TSqlParseErrorException(ParseError error, bool doNotLog) 
            : base()
        {
            _parseError = error;
            _doNotLog = doNotLog;
        }

        public TSqlParseErrorException(ParseError error)
            : this(error, false)
        {
        }

        /// <summary>
        /// Serialization Constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private TSqlParseErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {            
        }
    }
}
