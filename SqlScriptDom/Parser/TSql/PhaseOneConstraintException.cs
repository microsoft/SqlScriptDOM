//------------------------------------------------------------------------------
// <copyright file="PhaseOneConstraintException.cs" company="Microsoft">
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
    /// It is used to deliver the partially parsed constraint fragment
    /// to the tableConstraint level in the call stack.
    /// </summary> 
    [Serializable()]
    
    internal sealed class PhaseOneConstraintException : Exception
    {
        private ConstraintDefinition _constraint;

        public ConstraintDefinition Constraint
        {
            get { return _constraint; }
        }

        public PhaseOneConstraintException(ConstraintDefinition constraint) 
            : base()
        {
            _constraint = constraint;
        }

        private PhaseOneConstraintException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {            
        }
    }
}

