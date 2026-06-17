//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.InvokeExternalApiFunction.cs" company="Microsoft">
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
        /// Emits an INVOKE_EXTERNAL_API function call like
        /// INVOKE_EXTERNAL_API('FunctionSetName', 'FunctionName' [, arg1 [, arg2 ...]]).
        /// FunctionSetName and FunctionName are required string literals; remaining
        /// arguments are optional scalar expressions.
        /// </summary>
        /// <param name="node">Expression node to generate</param>
        public override void ExplicitVisit(InvokeExternalApiFunctionCall node)
        {
            if (node.FunctionSetName == null)
            {
                throw new InvalidOperationException("InvokeExternalApiFunctionCall.FunctionSetName is required.");
            }

            if (node.FunctionName == null)
            {
                throw new InvalidOperationException("InvokeExternalApiFunctionCall.FunctionName is required.");
            }

            GenerateIdentifier(CodeGenerationSupporter.InvokeExternalApi);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            GenerateFragmentIfNotNull(node.FunctionSetName);

            GenerateSymbol(TSqlTokenType.Comma);
            GenerateSpace();
            GenerateFragmentIfNotNull(node.FunctionName);

            if (node.Arguments != null)
            {
                for (int i = 0; i < node.Arguments.Count; i++)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                    GenerateSpace();
                    GenerateFragmentIfNotNull(node.Arguments[i]);
                }
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
