//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CursorId.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CursorId node)
        {
            if (node.IsGlobal)
            {
                GenerateIdentifier(CodeGenerationSupporter.Global);
                GenerateSpace();
            }

            // name
            GenerateFragmentIfNotNull(node.Name);
        }
    }
}
