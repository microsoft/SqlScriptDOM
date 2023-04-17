//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.WindowsCreateLoginSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(WindowsCreateLoginSource node)
        {
            GenerateKeyword(TSqlTokenType.From);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Windows);

            if (node.Options != null && node.Options.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpace();

                GenerateCommaSeparatedList(node.Options);
            }
        }
    }
}
