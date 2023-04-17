//------------------------------------------------------------------------------
// <copyright file="AffinityKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

#pragma warning disable 1591

	/// <summary>
	/// The types of endpoint affinity 
	/// </summary>       
    public enum AffinityKind
    {
        NotSpecified    = 0,
        None            = 1,
        Integer         = 2,
        Admin           = 3
    }

#pragma warning restore 1591
}
