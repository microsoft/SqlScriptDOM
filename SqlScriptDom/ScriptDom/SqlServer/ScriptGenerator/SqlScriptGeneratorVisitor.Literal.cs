//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.Literal.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        /// <summary>
        /// Visitor for IntegerLiteral
        /// </summary>
        /// <param name="node">The node.</param>
        public override void ExplicitVisit(IntegerLiteral node)
        {
            GenerateToken(TSqlTokenType.Integer, node.Value);
            GenerateSpaceAndCollation(node.Collation);
        }

        /// <summary>
        /// Visitor for NumericLiteral
        /// </summary>
        /// <param name="node">The node.</param>
        public override void ExplicitVisit(NumericLiteral node)
        {
            GenerateToken(TSqlTokenType.Numeric, node.Value);
            GenerateSpaceAndCollation(node.Collation);
        }

        /// <summary>
        /// Visitor for StringLiteral
        /// </summary>
        /// <param name="node">The node.</param>
        public override void ExplicitVisit(StringLiteral node)
        {
            if (!node.IsNational)
            {
                string encodedValue = "'" + node.Value.Replace("'", "''") + "'";
                GenerateToken(TSqlTokenType.AsciiStringLiteral, encodedValue);
            }
            else
            {
                string encodedValue = "N'" + node.Value.Replace("'", "''") + "'";
                GenerateToken(TSqlTokenType.UnicodeStringLiteral, encodedValue);
            }
            GenerateSpaceAndCollation(node.Collation);
        }

        /// <summary>
        /// Visitor for BinaryLiteral
        /// </summary>
        /// <param name="node">The node.</param>
        public override void ExplicitVisit(BinaryLiteral node)
        {
            GenerateToken(TSqlTokenType.HexLiteral, node.Value);
            GenerateSpaceAndCollation(node.Collation);
        }

        /// <summary>
        /// Visitor for DefaultLiteral
        /// </summary>
        /// <param name="node">The node.</param>
        public override void ExplicitVisit(DefaultLiteral node)
        {
            GenerateKeyword(TSqlTokenType.Default);
            GenerateSpaceAndCollation(node.Collation);
        }

        /// <summary>
        /// Visitor for MaxLiteral
        /// </summary>
        /// <param name="node">The node.</param>
        public override void ExplicitVisit(MaxLiteral node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Max);
            GenerateSpaceAndCollation(node.Collation);
        }

        /// <summary>
        /// Visitor for MoneyLiteral
        /// </summary>
        /// <param name="node">The node.</param>
        public override void ExplicitVisit(MoneyLiteral node)
        {
            GenerateToken(TSqlTokenType.Money, node.Value);
            GenerateSpaceAndCollation(node.Collation);
        }

        /// <summary>
        /// Visitor for NullLiteral
        /// </summary>
        /// <param name="node">The node.</param>
        public override void ExplicitVisit(NullLiteral node)
        {
            GenerateKeyword(TSqlTokenType.Null);
            GenerateSpaceAndCollation(node.Collation);
        }

        /// <summary>
        /// Visitor for RealLiteral
        /// </summary>
        /// <param name="node">The node.</param>
        public override void ExplicitVisit(RealLiteral node)
        {
            GenerateToken(TSqlTokenType.Real, node.Value);
            GenerateSpaceAndCollation(node.Collation);
        }

        /// <summary>
        /// Visitor for IdentifierLiteral
        /// </summary>
        /// <param name="node">The node.</param>
        public override void ExplicitVisit(IdentifierLiteral node)
        {
            if (node.QuoteType == QuoteType.NotQuoted)
            {
                GenerateIdentifierWithoutCheck(node.Value);
            }
            else
            {
                GenerateQuotedIdentifier(node.Value, node.QuoteType);
            }
            GenerateSpaceAndCollation(node.Collation);
        }


        public override void ExplicitVisit(VariableReference node)
        {
            GenerateToken(TSqlTokenType.Variable, node.Name);
            GenerateSpaceAndCollation(node.Collation);
        }

        public override void ExplicitVisit(GlobalVariableExpression node)
        {
            GenerateToken(TSqlTokenType.Variable, node.Name);
            GenerateSpaceAndCollation(node.Collation);
        }

        /// <summary>
        /// Visitor for OdbcLiteral
        /// </summary>
        /// <param name="node">The node.</param>
        public override void ExplicitVisit(OdbcLiteral node)
        {
            GenerateOdbcLiteralPrefix(node);

            if (!node.IsNational)
            {
                string encodedValue = "'" + node.Value.Replace("'", "''") + "'";
                GenerateToken(TSqlTokenType.AsciiStringLiteral, encodedValue);
            }
            else
            {
                string encodedValue = "N'" + node.Value.Replace("'", "''") + "'";
                GenerateToken(TSqlTokenType.UnicodeStringLiteral, encodedValue);
            }
            GenerateSpaceAndSymbol(TSqlTokenType.RightCurly);
            GenerateSpaceAndCollation(node.Collation);
        }

        private void GenerateOdbcLiteralPrefix(OdbcLiteral node)
        {
            GenerateSymbolAndSpace(TSqlTokenType.LeftCurly);

            switch (node.OdbcLiteralType)
            {
                case OdbcLiteralType.Time:
                    GenerateIdentifier(CodeGenerationSupporter.T);
                    break;
                case OdbcLiteralType.Date:
                    GenerateIdentifier(CodeGenerationSupporter.D);
                    break;
                case OdbcLiteralType.Timestamp:
                    GenerateIdentifier(CodeGenerationSupporter.TS);
                    break;
                case OdbcLiteralType.Guid:
                    GenerateIdentifier(CodeGenerationSupporter.Guid);
                    break;
                default:
                    break;
            }

            GenerateSpace();
        }
    }
}
