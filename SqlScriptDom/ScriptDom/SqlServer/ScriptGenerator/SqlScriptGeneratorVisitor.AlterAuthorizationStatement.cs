//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterAuthorizationStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterAuthorizationStatement node)
        {
            GenerateSpaceSeparatedTokens(TSqlTokenType.Alter, TSqlTokenType.Authorization);

            NewLineAndIndent();
            GenerateFragmentIfNotNull(node.SecurityTargetObject);

            NewLineAndIndent();
            GenerateKeywordAndSpace(TSqlTokenType.To); 

            if (node.ToSchemaOwner)
            {
                GenerateSpaceSeparatedTokens(TSqlTokenType.Schema, CodeGenerationSupporter.Owner); 
            }
            else
            {
                GenerateFragmentIfNotNull(node.PrincipalName);
            }
        }
    }
}
