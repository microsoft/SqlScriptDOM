//------------------------------------------------------------------------------
// <copyright file="InsertOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	/// <summary>
	/// The type of insert options
	/// </summary>    
    public enum InsertOption
    {
        /// <summary>
        /// Nothing was defined.
        /// </summary>
        None = 0,
        /// <summary>
        /// INTO was used.
        /// </summary>
        Into = 1,
        /// <summary>
        /// OVER was used.
        /// </summary>
        Over = 2,
    }
}
