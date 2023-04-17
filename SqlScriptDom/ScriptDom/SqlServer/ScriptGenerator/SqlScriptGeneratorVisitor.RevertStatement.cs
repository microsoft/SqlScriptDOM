//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.RevertStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(RevertStatement node)
        {
            GenerateKeyword(TSqlTokenType.Revert); 
            if (node.Cookie != null)
            {
                NewLineAndIndent();
                GenerateKeywordAndSpace(TSqlTokenType.With);
                GenerateNameEqualsValue(CodeGenerationSupporter.Cookie, node.Cookie);
            }
        }
    }
}
