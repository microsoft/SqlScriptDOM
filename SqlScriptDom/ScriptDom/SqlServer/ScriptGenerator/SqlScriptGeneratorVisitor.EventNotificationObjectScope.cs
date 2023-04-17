//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.EventNotificationObjectScope.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(EventNotificationObjectScope node)
        {
            switch (node.Target)
            {
                case EventNotificationTarget.Server:
                    GenerateIdentifier(CodeGenerationSupporter.Server); 
                    break;
                case EventNotificationTarget.Database:
                    GenerateKeyword(TSqlTokenType.Database); 
                    break;
                case EventNotificationTarget.Queue:
                    GenerateIdentifier(CodeGenerationSupporter.Queue);
                    GenerateSpaceAndFragmentIfNotNull(node.QueueName);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }
    }
}
