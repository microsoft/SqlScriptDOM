//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.WithOptionsList.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        // Scope of MultilineWithOptionsList (what these helpers do and do NOT cover):
        //
        // Covered (routed through these helpers):
        //   * Index options for every statement that funnels through the shared virtual
        //     GenerateIndexOptions (CREATE/ALTER INDEX, CREATE COLUMNSTORE/JSON/VECTOR INDEX,
        //     inline index definitions and UNIQUE/PRIMARY KEY constraints in CREATE TABLE, and
        //     ALTER TABLE ... ALTER INDEX / REBUILD).
        //   * CREATE SELECTIVE XML INDEX options, table hints (WITH), query hints (OPTION), and
        //     the non-parenthesized BACKUP / RESTORE WITH options.
        //
        // Intentionally NOT covered:
        //   * The Sql80ScriptGeneratorVisitor.GenerateIndexOptions override, which emits the legacy
        //     SQL Server 2000 "WITH opt, opt" (non-parenthesized) index-option syntax. It does not
        //     call these helpers, so the option is a no-op for that generator.
        //   * ALTER INDEX ... SET (...): SET is a distinct clause from WITH and is left single-line.
        //   * The non-selective CREATE XML INDEX and CREATE SPATIAL INDEX option lists, which emit
        //     their WITH (...) options through their own generators rather than the shared
        //     GenerateIndexOptions (only CREATE SELECTIVE XML INDEX is routed through these helpers).
        //   * Other WITH / option lists outside the list above, e.g. CREATE TABLE table options
        //     (MEMORY_OPTIMIZED, DATA_COMPRESSION, DISTRIBUTION), CREATE/ALTER PROCEDURE|FUNCTION|
        //     TRIGGER WITH options (ENCRYPTION, SCHEMABINDING, EXECUTE AS), CREATE STATISTICS WITH,
        //     FULLTEXT INDEX WITH, and DBCC ... WITH. CREATE EXTERNAL TABLE already has its own
        //     multiline handling and is not affected here. The WITH keyword of a common table
        //     expression and XMLNAMESPACES are unrelated constructs and are never touched.

        // Generates a parenthesized WITH / OPTION clause option list (index options, table hints,
        // query hints). When MultilineWithOptionsList is enabled the options are written one per
        // line inside the parentheses (honoring CommaPlacement and the parenthesis-placement
        // options); otherwise the original single-line parenthesized list is produced. Callers that
        // have not already written the separating space before the open parenthesis (for example
        // ALTER INDEX ... REBUILD, which historically emits WITH(...) with no space) pass
        // spaceBeforeSingleLineParenthesis = true so their single-line output is unchanged.
        protected void GenerateWithOptionsList<T>(IList<T> options, bool spaceBeforeSingleLineParenthesis) where T : TSqlFragment
        {
            if (options == null || options.Count == 0)
            {
                return;
            }

            if (_options.MultilineWithOptionsList)
            {
                GenerateFragmentList(options, ListGenerationOption.CreateOptionFromFormattingConfig(_options));
            }
            else
            {
                if (spaceBeforeSingleLineParenthesis)
                {
                    GenerateSpace();
                }

                GenerateParenthesisedCommaSeparatedList(options);
            }
        }

        // Generates a non-parenthesized WITH clause option list (BACKUP / RESTORE options). The
        // caller has already written the WITH keyword. When MultilineWithOptionsList is enabled each
        // option is written on its own line indented one level from the statement keyword (aligned
        // beneath the WITH keyword), honoring CommaPlacement; otherwise the options remain on a
        // single line following the WITH keyword.
        protected void GenerateWithOptionsListNonParenthesized<T>(IList<T> options) where T : TSqlFragment
        {
            if (options == null || options.Count == 0)
            {
                return;
            }

            if (_options.MultilineWithOptionsList)
            {
                // Same layout as procedure parameters: non-parenthesized, one option per line,
                // indented one level, with the leading new line produced by the option itself.
                GenerateFragmentList(options, ListGenerationOption.MultipleLineProcedureParameterOption);
            }
            else
            {
                GenerateSpace();
                GenerateCommaSeparatedList(options);
            }
        }
    }
}
