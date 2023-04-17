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
        public override void ExplicitVisit(OnOffPrimaryConfigurationOption node)
        {
            DatabaseConfigSetOptionKindHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateSpace();
            GenerateKeywordAndSpace(TSqlTokenType.EqualsSign);

            if (node.OptionState == DatabaseConfigurationOptionState.Primary)
            {
                GenerateIdentifier(CodeGenerationSupporter.Primary);
            }
            else
            {
                GenerateDatabaseConfigurationOptionStateOnOff(node.OptionState);
            }
            
        }
    }
}
