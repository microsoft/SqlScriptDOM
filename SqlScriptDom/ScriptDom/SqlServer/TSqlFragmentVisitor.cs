//------------------------------------------------------------------------------
// <copyright file="TSqlFragmentVisitor.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The base visitor for the TSql script dom.  
    /// </summary>
    public abstract partial class TSqlFragmentVisitor
    {
        private readonly bool _visitBaseType;

        /// <summary>
        /// Initializes a new instance of the <see cref="TSqlFragmentVisitor"/> class.
        /// </summary>
        protected TSqlFragmentVisitor()
            : this(true)
        {
        }

        internal TSqlFragmentVisitor(bool visitBaseType)
        {
            _visitBaseType = visitBaseType;
        }

        internal bool VisitBaseType
        {
            get { return _visitBaseType; }
        }

        /// <summary>
        /// Visits the specified fragment.
        /// </summary>
        /// <param name="fragment">The fragment.</param>
        public virtual void Visit(TSqlFragment fragment)
        {
            // throw new NotImplementedException();
        }

        // Notice there is no ExplicitVisit with a parameter TSqlFragment, this is on purpose, noone would be calling that.

        // There will be a ExplicitVisit and Visit per type of fragments, e.g., CreateTableStatement
        // Visit calls by default should call Visit((TSqlFragment)param)
        // ExplicitVisit calls it's Visit and then the children.
    }
}
