//------------------------------------------------------------------------------
// <copyright file="TriggerType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Specifies the type of the trigger.
    /// </summary>
    [Serializable]
    public enum TriggerType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// For behaves exactly as After.
        /// </summary>        
        For = 1,
        /// <summary>
        /// Specifies that the trigger is fired only when all 
        /// operations specified in the triggering SQL statement 
        /// have executed successfully. All referential cascade 
        /// actions and constraint checks also must succeed before 
        /// this trigger executes.
        /// </summary>
        After = 2,
        /// <summary>
        /// Specifies that the trigger is executed instead of the 
        /// triggering SQL statement, thus overriding the actions 
        /// of the triggering statements.
        /// </summary>
        InsteadOf = 3,
    }
}
