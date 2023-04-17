//------------------------------------------------------------------------------
// <copyright file="ApplicationRoleOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class ApplicationRoleOptionHelper : OptionsHelper<ApplicationRoleOptionKind>
    {
        private ApplicationRoleOptionHelper()
        {
            AddOptionMapping(ApplicationRoleOptionKind.DefaultSchema, CodeGenerationSupporter.DefaultSchema);
            AddOptionMapping(ApplicationRoleOptionKind.Password, CodeGenerationSupporter.Password);
            AddOptionMapping(ApplicationRoleOptionKind.Name, CodeGenerationSupporter.Name);
            AddOptionMapping(ApplicationRoleOptionKind.Login, CodeGenerationSupporter.Login);
        }

        internal static readonly ApplicationRoleOptionHelper Instance = new ApplicationRoleOptionHelper();
    }
}
