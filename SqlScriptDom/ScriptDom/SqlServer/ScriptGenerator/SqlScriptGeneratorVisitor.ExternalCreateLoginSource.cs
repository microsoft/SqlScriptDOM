//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ExternalCreateLoginSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ExternalCreateLoginSource node)
        {
            GenerateKeyword(TSqlTokenType.From);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.External);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Provider);

            if (node.Options != null && node.Options.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpace();

                GenerateCommaSeparatedList(node.Options);
            }
        }
    }
}