//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterRemoteServiceBindingStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterRemoteServiceBindingStatement node)
        {
            GenerateSpaceSeparatedTokens(
                TSqlTokenType.Alter, 
                CodeGenerationSupporter.Remote, 
                CodeGenerationSupporter.Service, 
                CodeGenerationSupporter.Binding);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateBindingOptions(node.Options);
        }
    }
}
