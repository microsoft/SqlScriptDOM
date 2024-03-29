//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropSecurityPolicyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropSecurityPolicyStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Security);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Policy);

            if(node.IsIfExists)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.If);
                GenerateSpaceAndKeyword(TSqlTokenType.Exists);
            }
            
            GenerateSpace();
            GenerateCommaSeparatedList(node.Objects);
        }
    }
}
