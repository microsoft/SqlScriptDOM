//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ApplicationRoleStatementBase.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateApplicationRoleStatementBase(ApplicationRoleStatement node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Application);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Role);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.With); 
            GenerateSpace();

            GenerateCommaSeparatedList(node.ApplicationRoleOptions);
        }
    }
}
