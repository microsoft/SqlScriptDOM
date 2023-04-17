//------------------------------------------------------------------------------
// <copyright file="AvailabilityReplicaOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The possible Availability Replica Options.
    /// </summary>
    public enum AvailabilityReplicaOptionKind
    {
        /// <summary>
        /// Availability mode
        /// </summary>
        AvailabilityMode    = 0,
        /// <summary>
        /// Failover mode
        /// </summary>
        FailoverMode        = 1,
        /// <summary>
        /// Endpoint url
        /// </summary>
        EndpointUrl         = 2,
        /// <summary>
        /// Secondary role
        /// </summary>
        SecondaryRole       = 3,
        /// <summary>
        /// Session timeout
        /// </summary>
        SessionTimeout      = 4,
        /// <summary>
        /// Apply delay
        /// </summary>
        ApplyDelay          = 5,
        /// <summary>
        /// Primary role
        /// </summary>
        PrimaryRole         = 6,

    }
}
