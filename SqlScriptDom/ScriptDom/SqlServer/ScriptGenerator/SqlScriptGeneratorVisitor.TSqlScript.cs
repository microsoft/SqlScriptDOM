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
            // Initialize token stream for comment preservation
            if (_options.PreserveComments && node.ScriptTokenStream != null)
            {
                SetTokenStreamForComments(node.ScriptTokenStream);
            }

            // Emit leading comments before the script
            BeforeVisitFragment(node);

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

            // Emit trailing comments after the script
            AfterVisitFragment(node);
        }
    }
}
