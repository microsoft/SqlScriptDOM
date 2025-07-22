//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterDbPerformCutoverStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterDatabasePerformCutoverStatement node)
        {
            GenerateAlterDbStatementHead(node);

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.PerformCutover);
        }
    }
}
