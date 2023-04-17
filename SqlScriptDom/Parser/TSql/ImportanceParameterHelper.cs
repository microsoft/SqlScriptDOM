//------------------------------------------------------------------------------
// <copyright file="ImportanceHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    [Serializable]
    internal class ImportanceParameterHelper : OptionsHelper<ImportanceParameterType>
    {
        private ImportanceParameterHelper()
        {
            AddOptionMapping(ImportanceParameterType.Low, CodeGenerationSupporter.Low);
            AddOptionMapping(ImportanceParameterType.High, CodeGenerationSupporter.High);
            AddOptionMapping(ImportanceParameterType.Medium, CodeGenerationSupporter.Medium);
            AddOptionMapping(ImportanceParameterType.Above_Normal, CodeGenerationSupporter.Above_Normal);
            AddOptionMapping(ImportanceParameterType.Below_Normal, CodeGenerationSupporter.Below_Normal);
            AddOptionMapping(ImportanceParameterType.Normal, CodeGenerationSupporter.Normal);
        }

        internal static readonly ImportanceParameterHelper Instance = new ImportanceParameterHelper();
    }
}
