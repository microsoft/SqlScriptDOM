//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropSearchPropertyListStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropSearchPropertyListStatement node)
        {
            GenerateSpaceSeparatedTokens(TSqlTokenType.Drop,
                CodeGenerationSupporter.Search,
                CodeGenerationSupporter.Property,
                CodeGenerationSupporter.List);
            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
