//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropFulltextIndexStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropFullTextIndexStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Fulltext);
            GenerateSpaceAndKeyword(TSqlTokenType.Index);
            GenerateSpaceAndKeyword(TSqlTokenType.On);

            GenerateSpaceAndFragmentIfNotNull(node.TableName);
        }
    }
}
