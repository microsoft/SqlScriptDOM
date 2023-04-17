//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropSensitivityClassification.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropSensitivityClassificationStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Sensitivity);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Classification);
            GenerateSpaceAndKeyword(TSqlTokenType.From);

            NewLineAndIndent();
            GenerateCommaSeparatedList(node.Columns);
        }
    }
}
