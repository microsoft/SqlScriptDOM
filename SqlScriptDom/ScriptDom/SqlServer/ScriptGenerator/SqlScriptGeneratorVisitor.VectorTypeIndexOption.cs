//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.VectorTypeIndexOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(VectorTypeIndexOption node)
        {
            IndexOptionHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            GenerateSymbol(TSqlTokenType.SingleQuote);
            VectorIndexTypeHelper.Instance.GenerateSourceForOption(_writer, node.VectorType);
            GenerateSymbol(TSqlTokenType.SingleQuote);
        }
    }
}