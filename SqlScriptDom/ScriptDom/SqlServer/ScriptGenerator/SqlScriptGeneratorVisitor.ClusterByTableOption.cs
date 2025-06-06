﻿//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ClusterByTableOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ClusterByTableOption node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Cluster);
            GenerateSpaceAndKeyword(TSqlTokenType.By);
            GenerateSpace();
            GenerateParenthesisedCommaSeparatedList(node.Columns);
        }
    }
}
