//------------------------------------------------------------------------------
// <copyright file="modifyfilegroupoptions.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible values for modify filegroup options
    /// </summary>        
    public enum ModifyFileGroupOption
    {
        None = 0,
        ReadWrite = 1,
        ReadWriteOld = 2,
        ReadOnly = 3,
        ReadOnlyOld = 4,
        AutogrowAllFiles = 5,
        AutogrowSingleFile = 6,
    }

#pragma warning restore 1591
}
