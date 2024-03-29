//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropApplicationRoleStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropApplicationRoleStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Application);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Role); 
            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
