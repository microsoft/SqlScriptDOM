//------------------------------------------------------------------------------
// <copyright file="AlterServerConfigurationBufferPoolExtensionOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of buffer pool extension options.
    /// </summary>       
    public enum AlterServerConfigurationBufferPoolExtensionOptionKind
    {
        None = 0,
        OnOff = 1,
        FileName = 2,
        Size = 3,
    }

#pragma warning disable 1591
}
