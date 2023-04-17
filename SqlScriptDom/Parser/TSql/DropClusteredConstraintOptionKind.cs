//------------------------------------------------------------------------------
// <copyright file="DropClusteredConstraintOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
 
using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Drop clustered constraint option types.
    /// </summary>
    public enum DropClusteredConstraintOptionKind
    {
        MaxDop = 0,
        Online = 1,
        MoveTo = 2,
        WaitAtLowPriority = 3
    }

#pragma warning restore 1591
}
