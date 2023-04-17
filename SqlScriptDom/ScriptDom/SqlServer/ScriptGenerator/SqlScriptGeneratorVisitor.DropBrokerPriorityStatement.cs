//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropBrokerPriorityStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropBrokerPriorityStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Broker);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Priority);
            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
