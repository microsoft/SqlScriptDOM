//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SecurityPolicyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        /// <summary>
        /// Generates the body for both Create and Alter Security policy statements.
        /// </summary>
        /// <param name="node">The SecurityPolicyStatement to script.</param>
        public void GenerateSecurityPolicyStatementBody(SecurityPolicyStatement node)
        {
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            for(int i = 0; i < node.SecurityPredicateActions.Count; i++)
            {
                GenerateFragmentIfNotNull(node.SecurityPredicateActions[i]);
                if (i < node.SecurityPredicateActions.Count - 1)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                }
            }

            if (node.SecurityPolicyOptions.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
                for (int i = 0; i < node.SecurityPolicyOptions.Count; i++)
                {
                    GenerateFragmentIfNotNull(node.SecurityPolicyOptions[i]);

                    if (i < node.SecurityPolicyOptions.Count - 1)
                    {
                        GenerateSymbol(TSqlTokenType.Comma);
                        GenerateSpace();
                    }
                }

                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }

            if (node.ActionType == SecurityPolicyActionType.Create || node.ActionType == SecurityPolicyActionType.AlterReplication)
            {
                if (node.NotForReplication)
                {
                    NewLineAndIndent();
                    if (node.ActionType == SecurityPolicyActionType.AlterReplication)
                    {
                        GenerateKeyword(TSqlTokenType.Add);
                        GenerateSpace();
                    }

                    GenerateKeyword(TSqlTokenType.Not);
                    GenerateSpaceAndKeyword(TSqlTokenType.For);
                    GenerateSpaceAndKeyword(TSqlTokenType.Replication);
                }
                else if (node.ActionType == SecurityPolicyActionType.AlterReplication)
                {
                    NewLineAndIndent();
                    GenerateKeyword(TSqlTokenType.Drop);
                    GenerateSpaceAndKeyword(TSqlTokenType.Not);
                    GenerateSpaceAndKeyword(TSqlTokenType.For);
                    GenerateSpaceAndKeyword(TSqlTokenType.Replication);
                }
            }
        }

        /// <summary>
        /// Generates a script for this predicate action (add, drop, or alter)
        /// </summary>
        /// <param name="node">The security predicate action to script</param>
        public override void ExplicitVisit(SecurityPredicateAction node)
        {
            NewLineAndIndent();
            if(node.ActionType == SecurityPredicateActionType.Create)
            {
                GenerateKeyword(TSqlTokenType.Add);
            }
            else if (node.ActionType == SecurityPredicateActionType.Alter)
            {
                GenerateKeyword(TSqlTokenType.Alter);
            }
            else
            {
                GenerateKeyword(TSqlTokenType.Drop);
            }

            if (node.SecurityPredicateType == SecurityPredicateType.Filter)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Filter);
            }
            else
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Block);
            }

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Predicate);
            if (node.ActionType != SecurityPredicateActionType.Drop)
            {
                GenerateSpaceAndFragmentIfNotNull(node.FunctionCall);
            }

            GenerateSpaceAndKeyword(TSqlTokenType.On);
            GenerateSpaceAndFragmentIfNotNull(node.TargetObjectName);

            switch (node.SecurityPredicateOperation)
            {
                case SecurityPredicateOperation.AfterInsert:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.After);
                    GenerateSpaceAndKeyword(TSqlTokenType.Insert);
                    break;
                case SecurityPredicateOperation.AfterUpdate:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.After);
                    GenerateSpaceAndKeyword(TSqlTokenType.Update);
                    break;
                case SecurityPredicateOperation.BeforeDelete:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Before);
                    GenerateSpaceAndKeyword(TSqlTokenType.Delete);
                    break;
                case SecurityPredicateOperation.BeforeUpdate:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Before);
                    GenerateSpaceAndKeyword(TSqlTokenType.Update);
                    break;
                default:
                    // Do nothing for SecurityPredicateOperation.All
                    //
                    break;
            }
        }

        /// <summary>
        /// Generates a script for this security policy option.
        /// </summary>
        /// <param name="node">The security policy option to script</param>
        public override void ExplicitVisit(SecurityPolicyOption node)
        {
            if (node.OptionKind == SecurityPolicyOptionKind.State)
            {
                GenerateIdentifier(CodeGenerationSupporter.State);
            }
            else
            {
                GenerateIdentifier(CodeGenerationSupporter.SchemaBinding);
            }

            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndKeyword(node.OptionState == OptionState.On ? TSqlTokenType.On : TSqlTokenType.Off);
        }
    }
}
