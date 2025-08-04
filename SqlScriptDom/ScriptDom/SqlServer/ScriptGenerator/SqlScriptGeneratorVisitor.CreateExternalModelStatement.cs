//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateExternalModelStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SqlServer.TransactSql.ScriptDom;
namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateExternalModelStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.External);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Model);
            GenerateCreateExternalModelStatementBody(node);
        }
        protected static Dictionary<ExternalModelTypeOption, string> _externalModelTypeOption = new Dictionary<ExternalModelTypeOption, string>()
        {
            {ExternalModelTypeOption.EMBEDDINGS, CodeGenerationSupporter.Embeddings}
        };

        protected void GenerateCreateExternalModelStatementBody(CreateExternalModelStatement node)
        {
            // external model name
            GenerateSpaceAndFragmentIfNotNull(node.Name);
            GenerateOwnerIfNotNull(node.Owner);

            NewLine();
            GenerateKeywordAndSpace(TSqlTokenType.With);
            GenerateKeyword(TSqlTokenType.LeftParenthesis);
            bool ifFirst = true;

            // external model location
            if (node.Location != null)
            {
                NewLine();
                GenerateNameEqualsValue(CodeGenerationSupporter.Location, node.Location);
                ifFirst = false;
            }

            // external model API Format options
            if (node.ApiFormat != null)
            {
                if (!ifFirst)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                }
                ifFirst = false;
                NewLine();
                GenerateNameEqualsValue(CodeGenerationSupporter.ApiFormat, node.ApiFormat);
            }

            // external model Model Type options
            if (node.ModelType == ExternalModelTypeOption.EMBEDDINGS)
            {
                if (!ifFirst)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                }
                ifFirst = false;
                ExternalModelTypeOption typeOption = ExternalModelTypeOption.EMBEDDINGS;
                string externalModelTypeOption = GetValueForEnumKey(_externalModelTypeOption, typeOption);
                NewLine();
                GenerateNameEqualsValue(CodeGenerationSupporter.ModelType, externalModelTypeOption);
            }

            // external model name options
            if (node.ModelName != null)
            {
                if (!ifFirst)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                }
                ifFirst = false;
                NewLine();
                GenerateNameEqualsValue(CodeGenerationSupporter.ModelName, node.ModelName);
            }

            // external model credential options
            if (node.Credential != null)
            {
                if (!ifFirst)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                }
                ifFirst = false;
                NewLine();
                GenerateNameEqualsValue(CodeGenerationSupporter.Credential, node.Credential);
            }

            // external model parameters options
            if (node.Parameters != null)
            {
                if (!ifFirst)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                }
                ifFirst = false;
                NewLine();
                GenerateNameEqualsValue(CodeGenerationSupporter.Parameters, node.Parameters);
            }

            // external model local runtime path options
            if (node.LocalRuntimePath != null)
            {
                if (!ifFirst)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                }
                ifFirst = false;
                NewLine();
                GenerateNameEqualsValue(CodeGenerationSupporter.LocalRuntimePath, node.LocalRuntimePath);
            }

            NewLine();
            GenerateKeyword(TSqlTokenType.RightParenthesis);
        }
    }
}
