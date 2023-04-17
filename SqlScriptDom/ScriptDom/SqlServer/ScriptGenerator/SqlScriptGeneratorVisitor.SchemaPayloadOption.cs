//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SchemaPayloadOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        // SCHEMA = { NONE | STANDARD } 
        public override void ExplicitVisit(SchemaPayloadOption node)
        {
            GenerateNameEqualsValue(
                    TSqlTokenType.Schema,
                    node.IsStandard ? CodeGenerationSupporter.Standard : CodeGenerationSupporter.None);
        }
    }
}
