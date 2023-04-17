//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.MethodSpecifier.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(MethodSpecifier node)
        {
            GenerateKeyword(TSqlTokenType.External); 
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Name);

            GenerateSpaceAndFragmentIfNotNull(node.AssemblyName);
            GenerateSymbol(TSqlTokenType.Dot);
            GenerateFragmentIfNotNull(node.ClassName);
            GenerateSymbol(TSqlTokenType.Dot);
            GenerateFragmentIfNotNull(node.MethodName);
        }
    }
}
