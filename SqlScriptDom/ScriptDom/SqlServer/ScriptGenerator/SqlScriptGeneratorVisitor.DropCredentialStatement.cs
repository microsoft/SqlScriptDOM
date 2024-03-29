//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropCredentialStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropCredentialStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);

            if (node.IsDatabaseScoped)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Database);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Scoped);
            }

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Credential);

            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
