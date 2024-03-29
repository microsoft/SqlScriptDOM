//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropMessageTypeStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropMessageTypeStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Message);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Type);

            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
