//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterRoleStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterRoleStatement node)
        {
            GenerateSpaceSeparatedTokens(
                TSqlTokenType.Alter, 
                CodeGenerationSupporter.Role);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);
            
            GenerateSpaceAndFragmentIfNotNull(node.Action);
        }
    }
}
