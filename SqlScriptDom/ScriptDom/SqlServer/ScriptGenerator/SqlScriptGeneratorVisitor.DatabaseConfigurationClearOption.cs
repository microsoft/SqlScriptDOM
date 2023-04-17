//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DatabaseConfigurationClearOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DatabaseConfigurationClearOption node)
        {
            DatabaseConfigClearOptionKindHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);

            GenerateSpaceAndFragmentIfNotNull(node.PlanHandle);
        }
    }
}

