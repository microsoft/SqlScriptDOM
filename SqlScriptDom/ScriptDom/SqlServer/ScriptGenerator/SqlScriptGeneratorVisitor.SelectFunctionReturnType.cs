//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SelectFunctionReturnType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SelectFunctionReturnType node)
        {
            if (node.SelectStatement != null)
            {
                NewLineAndIndent();
                GenerateFragmentIfNotNull(node.SelectStatement);
                NewLine();
            }
        }
    }
}
