//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DeviceInfoCollection.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(MirrorToClause node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Mirror);
            GenerateSpaceAndKeyword(TSqlTokenType.To);
            GenerateSpace();
            GenerateCommaSeparatedList(node.Devices);
        }
    }
}
