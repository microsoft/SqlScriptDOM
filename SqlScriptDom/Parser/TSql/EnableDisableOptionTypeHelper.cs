//------------------------------------------------------------------------------
// <copyright file="EnableDisableOptionTypeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class EnableDisableOptionTypeHelper : OptionsHelper<EnableDisableOptionType>
    {
        private EnableDisableOptionTypeHelper()
        {
            AddOptionMapping(EnableDisableOptionType.Enable, CodeGenerationSupporter.Enable);
            AddOptionMapping(EnableDisableOptionType.Disable, CodeGenerationSupporter.Disable);
        }

        internal static readonly EnableDisableOptionTypeHelper Instance = new EnableDisableOptionTypeHelper();
    }
}
