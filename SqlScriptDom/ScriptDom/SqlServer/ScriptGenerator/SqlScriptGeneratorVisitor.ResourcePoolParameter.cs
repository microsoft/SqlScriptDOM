//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ResourcePoolParameter.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ResourcePoolParameter node)
        {
            ResourcePoolParameterHelper.Instance.GenerateSourceForOption(_writer, node.ParameterType);

            if (node.ParameterType != ResourcePoolParameterType.Affinity)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
                GenerateSpaceAndFragmentIfNotNull(node.ParameterValue);
            }
            else
            {
                System.Diagnostics.Debug.Assert(node.AffinitySpecification != null,
                    "Resource pool parameter of type AFFINITY must have a valid AffinitySpecification");
                GenerateSpaceAndFragmentIfNotNull(node.AffinitySpecification);
            }
        }

        public override void ExplicitVisit(ResourcePoolAffinitySpecification node)
        {
            ResourcePoolAffinityHelper.Instance.GenerateSourceForOption(_writer, node.AffinityType);
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

        public override void ExplicitVisit(LiteralRange node)
        {
            GenerateFragmentIfNotNull(node.From);
            if (node.To != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.To);
                GenerateSpaceAndFragmentIfNotNull(node.To);
            }
        }
    }
}
