//------------------------------------------------------------------------------
// <copyright file="SessionOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    public enum SessionOptionKind
    {
        EventRetention      = 0,
        MemoryPartition     = 1,
        MaxMemory           = 2,
        MaxEventSize        = 3,
        MaxDispatchLatency  = 4,
        TrackCausality      = 5,
        StartUpState        = 6,
    }

#pragma warning restore 1591
}
