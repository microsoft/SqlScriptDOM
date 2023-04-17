//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ServiceContract.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ServiceContract node)
        {
            switch (node.Action)
            {
                case AlterAction.None:
                    break;
                case AlterAction.Add:
                    GenerateKeyword(TSqlTokenType.Add);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Contract);
                    GenerateSpace();
                    break;
                case AlterAction.Drop:
                    GenerateKeyword(TSqlTokenType.Drop);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Contract);
                    GenerateSpace();
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }

            GenerateFragmentIfNotNull(node.Name);
        }
    }
}
