//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ColumnDefinition.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ColumnDefinition node)
        {
            const String NameAP = "name";
            const String TypeAP = "type";
            const String ConstraintAP = "constraint";

            MarkWhenNecessary(NameAP);
            GenerateFragmentIfNotNull(node.ColumnIdentifier);

            // firstConstraint is used to determine what kind of whitespace will be inserted
            // before a constraint. If true, then the constraint is the first one to be seen,
            // so a new alignment point is inserted (depending on user options). Otherwise, there have
            // been other constraints before the current one, so a space or newline is inserted
            // (depending on user options).
            //
            // Example:
            // col1 int     NOT NULL DEFAULT 3
            // col2 int     DEFAULT 5
            //
            // Here, the DEFAULT constraint is the first one for col2 but the second for col1
            // which affects the whitespace inserted before it.
            bool firstConstraint = true;

            MarkWhenNecessary(TypeAP);

            if (node.ComputedColumnExpression != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.As); // align AS with data types
                MarkForConstraintsWhenNecessary(ConstraintAP, ref firstConstraint); // treat expression as a constraint
                GenerateSpaceAndFragmentIfNotNull(node.ComputedColumnExpression);
            }
            else
            {
                GenerateSpaceAndFragmentIfNotNull(node.DataType);

                // VisitCollation does this check too, but we need to set seenOneConstraint
                if (node.Collation != null)
                {
                    MarkForConstraintsWhenNecessary(ConstraintAP, ref firstConstraint); // treat expression as a constraint
                    GenerateSpaceAndCollation(node.Collation);
                }
                
                if (node.GeneratedAlways != null)
                {
                    GenerateGeneratedAlways(node.GeneratedAlways);
                }

                // Storage options
                GenerateFragmentIfNotNull(node.StorageOptions);

                // Column Encryption
                GenerateSpaceAndFragmentIfNotNull(node.Encryption);

                if (node.IsMasked)
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

                // Write constraints
                if (node.DefaultConstraint != null)
                {
                    MarkForConstraintsWhenNecessary(ConstraintAP, ref firstConstraint);
                    GenerateSpaceAndFragmentIfNotNull(node.DefaultConstraint);
                }

                GenerateIdentity(node.IdentityOptions, ConstraintAP, ref firstConstraint);

                if (node.IsRowGuidCol)
                {
                    MarkForConstraintsWhenNecessary(ConstraintAP, ref firstConstraint);
                    GenerateSpaceAndKeyword(TSqlTokenType.RowGuidColumn); 
                }
            }

            if (node.IsPersisted)
            {
                Debug.Assert(node.ComputedColumnExpression != null);

                MarkForConstraintsWhenNecessary(ConstraintAP, ref firstConstraint);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Persisted); 
            }

            foreach (ConstraintDefinition constraint in node.Constraints)
            {
                MarkForConstraintsWhenNecessary(ConstraintAP, ref firstConstraint);
                GenerateSpaceAndFragmentIfNotNull(constraint);
            }

            if (node.Index != null)
            {
                MarkForConstraintsWhenNecessary(ConstraintAP, ref firstConstraint);
                GenerateSpaceAndFragmentIfNotNull(node.Index);
            }            

            // TODO, yangg: if constraint AP is still not marked, we can mark it here or don't.  
            // either way we have some imperfect effect...check out the expression and table-declare unit tests
            MarkForConstraintsWhenNecessary(ConstraintAP, ref firstConstraint);
        }

        public override void ExplicitVisit(ColumnStorageOptions node)
        {
                switch (node.SparseOption)
                {
                    case SparseColumnOption.ColumnSetForAllSparseColumns:
                        GenerateSpaceAndIdentifier(CodeGenerationSupporter.ColumnSet);
                        GenerateSpaceAndKeyword(TSqlTokenType.For);
                        GenerateSpaceAndIdentifier(CodeGenerationSupporter.AllSparseColumns);
                        break;
                    case SparseColumnOption.Sparse:
                        GenerateSpaceAndIdentifier(CodeGenerationSupporter.Sparse);
                        break;
                    case SparseColumnOption.None:
                        break;
                    default:
                        Debug.Assert(false, "Unknown enum value");
                        break;
                }

                if (node.IsFileStream)
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.FileStream);
        }

        private void GenerateIdentity(IdentityOptions node, String apName, ref Boolean firstConstraint)
        {
            if (node != null)
            {
                MarkForConstraintsWhenNecessary(apName, ref firstConstraint);

                GenerateSpaceAndKeyword(TSqlTokenType.Identity);

                if (node.IdentitySeed != null)
                {
                    GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
                    GenerateFragmentIfNotNull(node.IdentitySeed);
                    GenerateSymbolAndSpace(TSqlTokenType.Comma);
                    GenerateFragmentIfNotNull(node.IdentityIncrement);
                    GenerateSymbol(TSqlTokenType.RightParenthesis);
                }

                if (node.IsIdentityNotForReplication)
                {
                    GenerateSpace();
                    GenerateNotForReplication();
                }
            }
        }

        private void GenerateGeneratedAlways(GeneratedAlwaysType? node)
        {
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Generated);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Always);
            GenerateSpaceAndKeyword(TSqlTokenType.As);
        
            switch (node.Value)
            {
                case GeneratedAlwaysType.RowStart:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Row);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Start);
                    break;
                
                case GeneratedAlwaysType.RowEnd:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Row);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.End);
                    break;

                case GeneratedAlwaysType.UserIdStart:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.SuserSid);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Start);
                    break;

                case GeneratedAlwaysType.UserIdEnd:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.SuserSid);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.End);
                    break;

                case GeneratedAlwaysType.UserNameStart:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.SuserSname);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Start);
                    break;

                case GeneratedAlwaysType.UserNameEnd:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.SuserSname);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.End);
                    break;

                case GeneratedAlwaysType.TransactionIdStart:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.TransactionId);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Start);
                    break;

                case GeneratedAlwaysType.TransactionIdEnd:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.TransactionId);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.End);
                    break;

                case GeneratedAlwaysType.SequenceNumberStart:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.SequenceNumber);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Start);
                    break;

                case GeneratedAlwaysType.SequenceNumberEnd:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.SequenceNumber);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.End);
                    break;

                default:
                    Debug.Assert(false, "Unknown enum value");
                    break;
            }
        }
        
        private void MarkForConstraintsWhenNecessary(String apName, ref Boolean firstConstraint)
        {
            if (firstConstraint)
            {
                MarkWhenNecessary(apName);
                firstConstraint = false;
            }
        }

        private void MarkWhenNecessary(String apName)
        {
            if (_options.AlignColumnDefinitionFields)
            {
                AlignmentPoint ap = FindOrCreateAlignmentPointByName(apName);
#if !PIMODLANGUAGE
                Debug.Assert(ap != null, "invalid Alignment Point for" + apName);
#endif
                Mark(ap);
            }
        }
    }
}
