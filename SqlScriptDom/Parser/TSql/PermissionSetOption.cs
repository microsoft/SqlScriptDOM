//------------------------------------------------------------------------------
// <copyright file="PermissionSetOption.cs" company="Microsoft">
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
    /// The permission set options.
    /// </summary>
    [Serializable]
    public enum PermissionSetOption
    {
        /// <summary>
        /// Nothing was defined.
        /// </summary>
        None = 0,
        /// <summary>
        /// SAFE was used.
        /// </summary>
        Safe = 1,
        /// <summary>
        /// EXTERNAL_ACCESS was used.
        /// </summary>
        ExternalAccess = 2,
        /// <summary>
        /// UNSAFE was used.
        /// </summary>
        Unsafe = 3
    }

#pragma warning restore 1591
}
