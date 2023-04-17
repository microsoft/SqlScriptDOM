//------------------------------------------------------------------------------
// <copyright file="DelayedDurabilityOptionKindHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class DelayedDurabilityOptionKindHelper : OptionsHelper<DelayedDurabilityOptionKind>
    {
        private DelayedDurabilityOptionKindHelper()
        {
            AddOptionMapping(DelayedDurabilityOptionKind.Disabled, CodeGenerationSupporter.Disabled);
            AddOptionMapping(DelayedDurabilityOptionKind.Allowed, CodeGenerationSupporter.Allowed);
            AddOptionMapping(DelayedDurabilityOptionKind.Forced, CodeGenerationSupporter.Forced);
        }

        internal static readonly DelayedDurabilityOptionKindHelper Instance = new DelayedDurabilityOptionKindHelper();
    }
}
