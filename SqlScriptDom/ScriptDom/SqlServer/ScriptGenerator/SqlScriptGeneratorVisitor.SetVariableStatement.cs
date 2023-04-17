//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SetVariableStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SetVariableStatement node)
        {
            GenerateKeyword(TSqlTokenType.Set);

            GenerateSpaceAndFragmentIfNotNull(node.Variable);

            if (node.SeparatorType != SeparatorType.NotSpecified)
            {
                switch (node.SeparatorType)
                {
                    case SeparatorType.NotSpecified:
                        break;
                    case SeparatorType.Dot:
                        GenerateSymbol(TSqlTokenType.Dot); 
                        break;
                    case SeparatorType.DoubleColon:
                        GenerateSymbol(TSqlTokenType.DoubleColon); 
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                        break;
                }

                GenerateFragmentIfNotNull(node.Identifier);

                if (node.FunctionCallExists)
                {
                    GenerateSpace();
                    GenerateSymbol(TSqlTokenType.LeftParenthesis);
                    GenerateCommaSeparatedList(node.Parameters);
                    GenerateSymbol(TSqlTokenType.RightParenthesis);
                }
            }

            if (node.Expression != null)
            {
                TSqlTokenType symbol = GetValueForEnumKey(_assignmentKindSymbols, node.AssignmentKind);
                GenerateSpaceAndSymbol(symbol);
                GenerateSpaceAndFragmentIfNotNull(node.Expression);
            }

            if (node.CursorDefinition != null)
            {
                GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                GenerateSpaceAndFragmentIfNotNull(node.CursorDefinition);
            }
        }
    }
}
