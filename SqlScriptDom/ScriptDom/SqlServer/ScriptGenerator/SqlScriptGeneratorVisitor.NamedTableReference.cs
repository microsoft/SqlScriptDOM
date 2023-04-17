//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SchemaObjectTableSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(NamedTableReference node)
        {
            GenerateFragmentIfNotNull(node.SchemaObject);

            if (node.TemporalClause != null)
            {
                ExplicitVisit(node.TemporalClause);
            }

            if (node.ForPath)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.For);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Path);
            }

            GenerateSpaceAndAlias(node.Alias);
            
            GenerateSpaceAndFragmentIfNotNull(node.TableSampleClause);
            GenerateWithTableHints(node.TableHints);
        }

        public override void ExplicitVisit(SchemaObjectFunctionTableReference node)
        {
            GenerateFragmentIfNotNull(node.SchemaObject);
            GenerateParenthesisedCommaSeparatedList(node.Parameters, true);
            GenerateTableAndColumnAliases(node); 
        }
    }
}
