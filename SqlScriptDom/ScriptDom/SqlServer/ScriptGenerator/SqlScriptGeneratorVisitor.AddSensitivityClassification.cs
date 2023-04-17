//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AddSensitivityClassification.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AddSensitivityClassificationStatement node)
        {
            GenerateKeyword(TSqlTokenType.Add);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Sensitivity);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Classification);
            GenerateSpaceAndKeyword(TSqlTokenType.To);

            NewLineAndIndent();
            GenerateCommaSeparatedList(node.Columns);

            NewLineAndIndent();            
            GenerateKeywordAndSpace(TSqlTokenType.With);
            GenerateParenthesisedCommaSeparatedList(node.Options);
        }

        public override void ExplicitVisit(SensitivityClassificationOption node)
        {
            switch (node.Type)
            {
                case SensitivityClassification.OptionType.Label:
                    GenerateNameEqualsValue(CodeGenerationSupporter.Label, node.Value);
                    break;

                case SensitivityClassification.OptionType.LabelId:
                    GenerateNameEqualsValue(CodeGenerationSupporter.LabelId, node.Value);
                    break;

                case SensitivityClassification.OptionType.InformationType:
                    GenerateNameEqualsValue(CodeGenerationSupporter.InformationType, node.Value);
                    break;

                case SensitivityClassification.OptionType.InformationTypeId:
                    GenerateNameEqualsValue(CodeGenerationSupporter.InformationTypeId, node.Value);
                    break;

                case SensitivityClassification.OptionType.Rank:
                    GenerateTokenAndEqualSign(CodeGenerationSupporter.Rank);
                    GenerateSpaceAndIdentifier(node.Value.Value);
                    break;
            }
        }
    }
}
