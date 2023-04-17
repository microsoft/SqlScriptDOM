//------------------------------------------------------------------------------
// <copyright file="AbortAfterWaitTypeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class AbortAfterWaitTypeHelper : OptionsHelper<AbortAfterWaitType>
    {
        private AbortAfterWaitTypeHelper()
        {
            AddOptionMapping(AbortAfterWaitType.None, CodeGenerationSupporter.None);
            AddOptionMapping(AbortAfterWaitType.Blockers, CodeGenerationSupporter.Blockers);
            AddOptionMapping(AbortAfterWaitType.Self, CodeGenerationSupporter.Self);
        }

        public static readonly AbortAfterWaitTypeHelper Instance = new AbortAfterWaitTypeHelper();
    }
}
