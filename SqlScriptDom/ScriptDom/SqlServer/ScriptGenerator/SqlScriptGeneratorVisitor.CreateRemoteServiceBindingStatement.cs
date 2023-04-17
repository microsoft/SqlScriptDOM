//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateRemoteServiceBindingStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateRemoteServiceBindingStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Remote);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Service);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Binding);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // owner
            GenerateOwnerIfNotNull(node.Owner);

            // TO SERVICE
            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.To);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Service);
            GenerateSpaceAndFragmentIfNotNull(node.Service);

            GenerateBindingOptions(node.Options);
        }
    }
}
