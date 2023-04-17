//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.Identifier.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Text;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(Identifier node)
        {
            if (node.Value != null)
            {
                if (node.QuoteType == QuoteType.NotQuoted)
                {
                    GenerateIdentifierWithoutCheck(node.Value);
                }
                else
                {
                    GenerateQuotedIdentifier(node.Value, node.QuoteType);
                }
            }
        }

        private void GenerateQuotedIdentifier(string identifier, QuoteType quoteType)
        {
            GenerateIdentifierWithoutCheck(Identifier.EncodeIdentifier(identifier, quoteType)); 
        }
    }
}
