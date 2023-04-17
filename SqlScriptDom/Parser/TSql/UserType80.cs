//------------------------------------------------------------------------------
// <copyright file="UserType80.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
 
using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The privilege types that can be used in security statements.
    /// </summary>
    [Serializable]
    public enum UserType80
    {
        /// <summary>
        /// Null was used.
        /// </summary>
        Null = 0,
        /// <summary>
        /// Public was used.
        /// </summary>
        Public = 1,
        /// <summary>
        /// Use the identifier list on the related class.
        /// </summary>
        Users = 2,
    }
}
