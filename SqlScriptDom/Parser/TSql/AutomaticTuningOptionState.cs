//------------------------------------------------------------------------------
// <copyright file="AutomaticTuningOptionState.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

	/// <summary>
	/// The possible Automatic Tuning Options under 'ALTER DATABASE d1 SET AUTOMATIC_TUNING = (OPTION = ...)'
	/// </summary>
	public enum AutomaticTuningOptionState
	{
		Off,
		On,
		Default
	}


#pragma warning restore 1591
}
