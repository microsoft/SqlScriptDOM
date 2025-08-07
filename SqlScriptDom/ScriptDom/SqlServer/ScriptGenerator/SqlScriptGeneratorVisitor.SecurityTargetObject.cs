//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SecurityTargetObject.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<SecurityObjectKind, List<TokenGenerator>> _securityObjectKindGenerators =
            new Dictionary<SecurityObjectKind, List<TokenGenerator>>()
        {
                { SecurityObjectKind.AvailabilityGroup, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Availability, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Group) }},

                { SecurityObjectKind.ApplicationRole, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Application, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Role) }},

                { SecurityObjectKind.Assembly, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Assembly) }},

                { SecurityObjectKind.AsymmetricKey, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Asymmetric, true),
                    new KeywordGenerator(TSqlTokenType.Key) }},

                { SecurityObjectKind.Certificate, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Certificate) }},

                { SecurityObjectKind.Contract, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Contract) }},

                { SecurityObjectKind.Database, new List<TokenGenerator>() {
                    new KeywordGenerator(TSqlTokenType.Database) }},

                { SecurityObjectKind.Endpoint, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Endpoint) }},

                { SecurityObjectKind.FullTextCatalog, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Fulltext, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Catalog) }},

                { SecurityObjectKind.Login, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Login) }},

                { SecurityObjectKind.MessageType, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Message, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Type) }},

                { SecurityObjectKind.Object, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Object) }},

                { SecurityObjectKind.RemoteServiceBinding, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Remote, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Service, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Binding) }},

                { SecurityObjectKind.Role, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Role) }},

                { SecurityObjectKind.Route, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Route) }},

                { SecurityObjectKind.Schema, new List<TokenGenerator>() {
                    new KeywordGenerator(TSqlTokenType.Schema) }},

                { SecurityObjectKind.Server, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Server) }},

                { SecurityObjectKind.ServerRole, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Server, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Role)}},

                { SecurityObjectKind.Service, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Service) }},

                { SecurityObjectKind.SymmetricKey, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Symmetric, true),
                    new KeywordGenerator(TSqlTokenType.Key) }},

                { SecurityObjectKind.Type, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Type) }},

                { SecurityObjectKind.User, new List<TokenGenerator>() {
                    new KeywordGenerator(TSqlTokenType.User) }},

                { SecurityObjectKind.XmlSchemaCollection, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Xml, true),
                    new KeywordGenerator(TSqlTokenType.Schema, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Collection) }},

                { SecurityObjectKind.FullTextStopList, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Fulltext, true),
                    new KeywordGenerator(TSqlTokenType.StopList)}},

                { SecurityObjectKind.SearchPropertyList, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.Search, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Property, true),
                    new IdentifierGenerator(CodeGenerationSupporter.List)}},

                { SecurityObjectKind.ExternalModel, new List<TokenGenerator>() {
                    new IdentifierGenerator(CodeGenerationSupporter.External, true),
                    new IdentifierGenerator(CodeGenerationSupporter.Model) }},
                
        };


        public override void ExplicitVisit(SecurityTargetObject node)
        {
            GenerateKeyword(TSqlTokenType.On);

            GenerateSpace();
            if (node.ObjectKind != SecurityObjectKind.NotSpecified)
            {
                GenerateSourceForSecurityObjectKind(node.ObjectKind);
            }

            GenerateFragmentIfNotNull(node.ObjectName);

            if (node.Columns != null && node.Columns.Count > 0)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Columns);
            }
        }

        protected void GenerateSourceForSecurityObjectKind(SecurityObjectKind type)
        {
            List<TokenGenerator> generators = GetValueForEnumKey(_securityObjectKindGenerators, type);
            if (generators != null)
            {
                GenerateTokenList(generators);
            }

            GenerateSymbol(TSqlTokenType.DoubleColon);
        }

        public override void ExplicitVisit(SecurityTargetObjectName node)
        {
            GenerateFragmentIfNotNull(node.MultiPartIdentifier);
        }

    }
}