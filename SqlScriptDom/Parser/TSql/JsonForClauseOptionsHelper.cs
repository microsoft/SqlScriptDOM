//------------------------------------------------------------------------------
// <copyright file="JsonForClauseOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    internal class JsonForClauseOptionsHelper : OptionsHelper<JsonForClauseOptions>
    {
        private JsonForClauseOptionsHelper()
        {
            AddOptionMapping(JsonForClauseOptions.Root, CodeGenerationSupporter.Root);
            AddOptionMapping(JsonForClauseOptions.IncludeNullValues, CodeGenerationSupporter.IncludeNullValues);
            AddOptionMapping(JsonForClauseOptions.WithoutArrayWrapper, CodeGenerationSupporter.WithoutArrayWrapper);
        }

        internal static readonly JsonForClauseOptionsHelper Instance = new JsonForClauseOptionsHelper();
    }
}