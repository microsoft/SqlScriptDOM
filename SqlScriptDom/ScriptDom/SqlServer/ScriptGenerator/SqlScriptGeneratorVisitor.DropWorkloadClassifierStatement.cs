//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropWorkloadClassifierStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropWorkloadClassifierStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Workload);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Classifier);
            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
