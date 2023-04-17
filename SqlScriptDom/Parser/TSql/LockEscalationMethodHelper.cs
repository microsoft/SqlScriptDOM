//------------------------------------------------------------------------------
// <copyright file="LockEscalationMethodHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class LockEscalationMethodHelper : OptionsHelper<LockEscalationMethod>
    {
        private LockEscalationMethodHelper()
        {
            AddOptionMapping(LockEscalationMethod.Auto, CodeGenerationSupporter.Auto);
            AddOptionMapping(LockEscalationMethod.Disable, CodeGenerationSupporter.Disable);
            AddOptionMapping(LockEscalationMethod.Table, TSqlTokenType.Table);
        }

        public static LockEscalationMethodHelper Instance = new LockEscalationMethodHelper();
    }
}
