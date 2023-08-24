//------------------------------------------------------------------------------
// <copyright file="UserOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    [Serializable]
    internal class UserOptionHelper : OptionsHelper<PrincipalOptionKind>
    {
        private UserOptionHelper()
        {
            AddOptionMapping(PrincipalOptionKind.DefaultSchema, CodeGenerationSupporter.DefaultSchema);
            AddOptionMapping(PrincipalOptionKind.DefaultLanguage, CodeGenerationSupporter.DefaultLanguage);
            AddOptionMapping(PrincipalOptionKind.Name, CodeGenerationSupporter.Name);
            AddOptionMapping(PrincipalOptionKind.Login, CodeGenerationSupporter.Login);
            AddOptionMapping(PrincipalOptionKind.Type, CodeGenerationSupporter.Type);
            AddOptionMapping(PrincipalOptionKind.Sid, CodeGenerationSupporter.Sid);
            AddOptionMapping(PrincipalOptionKind.Object_ID, CodeGenerationSupporter.Object_ID);
        }

        internal static readonly UserOptionHelper Instance = new UserOptionHelper();
    }
}
