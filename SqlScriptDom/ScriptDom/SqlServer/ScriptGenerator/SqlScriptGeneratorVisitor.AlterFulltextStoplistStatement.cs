//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterFulltextStoplistStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterFullTextStopListStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Fulltext);
            GenerateSpaceAndKeyword(TSqlTokenType.StopList);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateSpaceAndFragmentIfNotNull(node.Action);            
        }
    }
}
