//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OnOffDatabaseOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OnOffDatabaseOption node)
        {
            OnOffSimpleDbOptionsHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateSpace();
            if (OnOffSimpleDbOptionsHelper.Instance.RequiresEqualsSign(node.OptionKind))
            {
                GenerateKeywordAndSpace(TSqlTokenType.EqualsSign);
            }
            GenerateOptionStateOnOff(node.OptionState);
        }
    }
}
