//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.XmlNamespaces.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(XmlNamespaces node)
        {
            GenerateIdentifier(CodeGenerationSupporter.XmlNamespaces);

            GenerateSpace();
            GenerateParenthesisedCommaSeparatedList(node.XmlNamespacesElements);
        }
    }
}
