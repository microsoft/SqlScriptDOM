//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreatePartitionFunctionStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreatePartitionFunctionStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Partition);
            GenerateSpaceAndKeyword(TSqlTokenType.Function);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // input parameter type
            GenerateParenthesisedFragmentIfNotNull(node.ParameterType);

            // AS RANGE
            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.As);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Range);

            switch (node.Range)
            {
                case PartitionFunctionRange.Left:
                    GenerateSpaceAndKeyword(TSqlTokenType.Left); 
                    break;
                case PartitionFunctionRange.Right:
                    GenerateSpaceAndKeyword(TSqlTokenType.Right); 
                    break;
            }

            // FOR VALUES
            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.For);
            GenerateSpaceAndKeyword(TSqlTokenType.Values);
            GenerateSpace();

            GenerateParenthesisedCommaSeparatedList(node.BoundaryValues, true);
        }

        public override void ExplicitVisit(PartitionParameterType node)
        {
            GenerateFragmentIfNotNull(node.DataType);

            if (node.Collation != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Collate);
                GenerateSpaceAndFragmentIfNotNull(node.Collation);
            }
        }
    }
}
