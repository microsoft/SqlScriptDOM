//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AddSignatureStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AddSignatureStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Add);
            GenerateCounterSignature(node);
            GenerateSpaceAndKeyword(TSqlTokenType.To);
            GenerateSpace();

            GenerateModule(node);

            NewLineAndIndent();
            GenerateCryptos(node);
        }
    }
}
