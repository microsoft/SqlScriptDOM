//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SearchPropertyListAction.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AddSearchPropertyListAction node)
        {
            GenerateKeyword(TSqlTokenType.Add);
            GenerateSpaceAndFragmentIfNotNull(node.PropertyName);
            GenerateSpaceAndKeyword(TSqlTokenType.With);
            GenerateSpaceAndKeyword(TSqlTokenType.LeftParenthesis);
            
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.PropertySetGuid);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.Guid);
            GenerateKeyword(TSqlTokenType.Comma);

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.PropertyIntId);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.Id);

            if (node.Description != null)
            {
                GenerateKeyword(TSqlTokenType.Comma);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.PropertyDescription);
                GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
                GenerateSpaceAndFragmentIfNotNull(node.Description);
            }
            GenerateSpaceAndKeyword(TSqlTokenType.RightParenthesis);
        }

        public override void ExplicitVisit(DropSearchPropertyListAction node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndFragmentIfNotNull(node.PropertyName);
        }
    }
}
