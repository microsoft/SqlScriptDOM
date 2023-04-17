//------------------------------------------------------------------------------
// <copyright file="PhaseOneBatchException.cs" company="Microsoft">
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
    /// It is used to notify the top level to re-enter the script.
    /// </summary> 
    [Serializable()]
    
    internal sealed class PhaseOneBatchException : Exception
    {
        /// <summary>
        /// Basic Constructor
        /// </summary>
        public PhaseOneBatchException() 
            : base()
        { }

        /// <summary>
        /// Serialization Constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private PhaseOneBatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {            
        }
    }
}
