//------------------------------------------------------------------------------
// <copyright file="AutomaticTuningOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

	/// <summary>
	/// The possible Automatic Tuning Options under 'ALTER DATABASE d1 SET AUTOMATIC_TUNING = (...)'
	/// </summary>
	public enum AutomaticTuningOptionKind
	{
		Force_Last_Good_Plan,
		Create_Index,
		Drop_Index,
		Maintain_Index
	}


#pragma warning restore 1591
}
