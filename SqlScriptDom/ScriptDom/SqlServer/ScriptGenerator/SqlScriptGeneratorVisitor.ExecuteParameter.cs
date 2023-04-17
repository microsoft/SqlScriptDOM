//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ExecuteParameter.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ExecuteParameter node)
        {
            if (node.Variable != null)
            {
                GenerateFragmentIfNotNull(node.Variable);
                GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                GenerateSpace();
            }

            if (node.ParameterValue != null)
            {
                GenerateFragmentIfNotNull(node.ParameterValue);
            }

            if (node.IsOutput)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Output); 
            }
        }
    }
}
