//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateSelectiveXmlIndexStatement.cs" company="Microsoft">
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

        public override void ExplicitVisit(CreateSelectiveXmlIndexStatement node)
        {
            //CREATE SELECTIVE XML INDEX
            GenerateKeyword(TSqlTokenType.Create);
            if(!node.IsSecondary)
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Selective);
            
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Xml);
            GenerateSpaceAndKeyword(TSqlTokenType.Index);

            //SXI ON T1(XMLCOL)
            GenerateSpaceAndFragmentIfNotNull(node.Name);
            GenerateSpaceAndKeyword(TSqlTokenType.On);
            GenerateSpaceAndFragmentIfNotNull(node.OnName);
            GenerateKeyword(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.XmlColumn);
            GenerateKeyword(TSqlTokenType.RightParenthesis);

            if (node.IsSecondary)
            {
                NewLine();
                GenerateIdentifier(CodeGenerationSupporter.Using);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Xml);
                GenerateSpaceAndKeyword(TSqlTokenType.Index);
                GenerateSpaceAndFragmentIfNotNull(node.UsingXmlIndexName);
            }
            else
            {
                // XMLNAMESPACES
                if (node.XmlNamespaces != null)
                {
                    NewLine();
                    GenerateKeyword(TSqlTokenType.With);
                    GenerateSpaceAndFragmentIfNotNull(node.XmlNamespaces);
                }
            }

            NewLine();
            GenerateKeyword(TSqlTokenType.For);
            GenerateKeyword(TSqlTokenType.LeftParenthesis);
            GenerateSpaceAndFragmentIfNotNull(node.PathName);
            ListGenerationOption option = ListGenerationOption.CreateOptionFromFormattingConfig(_options);
            option.Parenthesised = false;
            option.AlwaysGenerateParenthesis = false;
            GenerateFragmentList(node.PromotedPaths, option);
            NewLine();
            GenerateKeyword(TSqlTokenType.RightParenthesis);

            // WITH
            if (node.IndexOptions != null && node.IndexOptions.Count > 0)
            {
                NewLine();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpace();

                GenerateParenthesisedCommaSeparatedList(node.IndexOptions);
            }
        }
    }
}
