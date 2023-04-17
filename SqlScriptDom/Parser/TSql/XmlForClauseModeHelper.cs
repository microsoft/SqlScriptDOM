//------------------------------------------------------------------------------
// <copyright file="XmlForClauseModeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class XmlForClauseModeHelper : OptionsHelper<XmlForClauseOptions>
    {
        private XmlForClauseModeHelper()
        {
            AddOptionMapping(XmlForClauseOptions.Auto, CodeGenerationSupporter.Auto);
            AddOptionMapping(XmlForClauseOptions.Raw, CodeGenerationSupporter.Raw);
            AddOptionMapping(XmlForClauseOptions.Explicit, CodeGenerationSupporter.Explicit);
            AddOptionMapping(XmlForClauseOptions.Path, CodeGenerationSupporter.Path);
        }

        internal static readonly XmlForClauseModeHelper Instance = new XmlForClauseModeHelper();
    }
}
