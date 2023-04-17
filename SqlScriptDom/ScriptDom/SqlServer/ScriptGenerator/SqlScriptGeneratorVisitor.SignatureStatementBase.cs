//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SignatureStatementBase.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        // generate SIGNATURE with an optional COUNTER
        protected void GenerateCounterSignature(SignatureStatementBase node)
        {
            if (node.IsCounter)
            {
                GenerateIdentifier(CodeGenerationSupporter.Counter);
                GenerateSpace();
            }

            GenerateIdentifier(CodeGenerationSupporter.Signature);
        }

        // generate signature module
        protected void GenerateModule(SignatureStatementBase node)
        {
            switch (node.ElementKind)
            {
                case SignableElementKind.NotSpecified:
                    break;
                case SignableElementKind.Object:
                    GenerateIdentifier(CodeGenerationSupporter.Object);
                    GenerateSymbol(TSqlTokenType.DoubleColon);
                    break;
                case SignableElementKind.Assembly:
                    GenerateIdentifier(CodeGenerationSupporter.Assembly);
                    GenerateSymbol(TSqlTokenType.DoubleColon);
                    break;
                case SignableElementKind.Database:
                    GenerateKeyword(TSqlTokenType.Database);
                    GenerateSymbol(TSqlTokenType.DoubleColon);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }

            GenerateFragmentIfNotNull(node.Element);
        }

        // generate cryptos
        protected void GenerateCryptos(SignatureStatementBase node)
        {
            GenerateKeyword(TSqlTokenType.By);
            GenerateSpace();

            GenerateCommaSeparatedList(node.Cryptos);
        }
    }
}
