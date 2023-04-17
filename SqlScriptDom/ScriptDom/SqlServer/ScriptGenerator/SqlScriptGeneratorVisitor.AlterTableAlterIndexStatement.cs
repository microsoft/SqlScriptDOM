//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterTableAlterIndexStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterTableAlterIndexStatement node)
        {
            GenerateAlterTableHead(node);

            GenerateSpaceAndKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndKeyword(TSqlTokenType.Index);
            GenerateSpaceAndFragmentIfNotNull(node.IndexIdentifier);
            GenerateSpace();

            if (node.AlterIndexType == AlterIndexType.Rebuild)
            {
                AlterIndexTypeHelper.Instance.GenerateSourceForOption(_writer, node.AlterIndexType);
                GenerateIndexOptions(node.IndexOptions);
            }
        }
    }
}
