//------------------------------------------------------------------------------
// <copyright file="BackupRestoreItemKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of backup restore item options
    /// </summary>       
    public enum BackupRestoreItemKind
    {
        None        = 0,
        Files       = 1,
        FileGroups  = 2,
        Page        = 3,
        ReadWriteFileGroups = 4
    }


#pragma warning restore 1591
}
