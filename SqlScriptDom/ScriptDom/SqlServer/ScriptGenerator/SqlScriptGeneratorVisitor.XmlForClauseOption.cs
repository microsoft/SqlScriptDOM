//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.XmlForClauseOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<XmlForClauseOptions, List<TokenGenerator>> _xmlForClauseOptionsGenerators = 
            new Dictionary<XmlForClauseOptions, List<TokenGenerator>>()
        {
            // exlude
            //{ XmlForClauseOptions.None, new IdentifierGenerator(CodeGenerationSupporter.None) },
            { XmlForClauseOptions.Raw, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Raw) }},

            { XmlForClauseOptions.Auto, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Auto) }},

            { XmlForClauseOptions.Explicit, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Explicit) }},

            { XmlForClauseOptions.Path, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Path) }},

            { XmlForClauseOptions.XmlData, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.XmlData) }},

            { XmlForClauseOptions.XmlSchema, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.XmlSchema) }},

            { XmlForClauseOptions.Elements, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Elements) }},

            { XmlForClauseOptions.ElementsXsiNil, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Elements, true),
                    new IdentifierGenerator(CodeGenerationSupporter.XsiNil) }},

            { XmlForClauseOptions.ElementsAbsent, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Elements, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Absent) }},

            { XmlForClauseOptions.BinaryBase64, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Binary, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Base64) }},

            { XmlForClauseOptions.Type, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Type) }},

            { XmlForClauseOptions.Root, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Root) }},

            // aggregated option
            //{ XmlForClauseOptions.ElementsAll, new List<TokenGenerator>() {
            //        new IdentifierGenerator(CodeGenerationSupporter.ElementsAll) }},
        };
  
        public override void ExplicitVisit(XmlForClauseOption node)
        {
            List<TokenGenerator> generators = GetValueForEnumKey(_xmlForClauseOptionsGenerators, node.OptionKind);
            if (generators != null)
            {
                GenerateTokenList(generators);
            }

            if (node.Value != null)
            {
                GenerateSpace();
                GenerateParenthesisedFragmentIfNotNull(node.Value);
            }
        }
    }
}
