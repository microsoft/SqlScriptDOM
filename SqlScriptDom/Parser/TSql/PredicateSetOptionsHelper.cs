//------------------------------------------------------------------------------
// <copyright file="PredicateSetOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Helps with RaiseError options
    /// </summary>
    
    [Serializable]
    internal class PredicateSetOptionsHelper : OptionsHelper<SetOptions>
    {
        private PredicateSetOptionsHelper()
        {
            AddOptionMapping(SetOptions.QuotedIdentifier, CodeGenerationSupporter.QuotedIdentifier, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.ConcatNullYieldsNull, CodeGenerationSupporter.ConcatNullYieldsNull, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.CursorCloseOnCommit, CodeGenerationSupporter.CursorCloseOnCommit, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.ArithAbort, CodeGenerationSupporter.ArithAbort, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.ArithIgnore, CodeGenerationSupporter.ArithIgnore, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.FmtOnly, CodeGenerationSupporter.FmtOnly, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.NoCount, CodeGenerationSupporter.NoCount, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.NoExec, CodeGenerationSupporter.NoExec, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.NumericRoundAbort, CodeGenerationSupporter.NumericRoundAbort, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.ParseOnly, CodeGenerationSupporter.ParseOnly, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.AnsiDefaults, CodeGenerationSupporter.AnsiDefaults, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.AnsiNullDfltOff, CodeGenerationSupporter.AnsiNullDfltOff, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.AnsiNullDfltOn, CodeGenerationSupporter.AnsiNullDfltOn, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.AnsiNulls, CodeGenerationSupporter.AnsiNulls, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.AnsiPadding, CodeGenerationSupporter.AnsiPadding, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.AnsiWarnings, CodeGenerationSupporter.AnsiWarnings, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.ForcePlan, CodeGenerationSupporter.ForcePlan, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.ShowPlanAll, CodeGenerationSupporter.ShowPlanAll, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.ShowPlanText, CodeGenerationSupporter.ShowPlanText, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.ShowPlanXml, CodeGenerationSupporter.ShowPlanXml, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.ImplicitTransactions, CodeGenerationSupporter.ImplicitTransactions, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.RemoteProcTransactions, CodeGenerationSupporter.RemoteProcTransactions, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.XactAbort, CodeGenerationSupporter.XactAbort, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetOptions.DisableDefCnstChk, CodeGenerationSupporter.DisableDefCnstChk, SqlVersionFlags.TSqlUnder110);
            AddOptionMapping(SetOptions.NoBrowsetable, CodeGenerationSupporter.NoBrowsetable, SqlVersionFlags.TSqlAll);
        }

        internal static readonly PredicateSetOptionsHelper Instance = new PredicateSetOptionsHelper();
    }
}