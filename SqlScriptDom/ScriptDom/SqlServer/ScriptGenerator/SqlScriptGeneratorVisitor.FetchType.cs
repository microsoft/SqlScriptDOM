//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.FetchType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        private static Dictionary<FetchOrientation, String> _fetchOrientationNames = new Dictionary<FetchOrientation , String>()
        {
            {FetchOrientation.Absolute, CodeGenerationSupporter.Absolute},
            {FetchOrientation.First, CodeGenerationSupporter.First},
            {FetchOrientation.Last, CodeGenerationSupporter.Last},
            {FetchOrientation.Next, CodeGenerationSupporter.Next},
            {FetchOrientation.Prior, CodeGenerationSupporter.Prior},
            {FetchOrientation.Relative, CodeGenerationSupporter.Relative},
        };

        public override void ExplicitVisit(FetchType node)
        {
            if (node.Orientation != FetchOrientation.None)
            {
                String optionName = GetValueForEnumKey(_fetchOrientationNames, node.Orientation);
                if (optionName != null)
                {
                    GenerateIdentifier(optionName); 
                    if (node.Orientation == FetchOrientation.Absolute || node.Orientation == FetchOrientation.Relative)
                    {
                        GenerateSpaceAndFragmentIfNotNull(node.RowOffset);
                    }
                }
            }
        }
    }
}
