//------------------------------------------------------------------------------
// <copyright file="AlterIndexTypeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class AlterIndexTypeHelper : OptionsHelper<AlterIndexType>
    {
        private AlterIndexTypeHelper()
        {
            AddOptionMapping(AlterIndexType.Disable, CodeGenerationSupporter.Disable);
            AddOptionMapping(AlterIndexType.Rebuild, CodeGenerationSupporter.Rebuild);
            AddOptionMapping(AlterIndexType.Reorganize, CodeGenerationSupporter.Reorganize);
            AddOptionMapping(AlterIndexType.Set, TSqlTokenType.Set);
            AddOptionMapping(AlterIndexType.Abort, CodeGenerationSupporter.Abort, SqlVersionFlags.TSql140AndAbove);
            AddOptionMapping(AlterIndexType.Pause, CodeGenerationSupporter.Pause, SqlVersionFlags.TSql140AndAbove);
            AddOptionMapping(AlterIndexType.Resume, CodeGenerationSupporter.Resume, SqlVersionFlags.TSql140AndAbove);
        }

        internal static readonly AlterIndexTypeHelper Instance = new AlterIndexTypeHelper();
    }
}
