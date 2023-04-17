//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterTableDropTableElement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        
        protected static Dictionary<TableElementType, TokenGenerator> _tableElementTypeGenerators = new Dictionary<TableElementType, TokenGenerator>()
        {
            { TableElementType.Column, new KeywordGenerator(TSqlTokenType.Column)},
            { TableElementType.Constraint, new KeywordGenerator(TSqlTokenType.Constraint)},
            { TableElementType.Index, new KeywordGenerator(TSqlTokenType.Index)},
            { TableElementType.Period, new IdentifierGenerator(CodeGenerationSupporter.Period)},
            { TableElementType.NotSpecified, new EmptyGenerator()},
        };
  
        public override void ExplicitVisit(AlterTableDropTableElement node)
        {
            TokenGenerator generator = GetValueForEnumKey(_tableElementTypeGenerators, node.TableElementType);
            if (generator != null)
            {
                GenerateToken(generator);
            }

            if (node.IsIfExists)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.If);
                GenerateSpaceAndKeyword(TSqlTokenType.Exists);
            }

            // name
            if (node.TableElementType != TableElementType.NotSpecified)
            {
                GenerateSpace();
            }
            GenerateFragmentIfNotNull(node.Name);

            if (node.DropClusteredConstraintOptions.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);

                // could be
                //      DropClusteredConstraintFragmentOption
                //      DropClusteredConstraintStateOption
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.DropClusteredConstraintOptions);
            }

            if (node.TableElementType == TableElementType.Period)
            {
                GenerateIdentifier(CodeGenerationSupporter.For);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.SystemTime);
            }
        }
    }
}
