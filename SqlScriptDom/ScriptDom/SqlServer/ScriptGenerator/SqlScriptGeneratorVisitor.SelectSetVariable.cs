//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SelectSetVariable.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SelectSetVariable node)
        {
            GenerateFragmentIfNotNull(node.Variable);

            TSqlTokenType symbol = GetValueForEnumKey(_assignmentKindSymbols, node.AssignmentKind);
            GenerateSpaceAndSymbol(symbol);

            GenerateSpaceAndFragmentIfNotNull(node.Expression);
        }
    }
}
