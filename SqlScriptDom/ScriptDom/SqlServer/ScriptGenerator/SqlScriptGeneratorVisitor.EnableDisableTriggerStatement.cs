//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.EnableDisableTriggerStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(EnableDisableTriggerStatement node)
        {
            GenerateTriggerEnforcement(node.TriggerEnforcement);
            GenerateSpaceAndKeyword(TSqlTokenType.Trigger);
            GenerateSpace();

            if (node.All)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.All); 
            }
            else
            {
                GenerateCommaSeparatedList(node.TriggerNames);
            }

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.On);
            GenerateSpaceAndFragmentIfNotNull(node.TriggerObject);
        }
    }
}
