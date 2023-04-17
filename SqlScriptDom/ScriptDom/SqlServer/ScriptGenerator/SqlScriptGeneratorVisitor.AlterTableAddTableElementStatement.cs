//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterTableAddTableElementStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterTableAddTableElementStatement node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            GenerateAlterTableHead(node);

            if (node.ExistingRowsCheckEnforcement != ConstraintEnforcement.NotSpecified)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateConstraintEnforcement(node.ExistingRowsCheckEnforcement);
            }

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.Add);
            GenerateSpace();

            AlignmentPoint addedElements = new AlignmentPoint();
            MarkAndPushAlignmentPoint(addedElements);

            GenerateCommaSeparatedList(node.Definition.ColumnDefinitions, true);

            if (node.Definition.ColumnDefinitions.Count > 0 && node.Definition.TableConstraints.Count > 0)
            {
                GenerateSymbolAndSpace(TSqlTokenType.Comma);
                NewLine();
            }

            if (node.Definition.TableConstraints.Count > 0)
            {
                // could be
                //      CheckConstraint
                //      DefaultConstraint
                //      ForeignKeyConstraint
                //      NullableConstraint
                //      UniqueConstraint
                GenerateCommaSeparatedList(node.Definition.TableConstraints, true);
            }

            if ( (node.Definition.ColumnDefinitions.Count > 0 || node.Definition.TableConstraints.Count > 0)
                    && node.Definition.SystemTimePeriod != null)
            {
                GenerateSymbolAndSpace(TSqlTokenType.Comma);
                NewLine();
            }

            if (node.Definition.SystemTimePeriod != null)
            {
                ExplicitVisit((SystemTimePeriodDefinition)node.Definition.SystemTimePeriod);
            }

            if (node.Definition.Indexes != null && node.Definition.Indexes.Count > 0)
            {
                GenerateCommaSeparatedList(node.Definition.Indexes, true);
            }

            PopAlignmentPoint();

            PopAlignmentPoint();
        }
    }
}
