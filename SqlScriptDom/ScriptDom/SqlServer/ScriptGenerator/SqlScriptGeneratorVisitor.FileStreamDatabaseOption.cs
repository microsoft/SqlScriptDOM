//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.FileStreamDatabaseOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<NonTransactedFileStreamAccess, string> _nonTransactedFileStreamAccessNames = new Dictionary<NonTransactedFileStreamAccess, string>()
        {
            { NonTransactedFileStreamAccess.Full, CodeGenerationSupporter.Full},
            { NonTransactedFileStreamAccess.Off, CodeGenerationSupporter.Off},
            { NonTransactedFileStreamAccess.ReadOnly, CodeGenerationSupporter.ReadOnly},
            
        };

        public override void ExplicitVisit(FileStreamDatabaseOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == DatabaseOptionKind.FileStream, "DatabaseOptionKind does not match");
            GenerateIdentifier(CodeGenerationSupporter.FileStream);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);
            if (node.NonTransactedAccess.HasValue)
            {
                string optionName = GetValueForEnumKey(_nonTransactedFileStreamAccessNames, node.NonTransactedAccess.Value);
                GenerateNameEqualsValue(CodeGenerationSupporter.NonTransactedAccess, optionName);
            }
            if (node.NonTransactedAccess.HasValue && node.DirectoryName != null)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpace();
            }
            if (node.DirectoryName != null)
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.DirectoryName, node.DirectoryName);
            }
            GenerateSymbol(TSqlTokenType.RightParenthesis);
        }
    }
}