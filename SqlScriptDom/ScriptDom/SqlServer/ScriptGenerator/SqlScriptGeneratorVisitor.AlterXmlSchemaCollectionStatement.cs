//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterXmlSchemaCollectionStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterXmlSchemaCollectionStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Xml);
            GenerateSpaceAndKeyword(TSqlTokenType.Schema);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Collection);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateSpaceAndKeyword(TSqlTokenType.Add);
            GenerateSpaceAndFragmentIfNotNull(node.Expression);
        }
    }
}
