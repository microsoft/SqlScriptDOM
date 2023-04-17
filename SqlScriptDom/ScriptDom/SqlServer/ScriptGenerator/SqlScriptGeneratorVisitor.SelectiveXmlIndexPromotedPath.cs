//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateSelectiveXmlIndexPromotedPath.cs" company="Microsoft">
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

        public override void ExplicitVisit(SelectiveXmlIndexPromotedPath node)
        {
            GenerateNameEqualsValue(node.Name.Value, node.Path);
            if (node.XQueryDataType != null || node.MaxLength != null || node.IsSingleton || node.SQLDataType != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.As);
                if ((node.XQueryDataType != null || node.MaxLength != null || node.IsSingleton) && node.SQLDataType == null)
                {
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.XQuery);
                    GenerateSpaceAndFragmentIfNotNull(node.XQueryDataType);
                    if (node.MaxLength != null)
                    {
                        GenerateSpaceAndIdentifier(CodeGenerationSupporter.MaxLength);
                        GenerateKeyword(TSqlTokenType.LeftParenthesis);
                        GenerateFragmentIfNotNull(node.MaxLength);
                        GenerateKeyword(TSqlTokenType.RightParenthesis);
                    }
                }
                else if (node.SQLDataType != null)
                {
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Sql);
                    GenerateSpaceAndFragmentIfNotNull(node.SQLDataType);
                }

                if (node.IsSingleton)
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Singleton);
            }
        }
    }
}
