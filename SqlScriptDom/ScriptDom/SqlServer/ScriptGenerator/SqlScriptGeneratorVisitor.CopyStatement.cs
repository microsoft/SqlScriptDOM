//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CopyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CopyStatement node)
        {
            GenerateIdentifier(CodeGenerationSupporter.CopyCommand);

            GenerateSpaceAndKeyword(TSqlTokenType.Into);
            GenerateSpaceAndFragmentIfNotNull(node.Into);
            var options = new List<CopyOption>(node.Options);
            foreach( var option in options)
            {
                if (option.Kind == CopyOptionKind.ColumnOptions)
                {
                    GenerateParenthesisedCommaSeparatedList(((ListTypeCopyOption)option.Value).Options);
                    options.Remove(option);
                    //NewLine();
                    break;
                }
            }

            GenerateSpaceAndKeyword(TSqlTokenType.From);
            GenerateSpace();
            GenerateFragmentList(node.From);

            if (node.Options.Count > 0)
            {
                NewLine();

                GenerateKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                NewLine();
                GenerateSpace();
                GenerateSpace();

                GenerateCommaSeparatedList(options);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }           
        }
    }
}
