//------------------------------------------------------------------------------
// <copyright file="ContainmentOptionKindHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class ContainmentOptionKindHelper : OptionsHelper<ContainmentOptionKind>
    {
        private ContainmentOptionKindHelper()
        {
            AddOptionMapping(ContainmentOptionKind.None, CodeGenerationSupporter.None);
            AddOptionMapping(ContainmentOptionKind.Partial, CodeGenerationSupporter.Partial);
        }

        internal static readonly ContainmentOptionKindHelper Instance = new ContainmentOptionKindHelper();
    }
}
