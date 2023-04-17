//------------------------------------------------------------------------------
// <copyright file="BulkInsertOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// If this enum grows beyond 63, Parser needs to be updated - it uses 
    /// Int64 to check for option duplication
    /// </summary>
    public enum BulkInsertOptionKind
    {
        None            = 0,
        BatchSize       = 1,
        CheckConstraints= 2,
        CodePage        = 3,
        DataFileType    = 4,
        FieldTerminator = 5,
        FirstRow        = 6,
        FireTriggers    = 7,
        FormatFile      = 8,
        KeepIdentity    = 9,
        KeepNulls       = 10,
        KilobytesPerBatch=11,
        LastRow         = 12,
        MaxErrors       = 13,
        RowsPerBatch    = 14,
        RowTerminator   = 15,
        TabLock         = 16,
        ErrorFile       = 17,
        NoTriggers      = 18,
        // OpenRowset(BULK) - specific options
        SingleBlob      = 19,
        SingleClob      = 20,
        SingleNClob     = 21,
        //
        Order           = 22,
        IncludeHidden   = 23,
        // Support for external sources for data, format and error files, i.e. Azure Blob Storage
        DataSource = 24,
        FormatDataSource = 25,
        ErrorDataSource = 26,
        // CSV specific options
        DataFileFormat  = 27,
        FieldQuote      = 28,
        EscapeChar      = 29,
        // Synapse SQL Serverless specific options
        DataCompression = 30,
        ParserVersion   = 31,
        HeaderRow       = 32,
        RowsetOptions   = 33,
    }

#pragma warning restore 1591
}
