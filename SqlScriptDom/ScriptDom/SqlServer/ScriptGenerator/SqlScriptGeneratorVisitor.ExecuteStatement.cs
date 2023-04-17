//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ExecuteStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ExecuteStatement node)
        {
            GenerateKeyword(TSqlTokenType.Execute);

            GenerateExecuteSpecificationBody(node.ExecuteSpecification);
            GenerateCommaSeparatedWithClause(node.Options, true, false);
        }

        public override void  ExplicitVisit(ExecuteSpecification node)
        {
            GenerateKeyword(TSqlTokenType.Execute);

            GenerateExecuteSpecificationBody(node);
        }

        private void GenerateExecuteSpecificationBody(ExecuteSpecification node)
        {
            if (node != null)
            {
                if (node.Variable != null)
                {
                    GenerateSpaceAndFragmentIfNotNull(node.Variable);
                    GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                }

                // could be
                //      ExecutableProcedureReference
                //      ExecutableStringList
                GenerateSpaceAndFragmentIfNotNull(node.ExecutableEntity);

                GenerateSpaceAndFragmentIfNotNull(node.ExecuteContext);

                if (node.LinkedServer != null)
                {
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.At);
                    GenerateSpaceAndFragmentIfNotNull(node.LinkedServer);
                }
            }
        }

        public override void ExplicitVisit(ExecuteOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == ExecuteOptionKind.Recompile);
            GenerateIdentifier(CodeGenerationSupporter.Recompile);
        }
    }
}
