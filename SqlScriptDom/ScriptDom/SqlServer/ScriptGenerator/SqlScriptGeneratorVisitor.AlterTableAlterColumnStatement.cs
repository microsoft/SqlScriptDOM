//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterTableAlterColumnStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {

        private static Dictionary<AlterTableAlterColumnOption, List<TokenGenerator>> _alterTableAlterColumnOptionNames =
            new Dictionary<AlterTableAlterColumnOption, List<TokenGenerator>>()
        {
            {   AlterTableAlterColumnOption.AddRowGuidCol, new List<TokenGenerator>() { 
                    new KeywordGenerator(TSqlTokenType.Add, true), 
                    new KeywordGenerator(TSqlTokenType.RowGuidColumn)}},
            {   AlterTableAlterColumnOption.DropRowGuidCol, new List<TokenGenerator>() { 
                    new KeywordGenerator(TSqlTokenType.Drop, true), 
                    new KeywordGenerator(TSqlTokenType.RowGuidColumn)}},
            {   AlterTableAlterColumnOption.AddPersisted, new List<TokenGenerator>() { 
                    new KeywordGenerator(TSqlTokenType.Add, true), 
                    new IdentifierGenerator(CodeGenerationSupporter.Persisted)}},
            {   AlterTableAlterColumnOption.DropPersisted, new List<TokenGenerator>() { 
                    new KeywordGenerator(TSqlTokenType.Drop, true), 
                    new IdentifierGenerator(CodeGenerationSupporter.Persisted)}},
            {   AlterTableAlterColumnOption.AddNotForReplication, new List<TokenGenerator>() { 
                    new KeywordGenerator(TSqlTokenType.Add, true), 
                    new KeywordGenerator(TSqlTokenType.Not, true), 
                    new KeywordGenerator(TSqlTokenType.For, true), 
                    new KeywordGenerator(TSqlTokenType.Replication)}},
            {   AlterTableAlterColumnOption.DropNotForReplication, new List<TokenGenerator>() { 
                    new KeywordGenerator(TSqlTokenType.Drop, true), 
                    new KeywordGenerator(TSqlTokenType.Not, true), 
                    new KeywordGenerator(TSqlTokenType.For, true), 
                    new KeywordGenerator(TSqlTokenType.Replication)}},
            { AlterTableAlterColumnOption.AddSparse, new List<TokenGenerator>() {
                    new KeywordGenerator(TSqlTokenType.Add, true), 
                    new IdentifierGenerator(CodeGenerationSupporter.Sparse)}},
            { AlterTableAlterColumnOption.DropSparse, new List<TokenGenerator>() {
                    new KeywordGenerator(TSqlTokenType.Drop, true), 
                    new IdentifierGenerator(CodeGenerationSupporter.Sparse)}},
            { AlterTableAlterColumnOption.AddMaskingFunction, new List<TokenGenerator>() {
                    new KeywordGenerator(TSqlTokenType.Add, true), 
                    new IdentifierGenerator(CodeGenerationSupporter.Masked, true),
                    new KeywordGenerator(TSqlTokenType.With, true), 
                    new KeywordGenerator(TSqlTokenType.LeftParenthesis), 
                    new KeywordGenerator(TSqlTokenType.Function, true), 
                    new KeywordGenerator(TSqlTokenType.EqualsSign)}}, 
            { AlterTableAlterColumnOption.DropMaskingFunction, new List<TokenGenerator>() {
                    new KeywordGenerator(TSqlTokenType.Drop, true), 
                    new IdentifierGenerator(CodeGenerationSupporter.Masked)}},
            { AlterTableAlterColumnOption.AddHidden, new List<TokenGenerator>() {
                    new KeywordGenerator(TSqlTokenType.Add, true), 
                    new IdentifierGenerator(CodeGenerationSupporter.Hidden)}},
            { AlterTableAlterColumnOption.DropHidden, new List<TokenGenerator>() {
                    new KeywordGenerator(TSqlTokenType.Drop, true), 
                    new IdentifierGenerator(CodeGenerationSupporter.Hidden)}},
        };

        public override void ExplicitVisit(AlterTableAlterColumnStatement node)
        {
            GenerateAlterTableHead(node);

            GenerateSpaceAndKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndKeyword(TSqlTokenType.Column);

            // column name
            GenerateSpaceAndFragmentIfNotNull(node.ColumnIdentifier);

            if (node.DataType != null)
            {
                GenerateSpaceAndFragmentIfNotNull(node.DataType);
                GenerateSpaceAndCollation(node.Collation);

                if (node.GeneratedAlways != null)
                {
                    GenerateGeneratedAlways(node.GeneratedAlways);
                }

                GenerateFragmentIfNotNull(node.StorageOptions);

                GenerateSpaceAndFragmentIfNotNull(node.Encryption);

                if (node.MaskingFunction != null)
                {
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Masked);
                    GenerateSpaceAndKeyword(TSqlTokenType.With);
                    GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
                    GenerateKeyword(TSqlTokenType.Function);
                    GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                    GenerateSpaceAndFragmentIfNotNull(node.MaskingFunction);
                    GenerateSymbol(TSqlTokenType.RightParenthesis);
                }

                if (node.IsHidden)
                {
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Hidden);
                }

                switch (node.AlterTableAlterColumnOption)
                {
                    case AlterTableAlterColumnOption.NoOptionDefined:
                        break;
                    case AlterTableAlterColumnOption.Null:
                        GenerateSpaceAndKeyword(TSqlTokenType.Null);
                        break;
                    case AlterTableAlterColumnOption.NotNull:
                        GenerateSpaceAndKeyword(TSqlTokenType.Not);
                        GenerateSpaceAndKeyword(TSqlTokenType.Null);
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                        break;
                }
            }
            else
            {
                List<TokenGenerator> generators = GetValueForEnumKey(_alterTableAlterColumnOptionNames, node.AlterTableAlterColumnOption);
                if (generators != null)
                {
                    GenerateSpace();
                    GenerateTokenList(generators);

                    if (node.AlterTableAlterColumnOption == AlterTableAlterColumnOption.AddMaskingFunction)
                    {
                        GenerateSpaceAndFragmentIfNotNull(node.MaskingFunction);
                        GenerateSymbol(TSqlTokenType.RightParenthesis);
                    }
                }
            }

            // Add the options if any.
            if (node.Options.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Options);
            }
        }
    }
}