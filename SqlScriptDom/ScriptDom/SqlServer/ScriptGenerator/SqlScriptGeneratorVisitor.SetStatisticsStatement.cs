//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SetStatisticsStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SetStatisticsStatement node)
        {
            GenerateKeyword(TSqlTokenType.Set);
            GenerateSpaceAndKeyword(TSqlTokenType.Statistics);
            GenerateSpace();
            SetStatisticsOptionsHelper.Instance.GenerateCommaSeparatedFlagOptions(_writer, node.Options);

            GenerateSpace();
            GenerateKeyword(node.IsOn ? TSqlTokenType.On : TSqlTokenType.Off); 
        }
    }
}
