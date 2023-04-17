//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.PageVerifyAlterDbOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {

        private static Dictionary<PageVerifyDatabaseOptionKind, String> _pageVerifyDatabaseOptionKindNames = new Dictionary<PageVerifyDatabaseOptionKind, String>()
        {
            {PageVerifyDatabaseOptionKind.Checksum, CodeGenerationSupporter.Checksum},
            {PageVerifyDatabaseOptionKind.None, CodeGenerationSupporter.None},
            {PageVerifyDatabaseOptionKind.TornPageDetection, CodeGenerationSupporter.TornPageDetection},
        };

        public override void ExplicitVisit(PageVerifyDatabaseOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == DatabaseOptionKind.PageVerify);
            GenerateIdentifier(CodeGenerationSupporter.PageVerify);
            String optionName = GetValueForEnumKey(_pageVerifyDatabaseOptionKindNames, node.Value);
            if (optionName != null)
            {
                GenerateSpaceAndIdentifier(optionName);
            }
        }
    }
}
