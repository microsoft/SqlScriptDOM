//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DeclareVariableStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DeclareVariableStatement node)
        {
            GenerateKeyword(TSqlTokenType.Declare);

            GenerateSpace();
            GenerateCommaSeparatedList(node.Declarations);
        }
    }
}
