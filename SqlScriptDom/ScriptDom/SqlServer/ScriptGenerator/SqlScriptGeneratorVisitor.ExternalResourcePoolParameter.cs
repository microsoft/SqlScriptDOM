//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ExternalResourcePoolParameter.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ExternalResourcePoolParameter node)
        {
            ExternalResourcePoolParameterHelper.Instance.GenerateSourceForOption(_writer, node.ParameterType);

            if (node.ParameterType != ExternalResourcePoolParameterType.Affinity)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
                GenerateSpaceAndFragmentIfNotNull(node.ParameterValue);
            }
            else
            {
                System.Diagnostics.Debug.Assert(node.AffinitySpecification != null,
                    "External resource pool parameter of type AFFINITY must have a valid AffinitySpecification");
                GenerateSpaceAndFragmentIfNotNull(node.AffinitySpecification);
            }
        }

        public override void ExplicitVisit(ExternalResourcePoolAffinitySpecification node)
        {
            ExternalResourcePoolAffinityHelper.Instance.GenerateSourceForOption(_writer, node.AffinityType);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            GenerateSpace();
            if (node.IsAuto)
            {
                GenerateIdentifier(CodeGenerationSupporter.Auto);
            }
            else if ((node.PoolAffinityRanges != null) && (node.PoolAffinityRanges.Count > 0))
            {
                GenerateParenthesisedCommaSeparatedList(node.PoolAffinityRanges);
            }
        }
    }
}
