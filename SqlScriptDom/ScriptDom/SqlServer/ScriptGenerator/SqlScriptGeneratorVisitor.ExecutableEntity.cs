//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ExecutableEntity.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateParameters(ExecutableEntity node)
        {
            GenerateCommaSeparatedList(node.Parameters);
        }
    }
}
