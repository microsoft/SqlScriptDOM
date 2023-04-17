//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.IPv4.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(IPv4 node)
        {
            GenerateFragmentIfNotNull(node.OctetOne);
            GenerateSymbol(TSqlTokenType.Dot);
            GenerateFragmentIfNotNull(node.OctetTwo);
            GenerateSymbol(TSqlTokenType.Dot);
            GenerateFragmentIfNotNull(node.OctetThree);
            GenerateSymbol(TSqlTokenType.Dot);
            GenerateFragmentIfNotNull(node.OctetFour);
        }
    }
}
