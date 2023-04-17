//------------------------------------------------------------------------------
// <copyright file="RouteOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    [Serializable]
    internal class RouteOptionHelper : OptionsHelper<RouteOptionKind>
    {
        private RouteOptionHelper()
        {
            AddOptionMapping(RouteOptionKind.Address, CodeGenerationSupporter.Address);
            AddOptionMapping(RouteOptionKind.BrokerInstance, CodeGenerationSupporter.BrokerInstance);
            AddOptionMapping(RouteOptionKind.Lifetime, CodeGenerationSupporter.LifeTime);
            AddOptionMapping(RouteOptionKind.MirrorAddress, CodeGenerationSupporter.MirrorAddress);
            AddOptionMapping(RouteOptionKind.ServiceName, CodeGenerationSupporter.ServiceName);
        }

        internal static readonly RouteOptionHelper Instance = new RouteOptionHelper();
    }
}
