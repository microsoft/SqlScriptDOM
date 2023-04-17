//------------------------------------------------------------------------------
// <copyright file="RaiseErrorOptionsHelper.cs" company="Microsoft">
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
    internal class RaiseErrorOptionsHelper : OptionsHelper<RaiseErrorOptions>
    {
        private RaiseErrorOptionsHelper()
        {
            AddOptionMapping(RaiseErrorOptions.Log, CodeGenerationSupporter.Log);
            AddOptionMapping(RaiseErrorOptions.NoWait, CodeGenerationSupporter.NoWait);
            AddOptionMapping(RaiseErrorOptions.SetError, CodeGenerationSupporter.SetError);
        }

        internal static readonly RaiseErrorOptionsHelper Instance = new RaiseErrorOptionsHelper();
    }
}