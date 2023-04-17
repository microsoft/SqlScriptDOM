//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.KillStatsJobStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(KillStatsJobStatement node)
        {
            GenerateKeyword(TSqlTokenType.Kill);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Stats);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Job);
            GenerateSpaceAndFragmentIfNotNull(node.JobId);
        }
    }
}
