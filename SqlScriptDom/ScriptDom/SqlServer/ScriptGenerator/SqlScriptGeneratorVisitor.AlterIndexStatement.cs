//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterIndexStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterIndexStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndKeyword(TSqlTokenType.Index); 

            if (node.All)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.All); 
            }
            else
            {
                GenerateSpaceAndFragmentIfNotNull(node.Name);
            }

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.On);
            GenerateSpaceAndFragmentIfNotNull(node.OnName);
            GenerateSpace();

            if (node.AlterIndexType == AlterIndexType.Set)
            {
                GenerateKeywordAndSpace(TSqlTokenType.Set);
                GenerateParenthesisedCommaSeparatedList(node.IndexOptions);
            }
            else if (node.AlterIndexType == AlterIndexType.UpdateSelectiveXmlPaths)
            {
                // XMLNAMESPACES
                if (node.XmlNamespaces != null)
                {
                    NewLine();
                    GenerateKeyword(TSqlTokenType.With);
                    GenerateSpaceAndFragmentIfNotNull(node.XmlNamespaces);
                }
                
                NewLine();
                // generate list of specified xml path
                GenerateKeyword(TSqlTokenType.For);
                NewLine();
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                bool first = true;
                foreach (SelectiveXmlIndexPromotedPath xmlpath in node.PromotedPaths)
                {
                    if (!first)
                        GenerateSymbol(TSqlTokenType.Comma);
                    NewLineAndIndent();
                    if (xmlpath.Path == null)
                    {
                        GenerateSpaceAndIdentifier(CodeGenerationSupporter.Remove);
                        GenerateSpace();
                        GenerateSpaceAndFragmentIfNotNull(xmlpath.Name);
                    }
                    else
                    {
                        GenerateSpaceAndKeyword(TSqlTokenType.Add);
                        GenerateSpace();
                        GenerateFragmentIfNotNull(xmlpath);
                    }
                    first = false;
                }
                NewLine();
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
            else
            {
                AlterIndexTypeHelper.Instance.GenerateSourceForOption(_writer, node.AlterIndexType);

                GenerateSpaceAndFragmentIfNotNull(node.Partition);

                if (node.IndexOptions.Count > 0)
                {
                    GenerateSpaceAndKeyword(TSqlTokenType.With);
                    GenerateParenthesisedCommaSeparatedList(node.IndexOptions);
                }
            }
        }
    }
}
