//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateTypeUdtStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateTypeUdtStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Type);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // EXTERNAL NAME
            NewLineAndIndent();
            GenerateSpaceAndKeyword(TSqlTokenType.External);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Name);

            GenerateSpaceAndFragmentIfNotNull(node.AssemblyName);
        }
    }
}
