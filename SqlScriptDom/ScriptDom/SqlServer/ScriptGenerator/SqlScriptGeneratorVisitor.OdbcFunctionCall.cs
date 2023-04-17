//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OdbcFunctionCall.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OdbcFunctionCall node)
        {
            GenerateSymbol(TSqlTokenType.LeftCurly);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Fn);
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            if (node.ParametersUsed)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Parameters, true);
            }

            GenerateSpaceAndSymbol(TSqlTokenType.RightCurly); 
        }
    }
}
