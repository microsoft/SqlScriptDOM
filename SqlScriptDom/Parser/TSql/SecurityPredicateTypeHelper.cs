//------------------------------------------------------------------------------
// <copyright file="SecurityPredicateTypeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Helper to determine the security predicate type.
    /// </summary>
    [Serializable]
    internal class SecurityPredicateTypeHelper : OptionsHelper<SecurityPredicateType>
    {
        private SecurityPredicateTypeHelper()
        {
            AddOptionMapping(SecurityPredicateType.Filter, CodeGenerationSupporter.Filter, SqlVersionFlags.TSql130AndAbove);
            AddOptionMapping(SecurityPredicateType.Block, CodeGenerationSupporter.Block, SqlVersionFlags.TSql130AndAbove);
        }

        internal static readonly SecurityPredicateTypeHelper Instance = new SecurityPredicateTypeHelper();
    }
}
