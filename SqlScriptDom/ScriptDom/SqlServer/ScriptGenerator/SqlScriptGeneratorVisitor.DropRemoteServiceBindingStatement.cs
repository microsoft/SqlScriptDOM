//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropRemoteServiceBindingStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropRemoteServiceBindingStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Remote);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Service);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Binding);

            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
