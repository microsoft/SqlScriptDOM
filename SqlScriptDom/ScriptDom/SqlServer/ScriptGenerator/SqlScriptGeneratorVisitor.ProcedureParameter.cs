//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ProcedureParameter.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ProcedureParameter node)
        {
            GenerateFragmentIfNotNull(node.VariableName);
            GenerateSpaceAndFragmentIfNotNull(node.DataType);
            if (node.IsVarying)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Varying); 
            }

			GenerateSpaceAndFragmentIfNotNull(node.Nullable);

            if (node.Value != null)
            {
                GenerateSymbol(TSqlTokenType.EqualsSign);
                GenerateFragmentIfNotNull(node.Value);
            }

            switch (node.Modifier)
            {
                case ParameterModifier.Output:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Output);
                    break;
                case ParameterModifier.ReadOnly:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.ReadOnlyOld);
                    break;
                case ParameterModifier.None:
                    break;
                default:
                    Debug.Assert(false, "Unknown parameter modifier");
                    break;
            }
        }
    }
}
