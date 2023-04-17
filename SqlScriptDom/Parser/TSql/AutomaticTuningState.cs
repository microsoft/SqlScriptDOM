//------------------------------------------------------------------------------
// <copyright file="optionstate.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

	/// <summary>
	/// State of Automatic Tuning option
	/// </summary>
	public enum AutomaticTuningState
	{
		NotSet = 0,
		Inherit = 1,
		Auto = 2,
		Custom = 3
	}

#pragma warning restore 1591
}
