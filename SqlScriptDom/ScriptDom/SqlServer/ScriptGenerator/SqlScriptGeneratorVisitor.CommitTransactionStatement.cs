//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CommitTransactionStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CommitTransactionStatement node)
        {
            GenerateKeyword(TSqlTokenType.Commit);
            GenerateSpaceAndKeyword(TSqlTokenType.Transaction);

            // name
            // TODO, yangg: why node.Name is of type Object? rather TSqlFragment?
            if (node.Name != null)
            {
                GenerateSpace();
                GenerateTransactionName(node.Name);
            }

            if (node.DelayedDurabilityOption != OptionState.NotSet)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateIdentifier(CodeGenerationSupporter.DelayedDurability);
                GenerateSpace();
                GenerateSymbol(TSqlTokenType.EqualsSign);
                GenerateSpace();
                GenerateOptionStateOnOff(node.DelayedDurabilityOption);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
        }
    }
}
