//------------------------------------------------------------------------------
// <copyright file="XmlForClauseOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class XmlForClauseOptionsHelper : OptionsHelper<XmlForClauseOptions>
    {
        private XmlForClauseOptionsHelper()
        {
            AddOptionMapping(XmlForClauseOptions.Elements, CodeGenerationSupporter.Elements);
            AddOptionMapping(XmlForClauseOptions.Root, CodeGenerationSupporter.Root);
            AddOptionMapping(XmlForClauseOptions.XmlSchema, CodeGenerationSupporter.XmlSchema);
            AddOptionMapping(XmlForClauseOptions.XmlData, CodeGenerationSupporter.XmlData);
            AddOptionMapping(XmlForClauseOptions.Type, CodeGenerationSupporter.Type);
        }

        internal static readonly XmlForClauseOptionsHelper Instance = new XmlForClauseOptionsHelper();
    }
}
