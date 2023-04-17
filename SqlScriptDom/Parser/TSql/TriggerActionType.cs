//------------------------------------------------------------------------------
// <copyright file="TriggerActionType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Specify which data modification statements, 
    /// when attempted against this table or view, 
    /// activate the trigger.
    /// </summary>
    [Serializable]
    public enum TriggerActionType
    {
        /// <summary>
        /// Delete action.
        /// </summary>
        Delete = 0,
        /// <summary>
        /// Insert Action.
        /// </summary>
        Insert = 1,
        /// <summary>
        /// Update Action.
        /// </summary>
        Update = 2,
        /// <summary>
        /// Event.
        /// </summary>
        Event = 3,
        /// <summary>
        /// Logon Trigger.
        /// </summary>        
        LogOn = 4,
    }
}
