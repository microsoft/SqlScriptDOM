//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SetTransactionIsolationLevelStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<IsolationLevel, List<TokenGenerator>> _isolationLevelGenerators = new Dictionary<IsolationLevel, List<TokenGenerator>>()
        {
            {IsolationLevel.ReadCommitted, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Read, true),
                new IdentifierGenerator(CodeGenerationSupporter.Committed),}},
            {IsolationLevel.ReadUncommitted, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Read, true),
                new IdentifierGenerator(CodeGenerationSupporter.Uncommitted),}},
            {IsolationLevel.RepeatableRead, new List<TokenGenerator>() {
                new IdentifierGenerator(CodeGenerationSupporter.Repeatable, true),
                new KeywordGenerator(TSqlTokenType.Read),}},
            {IsolationLevel.Serializable, new List<TokenGenerator>() {
                new IdentifierGenerator(CodeGenerationSupporter.Serializable), }},
            {IsolationLevel.Snapshot, new List<TokenGenerator>() {
                new IdentifierGenerator(CodeGenerationSupporter.Snapshot), }}
        };
  
        public override void ExplicitVisit(SetTransactionIsolationLevelStatement node)
        {
            GenerateKeyword(TSqlTokenType.Set);
            GenerateSpaceAndKeyword(TSqlTokenType.Transaction);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Isolation);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Level);

            List<TokenGenerator> generators = GetValueForEnumKey(_isolationLevelGenerators, node.Level);
            if (generators != null)
            {
                GenerateSpace();
                GenerateTokenList(generators);
            }
        }
    }
}
