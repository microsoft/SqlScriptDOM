//------------------------------------------------------------------------------
// <copyright file="FailoverModeOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The failover mode options.
    /// </summary>       
    public enum FailoverModeOptionKind
    {
        /// <summary>
        /// Automatic
        /// </summary>
        Automatic   = 0,
        /// <summary>
        /// Manual
        /// </summary>
        Manual      = 1,
    }
}
