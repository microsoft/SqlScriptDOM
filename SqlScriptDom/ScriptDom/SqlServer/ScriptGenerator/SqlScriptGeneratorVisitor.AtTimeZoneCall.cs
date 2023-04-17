//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AtTimeZoneCall.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AtTimeZoneCall node)
        {
            GenerateFragmentIfNotNull(node.DateValue);

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.At);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Time);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Zone);

            GenerateSpaceAndFragmentIfNotNull(node.TimeZone);
        }
    }
}
