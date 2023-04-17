using System.Collections.Generic;
//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.Constraints.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {

        private static Dictionary<DeleteUpdateAction, List<TokenGenerator>> _deleteUpdateActionGenerators = 
            new Dictionary<DeleteUpdateAction, List<TokenGenerator>>()
        {
            {DeleteUpdateAction.Cascade, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Cascade), }},
            {DeleteUpdateAction.NoAction,  new List<TokenGenerator>() {
                new IdentifierGenerator(CodeGenerationSupporter.No, true),
                new IdentifierGenerator(CodeGenerationSupporter.Action),}},
            {DeleteUpdateAction.SetDefault, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Set, true),
                new KeywordGenerator(TSqlTokenType.Default),}},
            {DeleteUpdateAction.SetNull, new List<TokenGenerator>() {
                new KeywordGenerator(TSqlTokenType.Set, true),
                new KeywordGenerator(TSqlTokenType.Null),}},
        };
  
        protected void GenerateConstraintHead(ConstraintDefinition node)
        {
            if (node.ConstraintIdentifier != null)
            {
                GenerateKeyword(TSqlTokenType.Constraint); 
                GenerateSpaceAndFragmentIfNotNull(node.ConstraintIdentifier);
                GenerateSpace();
            }
        }

        protected void GenerateDeleteUpdateAction(DeleteUpdateAction action)
        {
            List<TokenGenerator> generators = GetValueForEnumKey(_deleteUpdateActionGenerators, action);
            if (generators != null)
            {
                GenerateTokenList(generators);
            }
        }
    }
}
