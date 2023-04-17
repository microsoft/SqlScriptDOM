//------------------------------------------------------------------------------
// <copyright file="ParameterStyle.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Parameter style for external function bodies
    /// </summary>
    
	public enum ParameterStyle
    {
        None = 0,
        Sql = 1,
        General = 2,
    }

#pragma warning restore 1591
}
