//------------------------------------------------------------------------------
// <copyright file="BulkInsertStringOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class BulkInsertStringOptionsHelper : OptionsHelper<BulkInsertOptionKind>
    {
        private BulkInsertStringOptionsHelper()
        {
            AddOptionMapping(BulkInsertOptionKind.FieldTerminator, CodeGenerationSupporter.FieldTerminator);
            AddOptionMapping(BulkInsertOptionKind.RowTerminator, CodeGenerationSupporter.RowTerminator);
            AddOptionMapping(BulkInsertOptionKind.FormatFile, CodeGenerationSupporter.FormatFile);
            AddOptionMapping(BulkInsertOptionKind.ErrorFile, CodeGenerationSupporter.ErrorFile);
            AddOptionMapping(BulkInsertOptionKind.CodePage, CodeGenerationSupporter.CodePage);
            AddOptionMapping(BulkInsertOptionKind.DataFileType, CodeGenerationSupporter.DataFileType);
            AddOptionMapping(BulkInsertOptionKind.DataSource, CodeGenerationSupporter.DataSource);
            AddOptionMapping(BulkInsertOptionKind.FormatDataSource, CodeGenerationSupporter.FormatDataSource);
            AddOptionMapping(BulkInsertOptionKind.ErrorDataSource, CodeGenerationSupporter.ErrorDataSource);
            AddOptionMapping(BulkInsertOptionKind.DataFileFormat, CodeGenerationSupporter.Format, SqlVersionFlags.TSql140AndAbove);
            AddOptionMapping(BulkInsertOptionKind.FieldQuote, CodeGenerationSupporter.FieldQuote, SqlVersionFlags.TSql140AndAbove);
            AddOptionMapping(BulkInsertOptionKind.EscapeChar, CodeGenerationSupporter.EscapeChar, SqlVersionFlags.TSql150AndAbove);
            AddOptionMapping(BulkInsertOptionKind.DataCompression, CodeGenerationSupporter.DataCompression, SqlVersionFlags.TSql150AndAbove);
            AddOptionMapping(BulkInsertOptionKind.ParserVersion, CodeGenerationSupporter.ParserVersion, SqlVersionFlags.TSql150AndAbove);
            AddOptionMapping(BulkInsertOptionKind.HeaderRow, CodeGenerationSupporter.HeaderRow, SqlVersionFlags.TSql150AndAbove);
            AddOptionMapping(BulkInsertOptionKind.RowsetOptions, CodeGenerationSupporter.RowsetOptions, SqlVersionFlags.TSql150AndAbove);
        }

        internal static readonly BulkInsertStringOptionsHelper Instance = new BulkInsertStringOptionsHelper();
    }
}
