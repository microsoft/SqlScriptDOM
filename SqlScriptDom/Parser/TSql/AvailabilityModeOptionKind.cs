//------------------------------------------------------------------------------
// <copyright file="AvailabilityModeOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The availability mode options.
    /// </summary>       
    public enum AvailabilityModeOptionKind
    {
        /// <summary>
        /// Synchronous commit
        /// </summary>
        SynchronousCommit  = 0,
        /// <summary>
        /// Asynchronous commit
        /// </summary>
        AsynchronousCommit = 1,
    }
}
