//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ProcedureReference.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ProcedureReference node)
        {
            GenerateFragmentIfNotNull(node.Name);

            if (node.Number != null)
            {
                GenerateToken(TSqlTokenType.ProcNameSemicolon, CodeGenerationSupporter.SemiColon);
                GenerateFragmentIfNotNull(node.Number);
            }
        }
    }
}
