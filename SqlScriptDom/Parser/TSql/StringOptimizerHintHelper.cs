//------------------------------------------------------------------------------
// <copyright file="StringOptimizerHintHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class StringOptimizerHintHelper : OptionsHelper<OptimizerHintKind>
    {
        private StringOptimizerHintHelper()
        {
            AddOptionMapping(OptimizerHintKind.Label, CodeGenerationSupporter.Label, SqlVersionFlags.TSql130AndAbove);
        }

        internal static readonly StringOptimizerHintHelper Instance = new StringOptimizerHintHelper();
    }
}

