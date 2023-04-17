//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateXmlSchemaCollectionStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateXmlSchemaCollectionStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Xml);
            GenerateSpaceAndKeyword(TSqlTokenType.Schema);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Collection);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);
            
            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.As);
            GenerateSpaceAndFragmentIfNotNull(node.Expression);
        }
    }
}
