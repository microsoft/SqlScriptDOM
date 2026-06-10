//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateExternalFunctionStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateExternalFunctionStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateExternalFunctionStatementBody(node);
        }

        public override void ExplicitVisit(AlterExternalFunctionStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateExternalFunctionStatementBody(node);
        }

        public override void ExplicitVisit(CreateOrAlterExternalFunctionStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndKeyword(TSqlTokenType.Or);
            GenerateSpaceAndKeyword(TSqlTokenType.Alter);
            GenerateExternalFunctionStatementBody(node);
        }

        private void GenerateExternalFunctionStatementBody(ExternalFunctionStatement node)
        {
            GenerateSpaceAndKeyword(TSqlTokenType.Function);
            GenerateSpaceAndFragmentIfNotNull(node.Name);
            if (node.Parameters != null && node.Parameters.Count > 0)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Parameters);
            }
            if (node.ReturnType != null)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Returns);
                GenerateSpaceAndFragmentIfNotNull(node.ReturnType);
            }
            GenerateSpaceAndKeyword(TSqlTokenType.As);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.External);
            GenerateSpaceAndKeyword(TSqlTokenType.Function);
            GenerateSpaceAndFragmentIfNotNull(node.ExternalName);
        }
    }
}
