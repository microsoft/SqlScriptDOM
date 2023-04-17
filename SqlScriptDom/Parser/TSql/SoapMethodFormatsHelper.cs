//------------------------------------------------------------------------------
// <copyright file="SoapMethodFormatsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    [Serializable]
    internal class SoapMethodFormatsHelper : OptionsHelper<SoapMethodFormat>
    {
        private SoapMethodFormatsHelper()
        {
            AddOptionMapping(SoapMethodFormat.AllResults, CodeGenerationSupporter.AllResults);
            AddOptionMapping(SoapMethodFormat.RowsetsOnly, CodeGenerationSupporter.RowsetsOnly);
            AddOptionMapping(SoapMethodFormat.None, CodeGenerationSupporter.None);
        }
        internal static readonly SoapMethodFormatsHelper Instance = new SoapMethodFormatsHelper();
    }
}
