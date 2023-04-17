//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.FulltextStoplistAction.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(FullTextStopListAction node)
        {
            if (node.IsAdd)
                GenerateKeyword(TSqlTokenType.Add);
            else
                GenerateKeyword(TSqlTokenType.Drop);

            if (!node.IsAll)
                GenerateSpaceAndFragmentIfNotNull(node.StopWord);
            else
                GenerateSpaceAndKeyword(TSqlTokenType.All);
            
            if (node.LanguageTerm != null)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Language);
                GenerateSpaceAndFragmentIfNotNull(node.LanguageTerm);
            }
        }
    }
}
