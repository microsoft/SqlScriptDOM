//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.GenericConfigurationOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(GenericConfigurationOption node)
        {
            GenerateIdentifier(node.GenericOptionKind.Value);
            GenerateSpace();
            GenerateKeywordAndSpace(TSqlTokenType.EqualsSign);

            ScalarExpression valExp = node.GenericOptionState.ScalarExpression;

            if (valExp != null && (valExp is StringLiteral || valExp is IntegerLiteral || valExp is UnaryExpression))
            {
                GenerateFragmentIfNotNull(node.GenericOptionState);
            }
            else
            {
                GenerateIdentifier(node.GenericOptionState.Identifier.Value);
            }
        }
    }
}