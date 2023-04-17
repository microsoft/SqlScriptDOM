//------------------------------------------------------------------------------
// <copyright file="SimpleDbOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    [Serializable]
    internal class SimpleDbOptionsHelper : OptionsHelper<DatabaseOptionKind>
    {
        private SimpleDbOptionsHelper()
        {
            AddOptionMapping(DatabaseOptionKind.Online, CodeGenerationSupporter.Online, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.Offline, CodeGenerationSupporter.Offline, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.SingleUser, CodeGenerationSupporter.SingleUser, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.RestrictedUser, CodeGenerationSupporter.RestrictedUser, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.MultiUser, CodeGenerationSupporter.MultiUser, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.ReadOnly, CodeGenerationSupporter.ReadOnly, SqlVersionFlags.TSqlAll);
            AddOptionMapping(DatabaseOptionKind.ReadWrite, CodeGenerationSupporter.ReadWrite, SqlVersionFlags.TSqlAll);

            // TSql 90 options
            AddOptionMapping(DatabaseOptionKind.Emergency, CodeGenerationSupporter.Emergency, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(DatabaseOptionKind.EnableBroker, CodeGenerationSupporter.EnableBroker, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(DatabaseOptionKind.DisableBroker, CodeGenerationSupporter.DisableBroker, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(DatabaseOptionKind.NewBroker, CodeGenerationSupporter.NewBroker, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(DatabaseOptionKind.ErrorBrokerConversations, CodeGenerationSupporter.ErrorBrokerConversations, SqlVersionFlags.TSql90AndAbove);
        }

        internal static readonly SimpleDbOptionsHelper Instance = new SimpleDbOptionsHelper();
    }
}
