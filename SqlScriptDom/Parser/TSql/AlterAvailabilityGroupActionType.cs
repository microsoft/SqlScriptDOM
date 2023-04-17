//------------------------------------------------------------------------------
// <copyright file="AlterAvailabilityGroupActionType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The types alter availability group actions
    /// </summary>       
    public enum AlterAvailabilityGroupActionType
    {
        /// <summary>
        /// Failover
        /// </summary>
        Failover                    = 0,
        /// <summary>
        /// Force failover with allowed data loss
        /// </summary>
        ForceFailoverAllowDataLoss  = 1,
        /// <summary>
        /// Take the availability group online
        /// </summary>
        Online                      = 2,
        /// <summary>
        /// Take the availability group offline
        /// </summary>
        Offline                     = 3,
        /// <summary>
        /// Join the availability group
        /// </summary>
        Join                        = 4,
    }
}
