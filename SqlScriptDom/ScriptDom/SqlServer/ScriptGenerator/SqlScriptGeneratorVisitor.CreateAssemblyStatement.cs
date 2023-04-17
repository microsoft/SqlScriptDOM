//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateAssemblyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<PermissionSetOption, String> _permissionSetOptionNames = new Dictionary<PermissionSetOption, String>()
        {
            {PermissionSetOption.ExternalAccess, CodeGenerationSupporter.ExternalAccess},
            {PermissionSetOption.Safe, CodeGenerationSupporter.Safe},
            {PermissionSetOption.Unsafe, CodeGenerationSupporter.Unsafe},
        };

        public override void ExplicitVisit(CreateAssemblyStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Assembly);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateOwnerIfNotNull(node.Owner);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.From);
            GenerateSpace();

            GenerateCommaSeparatedList(node.Parameters);

            GenerateAssemblyOptions(node.Options);
        }
    }
}
