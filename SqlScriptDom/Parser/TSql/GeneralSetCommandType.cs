//------------------------------------------------------------------------------
// <copyright file="GeneralSetCommandType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Types of set commands with parameter
    /// </summary>
    
    public enum GeneralSetCommandType
    {
        None        = 0,
        Language    = 1,
        DateFormat  = 2,
        DateFirst   = 3,
        DeadlockPriority = 4,
        // Fips Flagger is a separate statement
        LockTimeout = 5,
        QueryGovernorCostLimit = 6,
        ContextInfo = 7,
    }

#pragma warning restore 1591
}
