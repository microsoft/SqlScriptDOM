//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AuditActionSpecification.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AuditActionSpecification node)
        {
            GenerateCommaSeparatedList(node.Actions);
            GenerateSpaceAndFragmentIfNotNull(node.TargetObject);
            GenerateSpaceAndKeyword(TSqlTokenType.By);
            GenerateSpace();
            GenerateCommaSeparatedList(node.Principals);
        }
    }
}
