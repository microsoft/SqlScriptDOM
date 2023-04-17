//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BulkInsertBase.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateOption(BulkInsertBase node)
        {
            if (node.Options.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeywordAndSpace(TSqlTokenType.With);
                GenerateParenthesisedCommaSeparatedList(node.Options);
            }
        }
    }
}
