//------------------------------------------------------------------------------
// <copyright file="NullNotNull.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Represents optional NULL/NOT NULL contraint in column definition in INSERT BULK statement
    /// </summary>       
    public enum NullNotNull
    {
        NotSpecified    = 0,
        Null            = 1,
        NotNull         = 2
    }

#pragma warning restore 1591
}
