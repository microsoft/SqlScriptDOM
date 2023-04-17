//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SessionTimeoutPayloadOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        // SESSION_TIMEOUT = timeoutInterval | NEVER 
        public override void ExplicitVisit(SessionTimeoutPayloadOption node)
        {
            if (node.IsNever)
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.SessionTimeout, CodeGenerationSupporter.Never);
            }
            else
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.SessionTimeout, node.Timeout);
            }
        }
    }
}
