//------------------------------------------------------------------------------
// <copyright file="porttypes.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible values for port types
    /// </summary>            
    [Serializable, Flags]
    public enum PortTypes
    {
        None        = 0x00,
        Clear       = 0x01,
        Ssl         = 0x02
    }

#pragma warning restore 1591
}
