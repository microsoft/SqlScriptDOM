//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ShutdownStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ShutdownStatement node)
        {
            GenerateKeyword(TSqlTokenType.Shutdown); 
            if (node.WithNoWait)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.NoWait); 
            }
        }
    }
}
