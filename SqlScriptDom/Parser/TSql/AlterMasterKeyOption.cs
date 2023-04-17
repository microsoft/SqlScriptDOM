//------------------------------------------------------------------------------
// <copyright file="AlterMasterKeyOptions.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


using System;
namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of alter master key options
    /// </summary>   
    [Serializable]
    public enum AlterMasterKeyOption
    {
        None                            = 0,
        Regenerate                      = 1,
        ForceRegenerate                 = 2,
        AddEncryptionByServiceMasterKey = 3,
        AddEncryptionByPassword         = 4,
        DropEncryptionByServiceMasterKey= 5,
        DropEncryptionByPassword        = 6,
    }

#pragma warning restore 1591
}
