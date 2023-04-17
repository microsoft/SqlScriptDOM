//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterSchemaStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterSchemaStatement node)
        {
            GenerateKeyword(TSqlTokenType.Alter);
            GenerateSpaceAndKeyword(TSqlTokenType.Schema);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Transfer); 

            if (node.ObjectKind != SecurityObjectKind.NotSpecified)
            {
                GenerateSpace();
                GenerateSourceForSecurityObjectKind(node.ObjectKind);
            }

            GenerateSpaceAndFragmentIfNotNull(node.ObjectName);
        }
    }
}
