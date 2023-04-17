//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AutoCreateStatisticsDatabaseOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AutoCreateStatisticsDatabaseOption node)
        {
            OnOffSimpleDbOptionsHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateSpace();
            GenerateOptionStateOnOff(node.OptionState);
            // Do more stuff here.
            if (node.HasIncremental)
            {
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateIdentifier(CodeGenerationSupporter.Incremental);
                GenerateSpace();
                GenerateKeywordAndSpace(TSqlTokenType.EqualsSign);
                GenerateOptionStateOnOff(node.IncrementalState);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
        }
    }
}
