//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropSignatureStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropSignatureStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Drop); 
            GenerateCounterSignature(node);

            GenerateSpaceAndKeyword(TSqlTokenType.From);
            GenerateSpace();
            GenerateModule(node);

            NewLineAndIndent();
            GenerateCryptos(node);
        }
    }
}
