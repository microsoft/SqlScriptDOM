//------------------------------------------------------------------------------
// <copyright file="VersioningVisitor.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.Versioning
{
    /// <summary>
    /// The base versioning visitor for the concrete TSql fragments
    /// </summary>
    public partial class VersioningVisitor : TSqlConcreteFragmentVisitor
    {
        private SqlEngineType _targetEngineType;
        private SqlVersion _targetVersion;
        private List<ParseError> _errors;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersioningVisitor"/> class.
        /// </summary>
        public VersioningVisitor(SqlScriptGeneratorOptions options)
            : base()
        {
            this._targetEngineType = options.SqlEngineType;
            this._targetVersion = options.SqlVersion;
            _errors = new List<ParseError>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersioningVisitor"/> class.
        /// </summary>
        public VersioningVisitor(SqlEngineType engineType, SqlVersion version)
            : base()
        {
            this._targetEngineType = engineType;
            this._targetVersion = version;
            _errors = new List<ParseError>();
        }

        internal IList<ParseError> GetErrors()
        {
            return _errors;
        }

        private void AddVersioningError(int offset, int line, int column, string unsupportedStatement)
        {
            ParseError e = new ParseError(46117, // The script contains clause {0} which is not supported by the targeted version of SQL Server.
                offset,
                line,
                column,
                String.Format(CultureInfo.CurrentCulture, TSqlParserResource.SQL46117Message, unsupportedStatement));

            _errors.Add(e);
        }
    }
}
