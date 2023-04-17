//------------------------------------------------------------------------------
// <copyright file="ApplicationRoleOptionType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible application role options.
    /// </summary>
    public enum ApplicationRoleOptionKind
    {
        Name = 0,
        DefaultSchema = 1,
        Login = 2,
        Password = 3,
    }

#pragma warning restore 1591
}
