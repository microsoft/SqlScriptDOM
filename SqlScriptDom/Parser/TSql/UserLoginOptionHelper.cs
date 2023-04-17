//------------------------------------------------------------------------------
// <copyright file="UserLoginOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    [Serializable]
    internal class UserLoginOptionHelper : OptionsHelper<UserLoginOptionType>
    {
        private UserLoginOptionHelper()
        {
            AddOptionMapping(UserLoginOptionType.Certificate, CodeGenerationSupporter.Certificate);
            AddOptionMapping(UserLoginOptionType.Login, CodeGenerationSupporter.Login);
        }

        internal static readonly UserLoginOptionHelper Instance = new UserLoginOptionHelper();
    }
}
