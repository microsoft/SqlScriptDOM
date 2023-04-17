//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AssemblyName.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AssemblyName node)
        {
            GenerateFragmentIfNotNull(node.Name);
            if (node.ClassName != null)
            {
                GenerateSymbol(TSqlTokenType.Dot);
                GenerateFragmentIfNotNull(node.ClassName);
            }
        }
    }
}
