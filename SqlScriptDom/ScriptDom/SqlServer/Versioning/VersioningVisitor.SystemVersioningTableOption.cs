//------------------------------------------------------------------------------
// <copyright file="VersioningVisitor.SystemVersioningTableOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.Versioning
{
    partial class VersioningVisitor
    {
        /// <summary>
        /// Visitor for the SYSTEM_VERSIONING clause
        /// </summary>
        public override void ExplicitVisit(SystemVersioningTableOption node)
        {
            if (node.OptionState == OptionState.On)
            {
                if (node.RetentionPeriod != null)
                {
                    if (!node.RetentionPeriod.IsInfinity)
                    {
                        if (_targetEngineType != SqlEngineType.SqlAzure && _targetVersion < SqlVersion.Sql140)
                        {
                            AddVersioningError(node.StartOffset,
                                node.StartLine,
                                node.StartColumn,
                                "HISTORY_RETENTION_PERIOD");
                        }
                    }
                }
            }
        }
    }
}
