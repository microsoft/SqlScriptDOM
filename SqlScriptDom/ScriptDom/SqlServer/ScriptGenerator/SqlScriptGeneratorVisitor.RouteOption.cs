//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.RouteOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        private static Dictionary<RouteOptionKind, String> _RouteOptionTypeNames = new Dictionary<RouteOptionKind, String>()
        {
            {RouteOptionKind.Address, CodeGenerationSupporter.Address},
            {RouteOptionKind.BrokerInstance, CodeGenerationSupporter.BrokerInstance},
            {RouteOptionKind.Lifetime, CodeGenerationSupporter.LifeTime},
            {RouteOptionKind.MirrorAddress, CodeGenerationSupporter.MirrorAddress},
            {RouteOptionKind.ServiceName, CodeGenerationSupporter.ServiceName},
        };

        public override void ExplicitVisit(RouteOption node)
        {
            String optionName = GetValueForEnumKey(_RouteOptionTypeNames, node.OptionKind);
            GenerateNameEqualsValue(optionName, node.Literal);
        }
    }
}
