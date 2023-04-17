//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateServerRoleStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateServerRoleStatement node)
        {
            GenerateSpaceSeparatedTokens(TSqlTokenType.Create,
                CodeGenerationSupporter.Server,
                CodeGenerationSupporter.Role);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // owner
            GenerateOwnerIfNotNull(node.Owner);
        }
    }
}
