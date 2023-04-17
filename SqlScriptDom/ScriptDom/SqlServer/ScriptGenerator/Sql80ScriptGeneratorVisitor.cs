//------------------------------------------------------------------------------
// <copyright file="Sql80SqlScriptGeneratorVisitor.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    internal sealed class Sql80ScriptGeneratorVisitor : SqlScriptGeneratorVisitor
    {
        #region ctor

        public Sql80ScriptGeneratorVisitor(SqlScriptGeneratorOptions options, ScriptWriter writer)
            : base(options, writer)
        {
            options.SqlVersion = SqlVersion.Sql80;
        }

        #endregion

        #region CreateIndexStatement

        protected override void GenerateIndexOptions(IList<IndexOption> options)
        {
            if (options != null && options.Count > 0)
            {
                Boolean first = true;
                foreach (IndexOption option in options)
                {
                    IndexStateOption stateOption = option as IndexStateOption;
                    if (stateOption != null && stateOption.OptionState != OptionState.On)
                    {
                        continue;
                    }

                    if (first)
                    {
                        first = false;
                        NewLineAndIndent();
                        GenerateKeyword(TSqlTokenType.With);
                        GenerateSpace();
                    }
                    else
                    {
                        GenerateSymbolAndSpace(TSqlTokenType.Comma);
                    }

                    GenerateFragmentIfNotNull(option);
                }
            }
        }

        public override void ExplicitVisit(IndexStateOption node)
        {
            if (node.OptionState == OptionState.On)
            {
                IndexOptionHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            }
        }

        #endregion

        #region Statements that can't have semicolon

        // we don't want to generate semicolon for some statements
        private static HashSet<Type> _typesCantHaveSemiColon = new HashSet<Type>()
        {
            // it's a syntax error to put a semicolon at the end of these statements
            typeof(CreateViewStatement),
            typeof(AlterViewStatement),
            typeof(CreateFunctionStatement),
            typeof(AlterFunctionStatement),
            typeof(CreateDefaultStatement),
            typeof(CreateRuleStatement),
            typeof(CreateSchemaStatement),
            typeof(TSqlStatementSnippet), // this is introduced to preserve comments for sys comments objects, for CREATE VIEW, we can't have an ending simicolon
            // we don't want to generate an extra semicolon for these statements, since their bodies may have a semocolon
            typeof(CreateTriggerStatement),
            typeof(AlterTriggerStatement), 
            typeof(CreateProcedureStatement),
            typeof(AlterProcedureStatement),
            // we just don't want to put a semicolon after these statements
            typeof(BeginEndBlockStatement),
            typeof(IfStatement),
            typeof(WhileStatement),
            typeof(LabelStatement),
            typeof(TryCatchStatement),
        };

        internal override HashSet<Type> StatementsThatCannotHaveSemiColon
        {
            get { return _typesCantHaveSemiColon; }
        }

        #endregion

#if false

        #region keywords

        private HashSet<String> _keywordListForDebug = new HashSet<String>(StringComparer.OrdinalIgnoreCase)
        {
            "ADD",
            "ALL",
            "ALTER",
            "AND",
            "ANY",
            "AS",
            "ASC",
            "AUTHORIZATION",
            "BACKUP",
            "BEGIN",
            "BETWEEN",
            "BREAK",
            "BROWSE",
            "BULK",
            "BY",
            "CASCADE",
            "CASE",
            "CHECK",
            "CHECKPOINT",
            "CLOSE",
            "CLUSTERED",
            "COALESCE",
            "COLLATE",
            "COLUMN",
            "COMMIT",
            "COMPUTE",
            "CONSTRAINT",
            "CONTAINS",
            "CONTAINSTABLE",
            "CONTINUE",
            "CONVERT",
            "CREATE",
            "CROSS",
            "CURRENT",
            "CURRENT_DATE",
            "CURRENT_TIME",
            "CURRENT_TIMESTAMP",
            "CURRENT_USER",
            "CURSOR",
            "DATABASE",
            "DBCC",
            "DEALLOCATE",
            "DECLARE",
            "DEFAULT",
            "DELETE",
            "DENY",
            "DESC",
            "DISK",
            "DISTINCT",
            "DISTRIBUTED",
            "DOUBLE",
            "DROP",
            "DUMP",
            "ELSE",
            "END",
            "ERRLVL",
            "ESCAPE",
            "EXCEPT",
            "EXEC",
            "EXECUTE",
            "EXISTS",
            "EXIT",
            "EXTERNAL",
            "FETCH",
            "FILE",
            "FILLFACTOR",
            "FOR",
            "FOREIGN",
            "FREETEXT",
            "FREETEXTTABLE",
            "FROM",
            "FULL",
            "FUNCTION",
            "GO",
            "GOTO",
            "GRANT",
            "GROUP",
            "HAVING",
            "HOLDLOCK",
            "IDENTITY",
            "IDENTITYCOL",
            "IDENTITY_INSERT",
            "IF",
            "IN",
            "INDEX",
            "INNER",
            "INSERT",
            "INTERSECT",
            "INTO",
            "IS",
            "JOIN",
            "KEY",
            "KILL",
            "LEFT",
            "LIKE",
            "LINENO",
            "LOAD",
            "NATIONAL",
            "NOCHECK",
            "NONCLUSTERED",
            "NOT",
            "NULL",
            "NULLIF",
            "OF",
            "OFF",
            "OFFSETS",
            "ON",
            "OPEN",
            "OPENDATASOURCE",
            "OPENQUERY",
            "OPENROWSET",
            "OPENXML",
            "OPTION",
            "OR",
            "ORDER",
            "OUTER",
            "OVER",
            "PERCENT",
            "PIVOT",
            "PLAN",
            "PRECISION",
            "PRIMARY",
            "PRINT",
            "PROC",
            "PROCEDURE",
            "PUBLIC",
            "RAISERROR",
            "READ",
            "READTEXT",
            "RECONFIGURE",
            "REFERENCES",
            "REPLICATION",
            "RESTORE",
            "RESTRICT",
            "RETURN",
            "REVERT",
            "REVOKE",
            "RIGHT",
            "ROLLBACK",
            "ROWCOUNT",
            "ROWGUIDCOL",
            "RULE",
            "SAVE",
            "SCHEMA",
            "SELECT",
            "SESSION_USER",
            "SET",
            "SETUSER",
            "SHUTDOWN",
            "SOME",
            "STATISTICS",
            "SYSTEM_USER",
            "TABLE",
            "TABLESAMPLE",
            "TEXTSIZE",
            "THEN",
            "TO",
            "TOP",
            "TRAN",
            "TRANSACTION",
            "TRIGGER",
            "TRUNCATE",
            "TSEQUAL",
            "UNION",
            "UNIQUE",
            "UNPIVOT",
            "UPDATE",
            "UPDATETEXT",
            "USE",
            "USER",
            "VALUES",
            "VARYING",
            "VIEW",
            "WAITFOR",
            "WHEN",
            "WHERE",
            "WHILE",
            "WITH",
            "WRITETEXT",
        };

        /// <summary>
        /// Check if an identifier is a keyword. This should be used only for debug purpose
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        protected override Boolean IsKeyword(String identifier)
        {
            return _keywordListForDebug.Contains(identifier);
        }

        #endregion

#endif
    }
}
