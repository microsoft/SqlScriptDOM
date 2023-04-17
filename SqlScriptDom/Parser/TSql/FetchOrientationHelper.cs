//------------------------------------------------------------------------------
// <copyright file="FetchOrientationHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Deals with fetch orientation kinds
    /// </summary>
    
    internal class FetchOrientationHelper : OptionsHelper<FetchOrientation>
    {
        private FetchOrientationHelper()
        {
            AddOptionMapping(FetchOrientation.First, CodeGenerationSupporter.First);
            AddOptionMapping(FetchOrientation.Next, CodeGenerationSupporter.Next);
            AddOptionMapping(FetchOrientation.Prior, CodeGenerationSupporter.Prior);
            AddOptionMapping(FetchOrientation.Last, CodeGenerationSupporter.Last);
            AddOptionMapping(FetchOrientation.Relative, CodeGenerationSupporter.Relative);
            AddOptionMapping(FetchOrientation.Absolute, CodeGenerationSupporter.Absolute);
        }

        internal static readonly FetchOrientationHelper Instance = new FetchOrientationHelper();
    }
}