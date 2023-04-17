//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateTypeUddtStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateTypeUddtStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Type);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // FROM
            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.From);

            GenerateSpaceAndFragmentIfNotNull(node.DataType);

            GenerateSpaceAndFragmentIfNotNull(node.NullableConstraint);

        }
    }
}
