//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropSchemaStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropSchemaStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndKeyword(TSqlTokenType.Schema);

            if (node.IsIfExists)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.If);
                GenerateSpaceAndKeyword(TSqlTokenType.Exists);
            }

            GenerateSpaceAndFragmentIfNotNull(node.Schema);

            if (node.DropBehavior == DropSchemaBehavior.Cascade)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Cascade);
            }
            else if (node.DropBehavior == DropSchemaBehavior.Restrict)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Restrict);
            }
        }
    }
}
