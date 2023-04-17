//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.LockEscalationTableOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(LockEscalationTableOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == TableOptionKind.LockEscalation, "TableOption does not match");
            GenerateIdentifier(CodeGenerationSupporter.LockEscalation);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            LockEscalationMethodHelper.Instance.GenerateSourceForOption(_writer, node.Value);
        }
    }
}