//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateRouteStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateRoleStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Role);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // owner
            GenerateOwnerIfNotNull(node.Owner);
        }
    }
}
