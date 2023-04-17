//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ContainmentDatabaseOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ContainmentDatabaseOption node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Containment);
            GenerateSpace();
            GenerateSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            ContainmentOptionKindHelper.Instance.GenerateSourceForOption(_writer, node.Value);

        }
    }
}
