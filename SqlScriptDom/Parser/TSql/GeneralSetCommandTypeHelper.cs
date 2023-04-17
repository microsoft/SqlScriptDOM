//------------------------------------------------------------------------------
// <copyright file="GeneralSetCommandTypeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Deals with different SET command types
    /// </summary>
    
    internal class GeneralSetCommandTypeHelper : OptionsHelper<GeneralSetCommandType>
    {
        private GeneralSetCommandTypeHelper()
        {
            AddOptionMapping(GeneralSetCommandType.Language, CodeGenerationSupporter.Language);
            AddOptionMapping(GeneralSetCommandType.DateFormat, CodeGenerationSupporter.DateFormat);
            AddOptionMapping(GeneralSetCommandType.DateFirst, CodeGenerationSupporter.DateFirst);
            AddOptionMapping(GeneralSetCommandType.DeadlockPriority, CodeGenerationSupporter.DeadlockPriority);
            AddOptionMapping(GeneralSetCommandType.LockTimeout, CodeGenerationSupporter.LockTimeout);
            AddOptionMapping(GeneralSetCommandType.QueryGovernorCostLimit, CodeGenerationSupporter.QueryGovernorCostLimit);
            AddOptionMapping(GeneralSetCommandType.ContextInfo, CodeGenerationSupporter.ContextInfo);
        }

        internal static readonly GeneralSetCommandTypeHelper Instance = new GeneralSetCommandTypeHelper();
    }
}