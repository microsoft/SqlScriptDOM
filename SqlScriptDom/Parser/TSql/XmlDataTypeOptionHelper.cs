//------------------------------------------------------------------------------
// <copyright file="XmlDataTypeOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Deals with DECLARE CURSOR options
    /// </summary>
    
    [Serializable]
    internal class XmlDataTypeOptionHelper : OptionsHelper<XmlDataTypeOption>
    {
        private XmlDataTypeOptionHelper()
        {
            AddOptionMapping(XmlDataTypeOption.Content, CodeGenerationSupporter.Content);
            AddOptionMapping(XmlDataTypeOption.Document, CodeGenerationSupporter.Document);
        }

        internal static readonly XmlDataTypeOptionHelper Instance = new XmlDataTypeOptionHelper();
    }
}