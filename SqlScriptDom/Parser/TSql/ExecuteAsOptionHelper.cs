//------------------------------------------------------------------------------
// <copyright file="ExecuteAsOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class ExecuteAsOptionHelper : OptionsHelper<ExecuteAsOption>
    {
        private ExecuteAsOptionHelper()
        {
            AddOptionMapping(ExecuteAsOption.Caller, CodeGenerationSupporter.Caller);
            AddOptionMapping(ExecuteAsOption.Self, CodeGenerationSupporter.Self);
            AddOptionMapping(ExecuteAsOption.Owner, CodeGenerationSupporter.Owner);
        }

        internal static readonly ExecuteAsOptionHelper Instance = new ExecuteAsOptionHelper();
    }
}
