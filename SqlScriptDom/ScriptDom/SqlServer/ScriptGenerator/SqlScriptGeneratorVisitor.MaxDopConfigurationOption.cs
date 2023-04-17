//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.MaxDopConfigurationOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(MaxDopConfigurationOption node)
        {
            DatabaseConfigSetOptionKindHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateSpace();
            GenerateKeywordAndSpace(TSqlTokenType.EqualsSign);

            if (node.Primary)
            {
                GenerateKeyword(TSqlTokenType.Primary); 
            }
            else
            {
                GenerateFragmentIfNotNull(node.Value);
            }
        }
    }
}
