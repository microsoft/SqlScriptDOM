//------------------------------------------------------------------------------
// <copyright company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        // Generates the statement
        // DROP EXTERNAL MODEL [{node.Name}]
        //
        public override void ExplicitVisit(DropExternalModelStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.External);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Model);

            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }
    }
}
