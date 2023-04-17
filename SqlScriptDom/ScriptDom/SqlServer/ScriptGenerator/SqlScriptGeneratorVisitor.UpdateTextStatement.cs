//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.UpdateTextStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(UpdateTextStatement node)
        {
            // UPDATETEXT
            GenerateKeyword(TSqlTokenType.UpdateText);
            
            // BULK column TIMESTAMP = timestamp
            GenerateSpace();
            GenerateBulkColumnTimestamp(node);

            // insert offset
            GenerateSpaceAndFragmentIfNotNull(node.InsertOffset);

            // delete length
            GenerateSpaceAndFragmentIfNotNull(node.DeleteLength);

            // WITH LOG
            NewLine();
            GenerateKeyword(TSqlTokenType.With);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Log); 

            if (node.SourceColumn != null)
            {
                GenerateSpaceAndFragmentIfNotNull(node.SourceColumn);
            }

            GenerateSpaceAndFragmentIfNotNull(node.SourceParameter);
        }
    }
}
