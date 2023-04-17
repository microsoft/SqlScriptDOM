//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterAssemblyStatement.cs" company="Microsoft">
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
        public override void ExplicitVisit(AlterAssemblyStatement node)
        {
            GenerateSpaceSeparatedTokens(TSqlTokenType.Alter, CodeGenerationSupporter.Assembly);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // FROM
            if (node.Parameters.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.From);
                GenerateSpaceAndFragmentIfNotNull(node.Parameters[0]); // only one file allowed in FROM clause in ALTER ASSEMBLY
            }
            GenerateAssemblyOptions(node.Options);

            // DROP FILE
            if (node.IsDropAll || node.DropFiles.Count > 0)
            {
                NewLineAndIndent();
                GenerateSpaceSeparatedTokens(TSqlTokenType.Drop, TSqlTokenType.File);
                if (node.IsDropAll)
                {
                    GenerateSpaceAndKeyword(TSqlTokenType.All);
                }
                else
                {
                    GenerateSpace();
                    GenerateCommaSeparatedList(node.DropFiles);
                }
            }

            // ADD FILE
            if (node.AddFiles.Count > 0)
            {
                NewLineAndIndent();
                GenerateSpaceSeparatedTokens(TSqlTokenType.Add, TSqlTokenType.File, TSqlTokenType.From);
                GenerateSpace();
                GenerateCommaSeparatedList(node.AddFiles);
            }
        }

        internal void GenerateAssemblyOptions(IList<AssemblyOption> options)
        {
            if (options != null && options.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeywordAndSpace(TSqlTokenType.With);
                GenerateCommaSeparatedList(options);
            }
        }

        public override void ExplicitVisit(AssemblyOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == AssemblyOptionKind.UncheckedData);
            GenerateSpaceSeparatedTokens(CodeGenerationSupporter.Unchecked, CodeGenerationSupporter.Data);
        }

        public override void ExplicitVisit(PermissionSetAssemblyOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == AssemblyOptionKind.PermissionSet);
            String optionName = GetValueForEnumKey(_permissionSetOptionNames, node.PermissionSetOption);
            GenerateNameEqualsValue(CodeGenerationSupporter.PermissionSet, optionName);
        }

        public override void ExplicitVisit(OnOffAssemblyOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == AssemblyOptionKind.Visibility);
            GenerateOptionStateWithEqualSign(CodeGenerationSupporter.Visibility, node.OptionState);
        }
    }
}
