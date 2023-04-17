//------------------------------------------------------------------------------
// <copyright file="ParameterlessCallType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
 
using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Specifies the type of the parameterless call.
    /// </summary>
    
    public enum ParameterlessCallType
    {
        /// <summary>
        /// The USER keyword was used.
        /// </summary>
        User = 0,
        /// <summary>
        /// The CURRENT_USER keyword was used.
        /// </summary>
        CurrentUser = 1,
        /// <summary>
        /// The SESSION_USER keyword was used.
        /// </summary>
        SessionUser = 2,
        /// <summary>
        /// The SYSTEM_USER keyword was used.
        /// </summary>
        SystemUser = 3,
        /// <summary>
        /// The CURRENT_TIMESTAMP keyword was used.
        /// </summary>
        CurrentTimestamp = 4
    }

#pragma warning restore 1591
}
