//------------------------------------------------------------------------------
// <copyright file="AllowReadOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    internal class AllowConnectionsOptionsHelper : OptionsHelper<AllowConnectionsOptionKind>
    {
        private AllowConnectionsOptionsHelper()
        {
            AddOptionMapping(AllowConnectionsOptionKind.All, CodeGenerationSupporter.All);
            AddOptionMapping(AllowConnectionsOptionKind.No, CodeGenerationSupporter.No);
            AddOptionMapping(AllowConnectionsOptionKind.ReadOnly, CodeGenerationSupporter.ReadOnly);
            AddOptionMapping(AllowConnectionsOptionKind.ReadWrite, CodeGenerationSupporter.ReadWrite);
        }

        public static readonly AllowConnectionsOptionsHelper Instance = new AllowConnectionsOptionsHelper();
    }
}
