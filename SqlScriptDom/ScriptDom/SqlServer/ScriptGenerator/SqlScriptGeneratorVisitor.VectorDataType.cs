//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.VectorDataType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(VectorDataTypeReference node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Vector);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.Dimension);
            if (node.BaseType != null)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpaceAndFragmentIfNotNull(node.BaseType);
            }
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
