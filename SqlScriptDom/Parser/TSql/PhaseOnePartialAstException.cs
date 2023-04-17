//------------------------------------------------------------------------------
// <copyright file="PhaseOnePartialAstException.cs" company="Microsoft">
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
    /// It is used to deliver the partially parsed DDL statement
    /// to the TSqlParser level in the call stack.
    /// </summary> 
    [Serializable()]
    
    internal sealed class PhaseOnePartialAstException : Exception
    {
        private TSqlStatement _statement;

        public TSqlStatement Statement
        {
            get { return _statement; }
        }

        /// <summary>
        /// Basic Constructor
        /// </summary>
        public PhaseOnePartialAstException(TSqlStatement statement) 
            : base()
        {
            _statement = statement;
        }

        /// <summary>
        /// Serialization Constructor.
        /// </summary>
        private PhaseOnePartialAstException(SerializationInfo info, StreamingContext context) : base(info, context)
        {            
        }
    }
}
