//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateSecurityPolicyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateSecurityPolicyStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Security);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Policy);

            GenerateSecurityPolicyStatementBody(node);
        }
    }
}
