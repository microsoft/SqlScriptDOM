//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterDbModifyFilegroupStatement.cs" company="Microsoft">
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
        public override void ExplicitVisit(AlterDatabaseModifyFileGroupStatement node)
        {
            GenerateAlterDbStatementHead(node);

            NewLineAndIndent();
            GenerateIdentifier(CodeGenerationSupporter.Modify);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Filegroup);
            GenerateSpaceAndFragmentIfNotNull(node.FileGroup);

            GenerateSpace();

            if (node.NewFileGroupName != null)
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.Name, node.NewFileGroupName);
            }
            else if (node.MakeDefault)
            {
                GenerateKeyword(TSqlTokenType.Default);
            }
            else if (node.UpdatabilityOption != ModifyFileGroupOption.None)
            {
                ModifyFilegroupOptionsHelper.Instance.GenerateSourceForOption(_writer, node.UpdatabilityOption);
                GenerateSpaceAndFragmentIfNotNull(node.Termination);
            }
        }
    }
}
