//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.RolePayloadOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<DatabaseMirroringEndpointRole, TokenGenerator> _databaseMirroringEndpointRoleGenerators = new Dictionary<DatabaseMirroringEndpointRole, TokenGenerator>()
        {
            {DatabaseMirroringEndpointRole.NotSpecified, new EmptyGenerator()},
            {DatabaseMirroringEndpointRole.All, new KeywordGenerator(TSqlTokenType.All)},
            {DatabaseMirroringEndpointRole.Partner, new IdentifierGenerator(CodeGenerationSupporter.Partner)},
            {DatabaseMirroringEndpointRole.Witness, new IdentifierGenerator(CodeGenerationSupporter.Witness)},
        };

        // ROLE = { WITNESS | PARTNER | ALL }
        public override void ExplicitVisit(RolePayloadOption node)
        {
            GenerateTokenAndEqualSign(CodeGenerationSupporter.Role);

            TokenGenerator generator = GetValueForEnumKey(_databaseMirroringEndpointRoleGenerators, node.Role);
            if (generator != null)
            {
                GenerateSpace();
                GenerateToken(generator);
            }
        }
    }
}
