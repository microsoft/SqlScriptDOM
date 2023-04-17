//------------------------------------------------------------------------------
// <copyright file="DiskStatementOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class DiskStatementOptionsHelper : OptionsHelper<DiskStatementOptionKind>
    {
        private DiskStatementOptionsHelper()
        {
            AddOptionMapping(DiskStatementOptionKind.Name, CodeGenerationSupporter.Name);
            AddOptionMapping(DiskStatementOptionKind.PhysName, CodeGenerationSupporter.PhysName);
            AddOptionMapping(DiskStatementOptionKind.VDevNo, CodeGenerationSupporter.VDevNo);
            AddOptionMapping(DiskStatementOptionKind.Size, CodeGenerationSupporter.Size);
            AddOptionMapping(DiskStatementOptionKind.VStart, CodeGenerationSupporter.VStart);
            
        }

        internal static readonly DiskStatementOptionsHelper Instance = new DiskStatementOptionsHelper();
    }
}
