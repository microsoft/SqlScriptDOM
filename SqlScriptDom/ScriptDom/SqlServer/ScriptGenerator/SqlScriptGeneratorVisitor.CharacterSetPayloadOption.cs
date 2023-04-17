//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CharacterSetPayloadOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CharacterSetPayloadOption node)
        {
            GenerateNameEqualsValue(
                CodeGenerationSupporter.CharacterSet,
                node.IsSql ? CodeGenerationSupporter.Sql : CodeGenerationSupporter.Xml);
        }
    }
}
