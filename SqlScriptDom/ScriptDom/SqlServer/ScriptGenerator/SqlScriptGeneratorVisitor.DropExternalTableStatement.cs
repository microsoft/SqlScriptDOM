//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropCertificateStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropExternalTableStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.External);
            GenerateSpaceAndKeyword(TSqlTokenType.Table);

            GenerateSpace();
            GenerateCommaSeparatedList(node.Objects);
        }
    }
}
