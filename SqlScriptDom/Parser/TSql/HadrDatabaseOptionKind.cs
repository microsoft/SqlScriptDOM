//------------------------------------------------------------------------------
// <copyright file="HadrDatabaseOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The types of Hadr database options
    /// </summary>   
    public enum HadrDatabaseOptionKind
    {
        /// <summary>
        /// Suspend
        /// </summary>
        Suspend             = 0,
        /// <summary>
        /// Resume
        /// </summary>
        Resume              = 1,
        /// <summary>
        /// Off
        /// </summary>
        Off                 = 2,
        /// <summary>
        /// Availability group
        /// </summary>
        AvailabilityGroup   = 3,
    }
}
