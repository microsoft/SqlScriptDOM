//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropEventNotificationStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropEventNotificationStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Event); 
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Notification); 

            GenerateSpace();
            GenerateCommaSeparatedList(node.Notifications);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.On); 
            GenerateSpaceAndFragmentIfNotNull(node.Scope);
        }
    }
}
