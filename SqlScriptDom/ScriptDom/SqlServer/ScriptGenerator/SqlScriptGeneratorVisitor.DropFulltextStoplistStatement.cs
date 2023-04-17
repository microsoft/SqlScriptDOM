//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropFulltextStoplistStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropFullTextStopListStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Fulltext);
            GenerateSpaceAndKeyword(TSqlTokenType.StopList);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
