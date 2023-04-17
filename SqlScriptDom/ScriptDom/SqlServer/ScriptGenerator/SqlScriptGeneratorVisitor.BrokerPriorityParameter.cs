//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BrokerPriorityParameter.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(BrokerPriorityParameter node)
        {
            BrokerPriorityParameterHelper.Instance.GenerateSourceForOption(_writer, node.ParameterType);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            switch (node.IsDefaultOrAny)
            {
                case BrokerPriorityParameterSpecialType.None:
                    GenerateSpaceAndFragmentIfNotNull(node.ParameterValue);
                    break;
                case BrokerPriorityParameterSpecialType.Default:
                    GenerateSpaceAndKeyword(TSqlTokenType.Default);
                    break;
                case BrokerPriorityParameterSpecialType.Any:
                    GenerateSpaceAndKeyword(TSqlTokenType.Any);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }                    
        }
    }
}
