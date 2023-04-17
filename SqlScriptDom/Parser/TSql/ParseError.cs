//------------------------------------------------------------------------------
// <copyright file="ParseError.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// This class reports the error that occured during parsing.
    /// </summary>
    [Serializable()]
    public sealed class ParseError
    {
        private readonly int _number;
        private readonly int _offset;
        private readonly int _line;
        private readonly int _column;
        private readonly string _message;

        /// <summary>
        /// Constructor
        /// </summary>
        public ParseError(int number, int offset, int line, int column, string message)
        {
            _number = number;
            _offset = offset;
            _message = message;
            _line = line;
            _column = column;
        }

        /// <summary>
        /// The error code that uniqely represents the type of error.
        /// For example, may have the form XXXXX where X is a digit.
        /// </summary>
        public int Number
        {
            get { return _number; }
        }

        /// <summary>
        /// Represents the starting offset of the AST that caused the error.
        /// </summary>
        public int Offset
        {
            get { return _offset; }
        }

        /// <summary>
        /// Represents the starting line of the AST that caused the error.
        /// </summary>
        public int Line
        {
            get { return _line; }
        }

        /// <summary>
        /// Represents the starting column of the AST that caused the error.
        /// </summary>
        public int Column
        {
            get { return _column; }
        }

        /// <summary>
        /// Definition of the error. 
        /// </summary>
        public string Message
        {
            get { return _message; }
        }
    }
}
