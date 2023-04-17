//------------------------------------------------------------------------------
// <copyright file="RaiseErrorOptions.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// RaiseError options.
    /// </summary>

    [Flags]
    [Serializable]
    public enum RaiseErrorOptions
    {
        None     =   0x0000,
        Log      =   0x0001,
        NoWait   =   0x0002,
        SetError =   0x0004,
    }

#pragma warning restore 1591
}
