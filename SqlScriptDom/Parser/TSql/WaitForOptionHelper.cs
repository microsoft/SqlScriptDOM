//------------------------------------------------------------------------------
// <copyright file="WaitForOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Deals with DECLARE CURSOR options
    /// </summary>
    
    [Serializable]
    internal class WaitForOptionHelper : OptionsHelper<WaitForOption>
    {
        private WaitForOptionHelper()
        {
            AddOptionMapping(WaitForOption.Delay, CodeGenerationSupporter.Delay);
            AddOptionMapping(WaitForOption.Time, CodeGenerationSupporter.Time);
        }

        internal static readonly WaitForOptionHelper Instance = new WaitForOptionHelper();
    }
}