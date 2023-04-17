//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropAvailabilityGroupStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropAvailabilityGroupStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Drop);
            GenerateIdentifier(CodeGenerationSupporter.Availability);
            GenerateSpaceAndKeyword(TSqlTokenType.Group);
            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
