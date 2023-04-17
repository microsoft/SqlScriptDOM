//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateSynonymStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateSynonymStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Synonym);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // FOR
            GenerateSpaceAndKeyword(TSqlTokenType.For);
            GenerateSpaceAndFragmentIfNotNull(node.ForName);
        }
    }
}
