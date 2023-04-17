//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.XmlNamespacesAliasElement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(XmlNamespacesAliasElement node)
        {
            GenerateFragmentIfNotNull(node.String);
            GenerateSpaceAndKeyword(TSqlTokenType.As);
            GenerateSpaceAndFragmentIfNotNull(node.Identifier);
        }
    }
}
