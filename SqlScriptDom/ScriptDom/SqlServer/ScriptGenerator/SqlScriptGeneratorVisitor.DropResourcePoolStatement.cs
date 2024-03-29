//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropResourcePoolStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropResourcePoolStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Resource);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Pool); 
            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
