//------------------------------------------------------------------------------
// <copyright file="PrincipalType.cs" company="Microsoft">
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
    /// The principal types that can be used in security statements.
    /// </summary>
    public enum PrincipalType
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
        /// Use the identifier.
        /// </summary>
        Identifier = 2,
    }

#pragma warning restore 1591
}
