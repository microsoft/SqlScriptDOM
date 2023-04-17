//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.PredictTableReference.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(PredictTableReference node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            GenerateIdentifier(CodeGenerationSupporter.Predict);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            if (node.ModelVariable != null)
                GenerateNameEqualsValue(CodeGenerationSupporter.Model, node.ModelVariable);
            else
                GenerateNameEqualsValue(CodeGenerationSupporter.Model, node.ModelSubquery);

            GenerateSymbol(TSqlTokenType.Comma);
            GenerateSpace();

            GenerateNameEqualsValue(CodeGenerationSupporter.Data, node.DataSource);

            if (node.RunTime != null)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpace();
                GenerateNameEqualsValue(CodeGenerationSupporter.RunTime, node.RunTime);
            }
            
            GenerateSymbol(TSqlTokenType.RightParenthesis);

            if (node.SchemaDeclarationItems.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);

                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.SchemaDeclarationItems);
            }

            GenerateSpaceAndAlias(node.Alias);
        }
    }
}
