//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SetTextsizeStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SetTextSizeStatement node)
        {
            GenerateKeyword(TSqlTokenType.Set);
            GenerateSpaceAndKeyword(TSqlTokenType.TextSize);
            GenerateSpaceAndFragmentIfNotNull(node.TextSize);
        }
    }
}
