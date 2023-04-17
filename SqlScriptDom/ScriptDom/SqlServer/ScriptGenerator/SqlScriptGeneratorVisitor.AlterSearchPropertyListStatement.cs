//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterSearchPropertyListStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterSearchPropertyListStatement node)
        {
            GenerateSpaceSeparatedTokens(TSqlTokenType.Alter,
                CodeGenerationSupporter.Search,
                CodeGenerationSupporter.Property,
                CodeGenerationSupporter.List);
            GenerateSpaceAndFragmentIfNotNull(node.Name);
            GenerateSpaceAndFragmentIfNotNull(node.Action);
        }
    }
}
