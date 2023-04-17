//------------------------------------------------------------------------------
// <copyright file="ExecuteAsOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The execute as options.
    /// </summary>
    public enum ExecuteAsOption
    {
        Caller = 0,
        Self = 1,
        Owner = 2,
        String = 3, // Login or User - in some cases, not specified explicitly
        Login = 4,
        User = 5,
    }

#pragma warning restore 1591
}
