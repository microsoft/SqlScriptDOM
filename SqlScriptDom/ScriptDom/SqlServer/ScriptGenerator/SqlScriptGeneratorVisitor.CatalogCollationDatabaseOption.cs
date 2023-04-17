//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CatalogCollationOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CatalogCollationOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == DatabaseOptionKind.CatalogCollation);

            if (node.CatalogCollation.HasValue)
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.CatalogCollation, node.CatalogCollation.Value.ToString());
            }
        }
    }
}
