//------------------------------------------------------------------------------
// <copyright file="CopyOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Purpose:
// Enum for copy command options.
//
namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// If this enum grows beyond 31, Parser needs to be updated - it uses 
    /// Int32 to check for option duplication
    /// </summary>
#pragma warning disable 1591
    public enum CopyOptionKind
    {
        File_Format = 1,
        File_Type = 2,
        ErrorFile = 3,
        Identity_Insert = 4,
        MaxErrors = 5,
        Compression = 6,
        FieldQuote = 7,
        FieldTerminator = 8,
        RowTerminator = 9,
        FirstRow = 10,
        DateFormat = 11,
        Encoding = 12,
        ColumnOptions = 13,
        Credential = 14,
        ErrorFileCredential = 15
    }
#pragma warning restore 1591
}
