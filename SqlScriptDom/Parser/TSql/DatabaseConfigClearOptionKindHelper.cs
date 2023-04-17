//------------------------------------------------------------------------------
// <copyright file="DatabaseConfigClearOptionKindHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class DatabaseConfigClearOptionKindHelper : OptionsHelper<DatabaseConfigClearOptionKind>
    {
        private DatabaseConfigClearOptionKindHelper()
        {
            // 130 Options
            AddOptionMapping(DatabaseConfigClearOptionKind.ProcedureCache, CodeGenerationSupporter.ProcedureCache, SqlVersionFlags.TSql130AndAbove);
        }

        internal static readonly DatabaseConfigClearOptionKindHelper Instance = new DatabaseConfigClearOptionKindHelper();
    }
}
