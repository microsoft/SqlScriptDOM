//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ProcedureStatementBody.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateProcedureStatementBody(ProcedureStatementBody node)
        {
            GenerateKeyword(TSqlTokenType.Procedure);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.ProcedureReference);

            if (node.Parameters != null && node.Parameters.Count > 0)
            {
                NewLine();
                GenerateCommaSeparatedList(node.Parameters);
            }

            GenerateCommaSeparatedWithClause(node.Options, false, false);

            // FOR REPLICATION
            if (node.IsForReplication)
            {
                NewLine();
                GenerateKeyword(TSqlTokenType.For);
                GenerateSpaceAndKeyword(TSqlTokenType.Replication); 
         }

            NewLine();
            GenerateKeyword(TSqlTokenType.As);

            // procedure body
            if (node.StatementList != null)
            {
                NewLine();
                GenerateFragmentIfNotNull(node.StatementList);
            }

            // external name
            GenerateSpaceAndFragmentIfNotNull(node.MethodSpecifier);
        }

        public override void ExplicitVisit(ProcedureOption node)
        {
            switch(node.OptionKind)
            {
                case ProcedureOptionKind.Encryption:
                    GenerateIdentifier(CodeGenerationSupporter.Encryption);
                    break;
                case ProcedureOptionKind.Recompile:
                    GenerateIdentifier(CodeGenerationSupporter.Recompile);
                    break;
                case ProcedureOptionKind.NativeCompilation:
                    GenerateIdentifier(CodeGenerationSupporter.NativeCompilation);
                    break;
                case ProcedureOptionKind.SchemaBinding:
                    GenerateIdentifier(CodeGenerationSupporter.SchemaBinding);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }

        public override void ExplicitVisit(ExecuteAsProcedureOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind==ProcedureOptionKind.ExecuteAs);
            GenerateFragmentIfNotNull(node.ExecuteAs);
        }
    }
}
