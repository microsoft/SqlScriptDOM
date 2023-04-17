//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TSqlScript.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(TSqlScript node)
        {
            Boolean firstItem = true;
            foreach (var item in node.Batches)
            {
                if (firstItem)
                {
                    firstItem = false;
                }
                else
                {
                    NewLine();
                    GenerateKeyword(TSqlTokenType.Go);
                    NewLine();
                }

                GenerateFragmentIfNotNull(item);
            }
        }
    }
}
