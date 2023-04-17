//------------------------------------------------------------------------------
// <copyright file="SecondaryXmlIndexTypeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    [Serializable]
    internal class SecondaryXmlIndexTypeHelper : OptionsHelper<SecondaryXmlIndexType>
    {
        private SecondaryXmlIndexTypeHelper()
        {
            AddOptionMapping(SecondaryXmlIndexType.Path, CodeGenerationSupporter.Path);
            AddOptionMapping(SecondaryXmlIndexType.Property, CodeGenerationSupporter.Property);
            AddOptionMapping(SecondaryXmlIndexType.Value, CodeGenerationSupporter.Value);
        }

        internal static readonly SecondaryXmlIndexTypeHelper Instance = new SecondaryXmlIndexTypeHelper();
    }
}
