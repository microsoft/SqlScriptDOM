//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterDbTermination.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterDatabaseTermination node)
        {
            NewLineAndIndent();
            GenerateKeywordAndSpace(TSqlTokenType.With); 
            if (node.ImmediateRollback)
            {
                GenerateKeyword(TSqlTokenType.Rollback);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Immediate); 
            }
            else if (node.RollbackAfter != null)
            {
                GenerateKeyword(TSqlTokenType.Rollback);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.After); 
                GenerateSpaceAndFragmentIfNotNull(node.RollbackAfter);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Seconds); 
            }
            else if (node.NoWait)
            {
                GenerateIdentifier(CodeGenerationSupporter.NoWaitAlterDb); 
            }
        }
    }
}
