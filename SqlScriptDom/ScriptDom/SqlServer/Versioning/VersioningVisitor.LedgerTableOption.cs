//------------------------------------------------------------------------------
// <copyright file="VersioningVisitor.LedgerTableOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Globalization;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.Versioning
{
    partial class VersioningVisitor
    {
        /// <summary>
        /// Visitor for the LEDGER clause
        /// </summary>
        public override void ExplicitVisit(LedgerTableOption node)
        {
            if (node.OptionState == OptionState.On)
            {
                if (_targetVersion < SqlVersion.Sql160)
                {
                    AddVersioningError(node.StartOffset,
                                 node.StartLine,
                                 node.StartColumn,
                                 String.Format(CultureInfo.InvariantCulture, TSqlParserResource.SQL46140Message));
                }
            }
        }
    }
}
