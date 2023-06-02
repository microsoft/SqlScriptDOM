//------------------------------------------------------------------------------
// <copyright file="DeviceTypes.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of devices
    /// </summary>               
    public enum DeviceType
    {
        None = 0,
        Disk = 1,
        Tape = 2,
        VirtualDevice = 3,
        DatabaseSnapshot = 4,
        Url = 5
    }

#pragma warning restore 1591
}
