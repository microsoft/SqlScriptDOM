//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateAvailabilityGroupStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateAvailabilityGroupStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Create);
            GenerateIdentifier(CodeGenerationSupporter.Availability);
            GenerateSpaceAndKeyword(TSqlTokenType.Group);

            GenerateSpaceAndFragmentIfNotNull(node.Name);
            
            GenerateCommaSeparatedWithClause(node.Options, false, true);
            NewLine();
            
            GenerateKeywordAndSpace(TSqlTokenType.For);
            if (node.Databases != null && node.Databases.Count > 0)
            {
                GenerateKeywordAndSpace(TSqlTokenType.Database);
                GenerateCommaSeparatedList(node.Databases);
                NewLine();
            }

            GenerateIdentifier(CodeGenerationSupporter.Replica);
            GenerateSpace();
            GenerateKeywordAndSpace(TSqlTokenType.On);
            GenerateCommaSeparatedList(node.Replicas,true);
        }

        public override void ExplicitVisit(LiteralAvailabilityGroupOption node)
        {
            Debug.Assert(node.OptionKind == AvailabilityGroupOptionKind.RequiredCopiesToCommit);
            GenerateNameEqualsValue(CodeGenerationSupporter.RequiredCopiesToCommit, node.Value);
        }
    }
}
