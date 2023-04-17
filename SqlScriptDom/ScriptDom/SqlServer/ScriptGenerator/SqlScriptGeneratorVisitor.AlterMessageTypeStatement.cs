//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterMessageTypeStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterMessageTypeStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Message);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Type);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateValidationMethod(node);
        }
    }
}
