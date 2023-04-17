//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.EventSessionStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateEventSessionStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Create);
            GenerateEventSessionParameters(node);
            GenerateEventDeclarations(node);
            GenerateTargetDeclarations(node);
            GenerateEventSessionOptions(node);
        }

        public void GenerateEventSessionParameters(EventSessionStatement node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Event);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Session);

            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateSpaceAndKeyword(TSqlTokenType.On);
            switch (node.SessionScope)
            {
                case EventSessionScope.Server:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Server);
                    break;
                case EventSessionScope.Database:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Database);
                    break;
                default:
                    throw new InvalidOperationException("Unexpected value for EventSessionScope: " + node.SessionScope.ToString());
            }
            
            GenerateSpace();
        }

        public void GenerateEventDeclarations(EventSessionStatement node)
        {
            if ((node.EventDeclarations != null) && (node.EventDeclarations.Count > 0))
            {
                GenerateCommaSeparatedList(node.EventDeclarations);
            }
        }

        public void GenerateTargetDeclarations(EventSessionStatement node)
        {
            if ((node.TargetDeclarations != null) && (node.TargetDeclarations.Count > 0))
            {
                if (node is CreateEventSessionStatement)
                    GenerateSpace();
                GenerateCommaSeparatedList(node.TargetDeclarations);
            }
        }

        public void GenerateEventSessionOptions(EventSessionStatement node)
        {
            if ((node.SessionOptions != null) && (node.SessionOptions.Count > 0))
            {
                NewLine();
                GenerateKeywordAndSpace(TSqlTokenType.With);
                GenerateAlignedParenthesizedOptionsWithMultipleIndent(node.SessionOptions, 2);
            }
        }

        public override void ExplicitVisit(EventDeclaration node)
        {
            NewLine();
            GenerateKeywordAndSpace(TSqlTokenType.Add);
            GenerateIdentifier(CodeGenerationSupporter.Event);
            GenerateSpaceAndFragmentIfNotNull(node.ObjectName);

            if ((node.EventDeclarationSetParameters.Count > 0) ||
                (node.EventDeclarationActionParameters.Count > 0) ||
                (node.EventDeclarationPredicateParameter != null))
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.LeftParenthesis);
            }
            
            if ((node.EventDeclarationSetParameters != null) && (node.EventDeclarationSetParameters.Count > 0))
            {
                NewLineAndIndent();
                GenerateKeywordAndSpace(TSqlTokenType.Set);
                GenerateFragmentList(node.EventDeclarationSetParameters, ListGenerationOption.MultipleLineSelectElementOption);
            }
            
            if ((node.EventDeclarationActionParameters != null) && (node.EventDeclarationActionParameters.Count > 0))
            {
                NewLineAndIndent();
                GenerateIdentifier(CodeGenerationSupporter.Action);
                GenerateSpaceAndKeyword(TSqlTokenType.LeftParenthesis);
                GenerateFragmentList(node.EventDeclarationActionParameters, ListGenerationOption.MultipleLineSelectElementOption);
                GenerateKeyword(TSqlTokenType.RightParenthesis);
            }
            
            if (node.EventDeclarationPredicateParameter != null)
            {
                NewLineAndIndent();
                GenerateKeywordAndSpace(TSqlTokenType.Where);
                GenerateFragmentIfNotNull(node.EventDeclarationPredicateParameter);
            }

            if ((node.EventDeclarationSetParameters.Count > 0) ||
                (node.EventDeclarationActionParameters.Count > 0) ||
                (node.EventDeclarationPredicateParameter != null))
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.RightParenthesis);
            }
        }

        public override void ExplicitVisit(EventDeclarationSetParameter node)
        {
            GenerateFragmentIfNotNull(node.EventField);
            GenerateSpace();
            GenerateKeywordAndSpace(TSqlTokenType.EqualsSign);
            GenerateFragmentIfNotNull(node.EventValue);
        }

        public override void ExplicitVisit(EventSessionObjectName node)
        {
            GenerateFragmentIfNotNull(node.MultiPartIdentifier);
        }

        public override void ExplicitVisit(EventDeclarationCompareFunctionParameter node)
        {
            GenerateFragmentIfNotNull(node.Name);
            GenerateSpaceAndKeyword(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.SourceDeclaration);
            GenerateKeyword(TSqlTokenType.Comma);
            GenerateSpaceAndFragmentIfNotNull(node.EventValue);
            GenerateKeyword(TSqlTokenType.RightParenthesis);
        }

        public override void ExplicitVisit(TargetDeclaration node)
        {
            NewLine();
            GenerateKeywordAndSpace(TSqlTokenType.Add);
            GenerateIdentifier(CodeGenerationSupporter.Target);
            GenerateSpaceAndFragmentIfNotNull(node.ObjectName);
            if ((node.TargetDeclarationParameters != null) && (node.TargetDeclarationParameters.Count > 0))
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.LeftParenthesis);
                NewLineAndIndent();
                GenerateKeywordAndSpace(TSqlTokenType.Set);
                GenerateFragmentList(node.TargetDeclarationParameters, ListGenerationOption.MultipleLineSelectElementOption);
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.RightParenthesis);
            }
        }

        public override void ExplicitVisit(LiteralSessionOption node)
        {
            switch (node.OptionKind)
            {
                case SessionOptionKind.MaxMemory:
                    GenerateIdentifier(CodeGenerationSupporter.MaxMemory);
                    break;
                case SessionOptionKind.MaxEventSize:
                    GenerateIdentifier(CodeGenerationSupporter.MaxEventSize);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false,"Unexpected Option encountered");
                    break;
            }
            GenerateIntegerValueSessionOption(node);
        }

        public override void ExplicitVisit(MaxDispatchLatencySessionOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == SessionOptionKind.MaxDispatchLatency);
            GenerateIdentifier(CodeGenerationSupporter.MaxDispatchLatency);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            GenerateSpace();
            if (node.IsInfinite)
                GenerateIdentifier(CodeGenerationSupporter.Infinite);
            else
            {
                GenerateFragmentIfNotNull(node.Value);
                GenerateSpace();
                GenerateIdentifier(CodeGenerationSupporter.Seconds);
            }
        }

        public void GenerateIntegerValueSessionOption(LiteralSessionOption node)
        {
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            GenerateSpace();
            GenerateFragmentIfNotNull(node.Value);
            GenerateSpace();
            SessionOptionUnitHelper.Instance.GenerateSourceForOption(_writer, node.Unit);
        }

        public override void ExplicitVisit(OnOffSessionOption node)
        {
            switch (node.OptionKind)
            {
                case SessionOptionKind.TrackCausality:
                    GenerateOptionState(CodeGenerationSupporter.TrackCausality, node.OptionState, true);
                    break;
                case SessionOptionKind.StartUpState:
                    GenerateOptionState(CodeGenerationSupporter.StartupState, node.OptionState, true);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }

        public override void ExplicitVisit(EventRetentionSessionOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == SessionOptionKind.EventRetention);
            GenerateIdentifier(CodeGenerationSupporter.EventRetentionMode);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            GenerateSpace();
            EventSessionEventRetentionModeTypeHelper.Instance.GenerateSourceForOption(_writer, node.Value);
        }

        public override void ExplicitVisit(MemoryPartitionSessionOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == SessionOptionKind.MemoryPartition);
            GenerateIdentifier(CodeGenerationSupporter.MemoryPartitionMode);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            GenerateSpace();
            EventSessionMemoryPartitionModeTypeHelper.Instance.GenerateSourceForOption(_writer, node.Value);
        }

        public override void ExplicitVisit(AlterEventSessionStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Alter);
            GenerateEventSessionParameters(node);

            switch (node.StatementType)
            {
                case AlterEventSessionStatementType.AddEventDeclarationOptionalSessionOptions:
                    GenerateEventDeclarations(node);
                    GenerateEventSessionOptions(node);
                    break;
                case AlterEventSessionStatementType.AddTargetDeclarationOptionalSessionOptions:
                    GenerateTargetDeclarations(node);
                    GenerateEventSessionOptions(node);
                    break;
                case AlterEventSessionStatementType.DropEventSpecificationOptionalSessionOptions:
                    GenerateCommaSeparatedDropDeclarations(node.DropEventDeclarations, CodeGenerationSupporter.Event);
                    GenerateEventSessionOptions(node);
                    break;
                case AlterEventSessionStatementType.DropTargetSpecificationOptionalSessionOptions:
                    GenerateCommaSeparatedDropDeclarations(node.DropTargetDeclarations, CodeGenerationSupporter.Target);
                    GenerateEventSessionOptions(node);
                    break;
                case AlterEventSessionStatementType.RequiredSessionOptions:
                    GenerateEventSessionOptions(node);
                    break;
                case AlterEventSessionStatementType.AlterStateIsStart:
                    NewLine();
                    GenerateNameEqualsValue(CodeGenerationSupporter.State, CodeGenerationSupporter.Start);
                    break;
                case AlterEventSessionStatementType.AlterStateIsStop:
                    NewLine();
                    GenerateNameEqualsValue(CodeGenerationSupporter.State, CodeGenerationSupporter.Stop);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }

        public override void ExplicitVisit(DropEventSessionStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Drop);
            GenerateIdentifier(CodeGenerationSupporter.Event);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Session);

            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateSpaceAndKeyword(TSqlTokenType.On);
            switch (node.SessionScope)
            {
                case EventSessionScope.Server:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Server);
                    break;
                case EventSessionScope.Database:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Database);
                    break;
                default:
                    throw new InvalidOperationException("Unexpected value for EventSessionScope: " + node.SessionScope.ToString());
            }
        }

        public void GenerateCommaSeparatedDropDeclarations<T>(IList<T> list, string declaration) where T : TSqlFragment
        {
            if (list != null)
            {
                Boolean first = true;

                foreach (T fragment in list)
                {
                    if (first)
                    {
                        NewLine();
                        GenerateKeywordAndSpace(TSqlTokenType.Drop);
                        GenerateIdentifier(declaration);
                        first = false;
                    }
                    else
                    {
                        GenerateKeyword(TSqlTokenType.Comma);
                        NewLine();
                        GenerateKeywordAndSpace(TSqlTokenType.Drop);
                        GenerateIdentifier(declaration);
                    }

                    GenerateSpaceAndFragmentIfNotNull(fragment);
                }
            }
        }
    }
}
