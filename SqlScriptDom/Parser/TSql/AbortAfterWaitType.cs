//------------------------------------------------------------------------------
// <copyright file="AbortAfterWaitType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The action to be taken after low priority wait times out.
    /// </summary>
    public enum AbortAfterWaitType
    {
        /// <summary>
        /// After timeout enter the normal wait queue.
        /// </summary>
        None = 0,

        /// <summary>
        /// After timeout abort blocking transactions.
        /// </summary>
        Blockers = 1,

        /// <summary>
        /// After timeout abort the statement itself.
        /// </summary>
        Self = 2,
    }
}
