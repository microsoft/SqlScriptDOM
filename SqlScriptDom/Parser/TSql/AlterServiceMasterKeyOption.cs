//------------------------------------------------------------------------------
// <copyright file="AlterServiceMasterKeyOptions.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of alter service master key option
    /// </summary>       
    public enum AlterServiceMasterKeyOption
    {
        None            = 0,
        Regenerate      = 1,
        ForceRegenerate = 2,
        WithOldAccount  = 3,
        WithNewAccount  = 4,
    }

#pragma warning restore 1591
}
