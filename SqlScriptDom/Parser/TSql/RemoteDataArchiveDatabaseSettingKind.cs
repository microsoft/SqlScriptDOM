//------------------------------------------------------------------------------
// <copyright file="RemoteDataArchiveDatabaseSettingKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	/// <summary>
	/// The possible settings under 'ALTER DATABASE d1 SET REMOTE_DATA_ARCHIVE = (...)'
	/// </summary>
	public enum RemoteDataArchiveDatabaseSettingKind
	{
		/// <summary>
		/// Represents the SERVER option in the REMOTE_DATA_ARCHIVE set options
		/// </summary>
		Server,

		/// <summary>
		/// Represents the CREDENTIAL option in the REMOTE_DATA_ARCHIVE set options
		/// </summary>
		Credential,

		/// <summary>
		/// Represents the FEDERATED_SERVICE_ACCOUNT option in the REMOTE_DATA_ARCHIVE set options
		/// </summary>
		FederatedServiceAccount,
	}
}
