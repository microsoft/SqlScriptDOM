//------------------------------------------------------------------------------
// <copyright file="ExternalFileFormatOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The external file format options.
    /// </summary>
    public enum ExternalFileFormatOptionKind
    {
        SerDeMethod = 0,
        FormatOptions = 1,
        FieldTerminator = 2,
        StringDelimiter = 3,
        DateFormat = 4,
        UseTypeDefault = 5,
        DataCompression = 6,
        FirstRow = 7,
        Encoding = 8,
        ParserVersion = 9
    }

#pragma warning restore 1591
}
