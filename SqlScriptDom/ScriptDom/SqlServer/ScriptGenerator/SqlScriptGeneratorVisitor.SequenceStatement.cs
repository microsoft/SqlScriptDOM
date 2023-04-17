//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SequenceStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public void GenerateSequenceStatementBody(SequenceStatement node)
        {
            GenerateSpaceAndFragmentIfNotNull(node.Name);
            foreach (SequenceOption item in node.SequenceOptions)
            {
                GenerateFragmentIfNotNull(item);
            }
        }

        public override void ExplicitVisit(DataTypeSequenceOption node)
        {
            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.As);
            GenerateSpaceAndFragmentIfNotNull(node.DataType);
        }

        public override void ExplicitVisit(SequenceOption node)
        {
            NewLineAndIndent();
            if (node.NoValue)
            {
                GenerateIdentifier(CodeGenerationSupporter.No);
                GenerateSpace();
            }
            switch (node.OptionKind)
            {
                case SequenceOptionKind.MinValue:
                    GenerateIdentifier(CodeGenerationSupporter.MinValue);
                    break;
                case SequenceOptionKind.MaxValue:
                    GenerateIdentifier(CodeGenerationSupporter.MaxValue);
                    break;
                case SequenceOptionKind.Cache:
                    GenerateIdentifier(CodeGenerationSupporter.Cache);
                    break;
                case SequenceOptionKind.Cycle:
                    GenerateIdentifier(CodeGenerationSupporter.Cycle);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false,"unexpected option encountered");
                    break;
            }
        }

        public override void ExplicitVisit(ScalarExpressionSequenceOption node)
        {
            NewLineAndIndent();
            switch (node.OptionKind)
            {
                case SequenceOptionKind.MinValue:
                    GenerateIdentifier(CodeGenerationSupporter.MinValue);
                    break;
                case SequenceOptionKind.MaxValue:
                    GenerateIdentifier(CodeGenerationSupporter.MaxValue);
                    break;
                case SequenceOptionKind.Cache:
                    GenerateIdentifier(CodeGenerationSupporter.Cache);
                    break;
                case SequenceOptionKind.Increment:
                    GenerateIdentifier(CodeGenerationSupporter.Increment);
                    GenerateSpaceAndKeyword(TSqlTokenType.By);
                    break;
                case SequenceOptionKind.Start:
                    GenerateIdentifier(CodeGenerationSupporter.Start);
                    GenerateSpaceAndKeyword(TSqlTokenType.With);
                    break;
                case SequenceOptionKind.Restart:
                    GenerateIdentifier(CodeGenerationSupporter.Restart);
                    if (node.OptionValue != null)
                    {
                        GenerateSpaceAndKeyword(TSqlTokenType.With);
                    }
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "unexpected option encountered");
                    break;
            }
            GenerateSpaceAndFragmentIfNotNull(node.OptionValue);
        }
    }
}
