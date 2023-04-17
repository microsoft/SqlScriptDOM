//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.WriteTextStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(WriteTextStatement node)
        {
            GenerateKeyword(TSqlTokenType.WriteText);

            GenerateSpace();
            GenerateBulkColumnTimestamp(node);

            if (node.WithLog)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Log);
            }

            GenerateSpaceAndFragmentIfNotNull(node.SourceParameter);
        }
    }
}
