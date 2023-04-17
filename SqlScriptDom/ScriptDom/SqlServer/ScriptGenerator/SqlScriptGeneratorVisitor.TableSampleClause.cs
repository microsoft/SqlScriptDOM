//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TableSampleClause.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<TableSampleClauseOption, TokenGenerator> _tableSampleClauseOptionGenerators = 
            new Dictionary<TableSampleClauseOption, TokenGenerator>()
        {
            {TableSampleClauseOption.NotSpecified, new EmptyGenerator()},
            {TableSampleClauseOption.Percent, new KeywordGenerator(TSqlTokenType.Percent)},
            {TableSampleClauseOption.Rows, new IdentifierGenerator(CodeGenerationSupporter.Rows)},
        };
  

        public override void ExplicitVisit(TableSampleClause node)
        {
            GenerateKeyword(TSqlTokenType.TableSample); 

            if (node.System)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.System); 
            }

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.SampleNumber);
            TokenGenerator generator = GetValueForEnumKey(_tableSampleClauseOptionGenerators, node.TableSampleClauseOption);
            if (generator != null && node.TableSampleClauseOption != TableSampleClauseOption.NotSpecified)
            {
                GenerateSpace();
                GenerateToken(generator);
            }
            GenerateSymbol(TSqlTokenType.RightParenthesis); 

            if (node.RepeatSeed != null)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Repeatable);
                GenerateSpace();
                GenerateParenthesisedFragmentIfNotNull(node.RepeatSeed);
            }
        }
    }
}
