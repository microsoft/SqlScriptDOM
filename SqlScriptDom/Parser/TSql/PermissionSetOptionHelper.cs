//------------------------------------------------------------------------------
// <copyright file="PermissionSetOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Deals with DECLARE CURSOR options
    /// </summary>
    
    [Serializable]
    internal class PermissionSetOptionHelper : OptionsHelper<PermissionSetOption>
    {
        private PermissionSetOptionHelper()
        {
            AddOptionMapping(PermissionSetOption.Safe, CodeGenerationSupporter.Safe);
            AddOptionMapping(PermissionSetOption.ExternalAccess, CodeGenerationSupporter.ExternalAccess);
            AddOptionMapping(PermissionSetOption.Unsafe, CodeGenerationSupporter.Unsafe);
        }

        internal static readonly PermissionSetOptionHelper Instance = new PermissionSetOptionHelper();
    }
}