//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropWorkloadGroupStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropWorkloadGroupStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Workload);
            GenerateSpaceAndKeyword(TSqlTokenType.Group);
            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
