//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SetFipsFlaggerCommand.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SetFipsFlaggerCommand node)
        {
            GenerateIdentifier(CodeGenerationSupporter.FipsFlagger);
            GenerateSpace();
            FipsComplianceLevelHelper.Instance.GenerateSourceForOption(_writer, node.ComplianceLevel);
        }
    }
}
