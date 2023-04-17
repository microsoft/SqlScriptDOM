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
        // DROP EXTERNAL LANGUAGE [{node.Name}]
        //     AUTHORIZATION [{node.Owner}]
        //
        public override void ExplicitVisit(DropExternalLanguageStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.External);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Language);

            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateOwnerIfNotNull(node.Owner);
        }
    }
}
