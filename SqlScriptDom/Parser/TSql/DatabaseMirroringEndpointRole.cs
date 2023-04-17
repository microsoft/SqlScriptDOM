//------------------------------------------------------------------------------
// <copyright file="DatabaseMirroringEndpointRoles.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of database mirroring endpoint role
    /// </summary>              
    public enum DatabaseMirroringEndpointRole
    {
        NotSpecified    = 0,
        Witness         = 1,
        Partner         = 2,
        All             = 3
    }

#pragma warning restore 1591
}
