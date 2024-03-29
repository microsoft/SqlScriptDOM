//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropExternalFileFormatStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropExternalFileFormatStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.External);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.File);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Format);

            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
