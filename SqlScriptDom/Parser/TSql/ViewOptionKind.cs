//------------------------------------------------------------------------------
// <copyright file="ViewOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible View options.
    /// </summary>
    public enum ViewOptionKind
    {
        Encryption = 0,
        SchemaBinding = 1,
        ViewMetadata = 2,
        Distribution = 3,
        ForAppend = 4
    }

#pragma warning restore 1591

}
