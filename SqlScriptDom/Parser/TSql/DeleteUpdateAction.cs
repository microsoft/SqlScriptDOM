//------------------------------------------------------------------------------
// <copyright file="DeleteUpdateAction.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
 
using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Shows the action that will be taken on delete or update.
    /// </summary>
    public enum DeleteUpdateAction
    {
        /// <summary>
        /// No action will be taken.
        /// </summary>
        NotSpecified = 0,
        /// <summary>
        /// Cascade action will be taken.
        /// </summary>
        Cascade = 1,
        /// <summary>
        /// Set null action will be taken.
        /// </summary>
        SetNull = 2,
        /// <summary>
        /// Set default action will be taken.
        /// </summary>
        SetDefault = 3,
        /// <summary>
        /// No action was specified.
        /// </summary>
        NoAction = 4,
    }
}
