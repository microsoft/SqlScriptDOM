//------------------------------------------------------------------------------
// <copyright file="IdentifierCreateLoginOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class IdentifierCreateLoginOptionsHelper : OptionsHelper<PrincipalOptionKind>
    {
        private IdentifierCreateLoginOptionsHelper()
        {
            AddOptionMapping(PrincipalOptionKind.DefaultDatabase, CodeGenerationSupporter.DefaultDatabase);
            AddOptionMapping(PrincipalOptionKind.DefaultLanguage, CodeGenerationSupporter.DefaultLanguage);
            AddOptionMapping(PrincipalOptionKind.Credential, CodeGenerationSupporter.Credential);
        }

        internal readonly static IdentifierCreateLoginOptionsHelper Instance = new IdentifierCreateLoginOptionsHelper();
    }
}
