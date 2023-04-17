//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.PartnerDatabaseOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(PartnerDatabaseOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == DatabaseOptionKind.Partner);
            GenerateIdentifier(CodeGenerationSupporter.Partner);
            GenerateSpace();

            if (node.PartnerServer != null)
            {
                GenerateSymbol(TSqlTokenType.EqualsSign);
                GenerateSpaceAndFragmentIfNotNull(node.PartnerServer);
            }
            else if (node.PartnerOption == PartnerDatabaseOptionKind.SafetyFull)
            {
                GenerateIdentifier(CodeGenerationSupporter.Safety);
                GenerateSpaceAndKeyword(TSqlTokenType.Full);
            }
            else if (node.PartnerOption == PartnerDatabaseOptionKind.SafetyOff)
            {
                GenerateIdentifier(CodeGenerationSupporter.Safety);
                GenerateSpaceAndKeyword(TSqlTokenType.Off);
            }
            else
            {
                PartnerDbOptionsHelper.Instance.GenerateSourceForOption(_writer, node.PartnerOption);
            }

            if (node.PartnerOption == PartnerDatabaseOptionKind.Timeout && node.Timeout != null)
            {
                GenerateSpaceAndFragmentIfNotNull(node.Timeout);
            }

        }
    }
}
