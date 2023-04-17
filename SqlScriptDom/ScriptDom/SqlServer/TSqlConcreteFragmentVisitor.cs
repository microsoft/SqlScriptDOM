//------------------------------------------------------------------------------
// <copyright file="TSqlConcreteFragmentVisitor.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The base visitor for the concrete TSql fragments  
    /// </summary>
    public abstract partial class TSqlConcreteFragmentVisitor : TSqlFragmentVisitor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TSqlConcreteFragmentVisitor"/> class.
        /// </summary>
        protected TSqlConcreteFragmentVisitor()
            : base(false)
        {
        }
    }
}
