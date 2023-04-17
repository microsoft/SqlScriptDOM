//------------------------------------------------------------------------------
// <copyright file="AuthenticationTypesHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class AuthenticationTypesHelper : OptionsHelper<AuthenticationTypes>
    {
        private AuthenticationTypesHelper()
        {
            AddOptionMapping(AuthenticationTypes.Basic, CodeGenerationSupporter.Basic);
            AddOptionMapping(AuthenticationTypes.Digest, CodeGenerationSupporter.Digest);
            AddOptionMapping(AuthenticationTypes.Integrated, CodeGenerationSupporter.Integrated);
            AddOptionMapping(AuthenticationTypes.Kerberos, CodeGenerationSupporter.Kerberos);
            AddOptionMapping(AuthenticationTypes.Ntlm, CodeGenerationSupporter.Ntlm);
        }

        internal static readonly AuthenticationTypesHelper Instance = new AuthenticationTypesHelper();
    }
}
