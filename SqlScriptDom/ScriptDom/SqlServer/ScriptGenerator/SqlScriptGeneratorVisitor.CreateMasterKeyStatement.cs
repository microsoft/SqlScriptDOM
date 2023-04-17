//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateMasterKeyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateMasterKeyStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Master);
            GenerateSpaceAndKeyword(TSqlTokenType.Key);

			if (node.Password != null)
			{
				GenerateSpaceAndIdentifier(CodeGenerationSupporter.Encryption);
				GenerateSpaceAndKeyword(TSqlTokenType.By);
				GenerateSpaceAndIdentifier(CodeGenerationSupporter.Password);
				GenerateSymbol(TSqlTokenType.EqualsSign);
				GenerateSpaceAndFragmentIfNotNull(node.Password);
			}
        }
    }
}
