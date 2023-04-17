//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ResultSetsExecuteOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ResultSetsExecuteOption node)
        {
            Debug.Assert(node.OptionKind == ExecuteOptionKind.ResultSets);
            GenerateIdentifier(CodeGenerationSupporter.Result);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Sets);
            switch (node.ResultSetsOptionKind)
            {
                case ResultSetsOptionKind.Undefined:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Undefined);
                    break;
                case ResultSetsOptionKind.None:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.None);
                    break;
                case ResultSetsOptionKind.ResultSetsDefined:
                    GenerateAlignedParenthesizedOptionsWithMultipleIndent(node.Definitions, 2);
                    break;
                default:
                    Debug.Assert(false, "unknown option");
                    break;
            }
        }

        public override void ExplicitVisit(ResultSetDefinition node)
        {
            Debug.Assert(node.ResultSetType == ResultSetType.ForXml);
            GenerateSpaceAndKeyword(TSqlTokenType.As);
            GenerateSpaceAndKeyword(TSqlTokenType.For);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Xml);
        }

        public override void ExplicitVisit(SchemaObjectResultSetDefinition node)
        {
            Debug.Assert(node.ResultSetType == ResultSetType.Object || node.ResultSetType==ResultSetType.Type);
            GenerateSpaceAndKeyword(TSqlTokenType.As);
            switch (node.ResultSetType)
            {
                case ResultSetType.Object:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Object);
                    break;
                case ResultSetType.Type:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Type);
                    break;
                default:
                    Debug.Assert(false,"Unexpected option encountered");
                    break;
            }
            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }

        public override void ExplicitVisit(InlineResultSetDefinition node)
        {
            Debug.Assert(node.ResultSetType == ResultSetType.Inline);
            GenerateParenthesisedCommaSeparatedList(node.ResultColumnDefinitions);
        }

        public override void ExplicitVisit(ResultColumnDefinition node)
        {
            GenerateFragmentIfNotNull(node.ColumnDefinition);
            GenerateSpaceAndFragmentIfNotNull(node.Nullable);
        }
    }
}
