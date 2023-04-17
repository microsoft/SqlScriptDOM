//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ExecutableProcedureReference.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ExecutableProcedureReference node)
        {
            if (node.AdHocDataSource != null)
            {
                GenerateFragmentIfNotNull(node.AdHocDataSource);
                GenerateSymbol(TSqlTokenType.Dot); 
            }

            GenerateFragmentIfNotNull(node.ProcedureReference);

            GenerateSpace();
            GenerateParameters(node);
        }
    }
}
