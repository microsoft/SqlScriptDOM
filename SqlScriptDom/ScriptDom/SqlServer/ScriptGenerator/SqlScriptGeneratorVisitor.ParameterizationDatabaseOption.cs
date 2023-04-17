//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ParameterizationDatabaseOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ParameterizationDatabaseOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == DatabaseOptionKind.Parameterization);
            GenerateIdentifier(CodeGenerationSupporter.Parameterization);
            GenerateSpaceAndIdentifier(node.IsSimple ? CodeGenerationSupporter.Simple : CodeGenerationSupporter.Forced);
        }
    }
}
