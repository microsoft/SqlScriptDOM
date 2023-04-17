//------------------------------------------------------------------------------
// <copyright file="ProcedureOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


using System;
namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of procedure options
    /// </summary>        
    public enum ProcedureOptionKind
    {
        Encryption  = 0,
        Recompile   = 1,
        ExecuteAs   = 2,
        NativeCompilation = 3,
        SchemaBinding = 4,
    }

#pragma warning restore 1591
}
