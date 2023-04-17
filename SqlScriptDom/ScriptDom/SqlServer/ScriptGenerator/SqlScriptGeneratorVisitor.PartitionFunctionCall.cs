//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.PartitionFunctionCall.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(PartitionFunctionCall node)
        {
            if (node.DatabaseName != null)
            {
                GenerateFragmentIfNotNull(node.DatabaseName);
                GenerateSymbol(TSqlTokenType.Dot); 
            }

            GenerateIdentifier(CodeGenerationSupporter.DollarPartition); // $Partition
            GenerateSymbol(TSqlTokenType.Dot);
            GenerateFragmentIfNotNull(node.FunctionName);

            GenerateSpace();
            GenerateParenthesisedCommaSeparatedList(node.Parameters);
        }
    }
}
