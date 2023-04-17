//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.KillQueryNotificationSubscriptionStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(KillQueryNotificationSubscriptionStatement node)
        {
            GenerateKeyword(TSqlTokenType.Kill);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Query);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Notification);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Subscription);

            if (node.All)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.All); 
            }
            else
            {
                GenerateSpaceAndFragmentIfNotNull(node.SubscriptionId);
            }
        }
    }
}
