//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CompressionEndpointProtocolOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CompressionEndpointProtocolOption node)
        {
            GenerateNameEqualsValue(
                    CodeGenerationSupporter.Compression, 
                    node.IsEnabled ? CodeGenerationSupporter.Enabled : CodeGenerationSupporter.Disabled);
        }
    }
}
