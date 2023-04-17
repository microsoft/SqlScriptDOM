//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateEventNotificationStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateEventNotificationStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Event);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Notification); 

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.On);

            if (node.Scope != null)
            {
                switch (node.Scope.Target)
                {
                    case EventNotificationTarget.Server:
                        GenerateSpaceAndIdentifier(CodeGenerationSupporter.Server);
                        break;
                    case EventNotificationTarget.Database:
                        GenerateSpaceAndKeyword(TSqlTokenType.Database);
                        break;
                    case EventNotificationTarget.Queue:
                        GenerateSpaceAndIdentifier(CodeGenerationSupporter.Queue);
                        GenerateSpaceAndFragmentIfNotNull(node.Scope.QueueName);
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                        break;
                }
            }

            if (node.WithFanIn)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.FanIn); 
            }

            if (node.EventTypeGroups != null && node.EventTypeGroups.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.For);

                GenerateSpace();
                GenerateFragmentList(node.EventTypeGroups);
            }

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.To);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Service);
            GenerateSpaceAndFragmentIfNotNull(node.BrokerService);
            GenerateSymbol(TSqlTokenType.Comma);  
            GenerateSpaceAndFragmentIfNotNull(node.BrokerInstanceSpecifier);
        }
    }
}
