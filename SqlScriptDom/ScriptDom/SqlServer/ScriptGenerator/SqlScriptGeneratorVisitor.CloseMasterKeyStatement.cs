//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CloseMasterKeyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CloseMasterKeyStatement node)
        {
            GenerateKeyword(TSqlTokenType.Close);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Master);
            GenerateSpaceAndKeyword(TSqlTokenType.Key); 
        }
    }
}
