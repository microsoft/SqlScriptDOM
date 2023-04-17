//------------------------------------------------------------------------------
// <copyright file="CommandOptions.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Commands that can be used in security statements.
    /// </summary>
    [Flags]
    public enum CommandOptions // : long
    {
        None                = 0x0000,
        CreateDatabase      = 0x0001,
        CreateDefault       = 0x0002,
        CreateProcedure     = 0x0004,
        CreateFunction      = 0x0008,
        CreateRule          = 0x0010,
        CreateTable         = 0x0020,
        CreateView          = 0x0040,
        BackupDatabase      = 0x0080,
        BackupLog           = 0x0100,


        //AlterDatabase       = 0x0001,
        //AlterTable          = 0x0002,
        //BeginTransaction    = 0x0004,
        //Checkpoint          = 0x0008,
        //CommitTransaction   = 0x0010,
        //CreateDatabase      = 0x0020,
        //CreateDefault       = 0x0040,
        //CreateIndex         = 0x0080,
        //CreateProcedure     = 0x0100,
        //CreateFunction      = 0x0200,
        //CreateRule          = 0x0400,
        //CreateTable         = 0x0800,
        //CreateTrigger       = 0x1000,
        //CreateView          = 0x2000,
        //Dbcc                = 0x4000,
        //Disk                = 0x8000,
        //Drop                = 0x10000,
        //BackupDatabase      = 0x20000,
        //BackupLog           = 0x40000,
        //BackupTable         = 0x80000,
        //BackupTransaction   = 0x100000,
        //Grant               = 0x200000,
        //Kill                = 0x400000,
        //LoadDatabase        = 0x800000,
        //LoadIdentifier      = 0x1000000, // ask this
        //LoadTransaction     = 0x2000000,
        //Print               = 0x4000000,
        //Raiserror           = 0x8000000,
        //Reconfigure         = 0x10000000,
        //Revoke              = 0x20000000,
        //RollbackTransaction = 0x40000000,
        //SaveTransaction     = 0x80000000,
        //Set                 = 0x100000000,
        //SetUser             = 0x200000000,
        //TruncateTable       = 0x400000000,
        //UpdateStatistics    = 0x800000000,
        //WaitFor             = 0x1000000000,
        //Deny                = 0x2000000000
    }

#pragma warning restore 1591
}
