//------------------------------------------------------------------------------
// <copyright file="EventSessionEventRetentionModeTypeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class EventSessionEventRetentionModeTypeHelper : OptionsHelper<EventSessionEventRetentionModeType>
    {
        private EventSessionEventRetentionModeTypeHelper()
        {
            AddOptionMapping(EventSessionEventRetentionModeType.AllowMultipleEventLoss, CodeGenerationSupporter.AllowMultipleEventLoss);
            AddOptionMapping(EventSessionEventRetentionModeType.AllowSingleEventLoss, CodeGenerationSupporter.AllowSingleEventLoss);
            AddOptionMapping(EventSessionEventRetentionModeType.NoEventLoss, CodeGenerationSupporter.NoEventLoss);

        }

        internal static readonly EventSessionEventRetentionModeTypeHelper Instance = new EventSessionEventRetentionModeTypeHelper();
    }
}
