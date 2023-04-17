//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateFulltextStoplistStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateFullTextStopListStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Fulltext);
            GenerateSpaceAndKeyword(TSqlTokenType.StopList);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            if (node.IsSystemStopList)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.From);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.System);
                GenerateSpaceAndKeyword(TSqlTokenType.StopList);
            }
            else if (node.SourceStopListName != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.From);
                if (node.DatabaseName != null)
                {
                    GenerateSpaceAndFragmentIfNotNull(node.DatabaseName);
                    GenerateKeyword(TSqlTokenType.Dot);
                }
                else
                    GenerateSpace();
                GenerateFragmentIfNotNull(node.SourceStopListName);
            }

            // owner
            GenerateOwnerIfNotNull(node.Owner);
        }
    }
}
