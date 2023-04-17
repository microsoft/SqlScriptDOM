//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterTableTriggerModificationStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterTableTriggerModificationStatement node)
        {
            GenerateAlterTableHead(node);

            GenerateSpace();
            GenerateTriggerEnforcement(node.TriggerEnforcement);
            GenerateSpaceAndKeyword(TSqlTokenType.Trigger); 
            GenerateSpace();

            if (node.All)
            {
                GenerateKeyword(TSqlTokenType.All); 
            }
            else
            {
                GenerateCommaSeparatedList(node.TriggerNames);
            }
        }
    }
}
