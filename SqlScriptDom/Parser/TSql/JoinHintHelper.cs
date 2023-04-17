//------------------------------------------------------------------------------
// <copyright file="IntegerOptimizerHintHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class JoinHintHelper : OptionsHelper<JoinHint>
    {
        private JoinHintHelper()
        {
            AddOptionMapping(JoinHint.Hash, CodeGenerationSupporter.Hash, SqlVersionFlags.TSqlAll);
            AddOptionMapping(JoinHint.Loop, CodeGenerationSupporter.Loop, SqlVersionFlags.TSqlAll);
            AddOptionMapping(JoinHint.Merge, CodeGenerationSupporter.Merge, SqlVersionFlags.TSqlAll);
            AddOptionMapping(JoinHint.Remote, CodeGenerationSupporter.Remote, SqlVersionFlags.TSqlAll);
        }

        internal static readonly JoinHintHelper Instance = new JoinHintHelper();
    }
}
