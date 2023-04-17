//------------------------------------------------------------------------------
// <copyright file="AlterIndexType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible alter index types.
    /// </summary>
    public enum AlterIndexType
    {
        Rebuild = 0,
        Disable = 1,
        Reorganize = 2,
        Set = 3,
        UpdateSelectiveXmlPaths = 4,
        Abort = 5,
        Pause = 6,
        Resume = 7
    }

#pragma warning restore 1591
}
