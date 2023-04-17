//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DeclareCursorStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DeclareCursorStatement node)
        {
            GenerateKeyword(TSqlTokenType.Declare);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);
            if (node.CursorDefinition != null && node.CursorDefinition.Options != null)
            {
                foreach (CursorOption cursorOption in node.CursorDefinition.Options)
                {
                    if (cursorOption.OptionKind == CursorOptionKind.Insensitive)
                    {
                        GenerateFragmentIfNotNull(cursorOption);
                    }
                }
            }
            GenerateSpaceAndFragmentIfNotNull(node.CursorDefinition);
        }
    }
}
