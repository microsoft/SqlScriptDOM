//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.PasswordCreateLoginSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(PasswordCreateLoginSource node)
        {
            GenerateKeyword(TSqlTokenType.With);

            GenerateSpace();
            GenerateNameEqualsValue(CodeGenerationSupporter.Password, node.Password);

            if (node.Hashed)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Hashed);
            }

            if (node.MustChange)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.MustChange);
            }

            if (node.Options != null && node.Options.Count > 0)
            {
                GenerateSymbolAndSpace(TSqlTokenType.Comma);

                GenerateFragmentList(node.Options);
            }
        }
    }
}
