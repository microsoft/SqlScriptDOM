//------------------------------------------------------------------------------
// <copyright file="RestoreStatementKindsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    [Serializable]
    internal class RestoreStatementKindsHelper : OptionsHelper<RestoreStatementKind>
    {
        private RestoreStatementKindsHelper()
        {
            AddOptionMapping(RestoreStatementKind.FileListOnly, CodeGenerationSupporter.FileListOnly);
            AddOptionMapping(RestoreStatementKind.VerifyOnly, CodeGenerationSupporter.VerifyOnly);
            AddOptionMapping(RestoreStatementKind.LabelOnly, CodeGenerationSupporter.LabelOnly);
            AddOptionMapping(RestoreStatementKind.RewindOnly, CodeGenerationSupporter.RewindOnly);
            AddOptionMapping(RestoreStatementKind.HeaderOnly, CodeGenerationSupporter.HeaderOnly);
        }

        internal static readonly RestoreStatementKindsHelper Instance = new RestoreStatementKindsHelper();
    }
}
