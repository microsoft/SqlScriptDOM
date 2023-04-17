//------------------------------------------------------------------------------
// <copyright file="RemoteDataArchiveDatabaseSettingsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	/// <summary>
	/// Helper Class for REMOTE_DATA_ARCHIVE options
	/// </summary>

	[Serializable]
	internal class RemoteDataArchiveDatabaseSettingsHelper : OptionsHelper<RemoteDataArchiveDatabaseSettingKind>
	{
		private RemoteDataArchiveDatabaseSettingsHelper()
		{
			AddOptionMapping(RemoteDataArchiveDatabaseSettingKind.Server, CodeGenerationSupporter.Server);
			AddOptionMapping(RemoteDataArchiveDatabaseSettingKind.Credential, CodeGenerationSupporter.Credential);
			AddOptionMapping(RemoteDataArchiveDatabaseSettingKind.FederatedServiceAccount, CodeGenerationSupporter.FederatedServiceAccount);
		}

		internal static readonly RemoteDataArchiveDatabaseSettingsHelper Instance = new RemoteDataArchiveDatabaseSettingsHelper();
	}
}