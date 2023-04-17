//------------------------------------------------------------------------------
// <copyright file="JsonForClauseModeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class JsonForClauseModeHelper : OptionsHelper<JsonForClauseOptions>
    {
        private JsonForClauseModeHelper()
        {
            AddOptionMapping(JsonForClauseOptions.Auto, CodeGenerationSupporter.Auto);
            AddOptionMapping(JsonForClauseOptions.Path, CodeGenerationSupporter.Path);
        }

        internal static readonly JsonForClauseModeHelper Instance = new JsonForClauseModeHelper();
    }
}