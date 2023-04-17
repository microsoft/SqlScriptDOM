//------------------------------------------------------------------------------
// <copyright file="TriggerEnforcement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Options on enforcement of trigger execution.
    /// </summary>
    [Serializable]
    public enum TriggerEnforcement
    {
        /// <summary>
        /// Trigger is not evaluated.
        /// </summary>
        Disable = 0,
        /// <summary>
        /// Trigger is evaluated.
        /// </summary>
        Enable = 1
    }
}
