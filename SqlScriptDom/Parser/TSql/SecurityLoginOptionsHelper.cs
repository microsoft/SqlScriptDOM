//------------------------------------------------------------------------------
// <copyright file="SecurityLoginOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class SecurityLoginOptionsHelper : OptionsHelper<PrincipalOptionKind>
    {
        private SecurityLoginOptionsHelper()
        {
            AddOptionMapping(PrincipalOptionKind.CheckExpiration, CodeGenerationSupporter.CheckExpiration);
            AddOptionMapping(PrincipalOptionKind.CheckPolicy, CodeGenerationSupporter.CheckPolicy);
        }

        internal static readonly SecurityLoginOptionsHelper Instance = new SecurityLoginOptionsHelper();
    }
}
