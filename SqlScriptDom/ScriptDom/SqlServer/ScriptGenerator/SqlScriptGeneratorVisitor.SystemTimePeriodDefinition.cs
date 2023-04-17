//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SystemTimePeriodDefinition.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SystemTimePeriodDefinition node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Period);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.For);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.SystemTime);

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);

            GenerateFragmentIfNotNull(node.StartTimeColumn);
            GenerateSymbol(TSqlTokenType.Comma);
            GenerateSpace();
            GenerateFragmentIfNotNull(node.EndTimeColumn);

            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}
