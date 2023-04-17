//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterRouteStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterRouteStatement node)
        {
            GenerateSpaceSeparatedTokens(TSqlTokenType.Alter, CodeGenerationSupporter.Route);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateRouteOptions(node);
        }
    }
}
