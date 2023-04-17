//------------------------------------------------------------------------------
// <copyright file="OpenRowsetCosmosOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Available options for OPENROWSET (Provider =' CosmosDB' ....) statement.
	/// This syntax is specific to Serverless SQL pools
    /// </summary>
    public enum OpenRowsetCosmosOptionKind
    {
        Provider = 0,
        Connection = 1,
        Object = 2,
        Credential = 3,
        Server_Credential = 4,
    }


#pragma warning restore 1591
}
