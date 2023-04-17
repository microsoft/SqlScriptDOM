//------------------------------------------------------------------------------
// <copyright file="LowPriorityLockWaitMaxDurationTimeUnitHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    [Serializable]
    internal class LowPriorityLockWaitMaxDurationTimeUnitHelper : OptionsHelper<TimeUnit>
    {
        private LowPriorityLockWaitMaxDurationTimeUnitHelper()
        {
            AddOptionMapping(TimeUnit.Minutes, CodeGenerationSupporter.Minutes);
        }

        internal static readonly LowPriorityLockWaitMaxDurationTimeUnitHelper Instance = new LowPriorityLockWaitMaxDurationTimeUnitHelper();
    }
}
