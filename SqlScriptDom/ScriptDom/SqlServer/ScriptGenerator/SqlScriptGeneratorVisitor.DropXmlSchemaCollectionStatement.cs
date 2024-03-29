//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropXmlSchemaCollectionStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropXmlSchemaCollectionStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Xml);
            GenerateSpaceAndKeyword(TSqlTokenType.Schema);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Collection);

            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
