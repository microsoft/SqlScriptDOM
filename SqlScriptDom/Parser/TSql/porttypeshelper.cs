//------------------------------------------------------------------------------
// <copyright file="porttypeshelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    [Serializable]
    internal partial class PortTypesHelper : OptionsHelper<PortTypes>
    {
        private PortTypesHelper()
        {
            AddOptionMapping(PortTypes.Clear, CodeGenerationSupporter.Clear);
            AddOptionMapping(PortTypes.Ssl, CodeGenerationSupporter.Ssl);
        }
        internal static readonly PortTypesHelper Instance = new PortTypesHelper();
    }
}
