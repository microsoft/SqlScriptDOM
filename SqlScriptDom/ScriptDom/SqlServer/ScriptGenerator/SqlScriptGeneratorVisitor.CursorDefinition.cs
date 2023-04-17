//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CursorDefinition.cs" company="Microsoft">
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

        private static Dictionary<CursorOptionKind, String> _cursorOptionsNames = new Dictionary<CursorOptionKind, String>()
        {
            { CursorOptionKind.Local, CodeGenerationSupporter.Local},
            { CursorOptionKind.Global, CodeGenerationSupporter.Global},
            { CursorOptionKind.Scroll, CodeGenerationSupporter.Scroll},
            { CursorOptionKind.ForwardOnly, CodeGenerationSupporter.ForwardOnly},
            { CursorOptionKind.Insensitive, CodeGenerationSupporter.Insensitive},
            { CursorOptionKind.Keyset, CodeGenerationSupporter.Keyset},
            { CursorOptionKind.Dynamic, CodeGenerationSupporter.Dynamic},
            { CursorOptionKind.FastForward, CodeGenerationSupporter.FastForward},
            { CursorOptionKind.ScrollLocks, CodeGenerationSupporter.ScrollLocks},
            { CursorOptionKind.Optimistic, CodeGenerationSupporter.Optimistic},
            { CursorOptionKind.ReadOnly, CodeGenerationSupporter.ReadOnly},
            { CursorOptionKind.Static, CodeGenerationSupporter.Static},
            { CursorOptionKind.TypeWarning, CodeGenerationSupporter.TypeWarning},
        };
  
        public override void ExplicitVisit(CursorDefinition node)
        {
            AlignmentPoint forBody = new AlignmentPoint();

            GenerateKeyword(TSqlTokenType.Cursor);

            foreach (CursorOption option in node.Options)
            {
                if (option.OptionKind != CursorOptionKind.Insensitive)
                {
                    GenerateFragmentIfNotNull(option);
                }
            }
            
            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.For);

            GenerateSpace();
            MarkAndPushAlignmentPoint(forBody);

            // to avoid generate semicolon for the SELECT statement
            Boolean originalValue = _generateSemiColon;
            _generateSemiColon = false;

            GenerateFragmentIfNotNull(node.Select);
            
            // restore the original value
            _generateSemiColon = originalValue;

            PopAlignmentPoint();
        }

        public override void ExplicitVisit(CursorOption node)
        {
            String optionName = GetValueForEnumKey(_cursorOptionsNames, node.OptionKind);
            GenerateSpaceAndIdentifier(optionName); 
        }
    }

    
}
