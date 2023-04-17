//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ExecuteAsStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ExecuteAsStatement node)
        {
            GenerateKeyword(TSqlTokenType.Execute);

            GenerateSpaceAndFragmentIfNotNull(node.ExecuteContext);

            if (node.WithNoRevert)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.No);
                GenerateSpaceAndKeyword(TSqlTokenType.Revert); 
            }

            else if (node.Cookie != null)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Cookie);
                GenerateSpaceAndKeyword(TSqlTokenType.Into);
                GenerateSpaceAndFragmentIfNotNull(node.Cookie);
            }
        }
    }
}
