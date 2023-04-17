//------------------------------------------------------------------------------
// <copyright file="RecoveryDbOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    [Serializable]
    internal class RecoveryDbOptionsHelper : OptionsHelper<RecoveryDatabaseOptionKind>
    {
        private RecoveryDbOptionsHelper()
        {
            AddOptionMapping(RecoveryDatabaseOptionKind.Full, CodeGenerationSupporter.Full);
            AddOptionMapping(RecoveryDatabaseOptionKind.BulkLogged, CodeGenerationSupporter.BulkLogged);
            AddOptionMapping(RecoveryDatabaseOptionKind.Simple, CodeGenerationSupporter.Simple);
        }

        internal static readonly RecoveryDbOptionsHelper Instance = new RecoveryDbOptionsHelper();
    }
}
