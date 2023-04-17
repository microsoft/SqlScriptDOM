//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterBrokerPriorityStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterBrokerPriorityStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Alter);
            GenerateBrokerPriorityStatementBody(node);
        }
    }
}
