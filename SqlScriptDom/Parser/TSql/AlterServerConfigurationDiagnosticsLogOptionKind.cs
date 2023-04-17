//------------------------------------------------------------------------------
// <copyright file="AlterServerConfigurationDiagnosticsLogOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of diagnostics log options.
    /// </summary>       
    public enum AlterServerConfigurationDiagnosticsLogOptionKind
    {
        None  = 0,
        OnOff = 1,
        Path = 2,
        MaxSize = 3,
        MaxFiles = 4,
    }

#pragma warning restore 1591
}
