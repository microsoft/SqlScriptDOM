//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ReadTextStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ReadTextStatement node)
        {
            GenerateKeyword(TSqlTokenType.ReadText);
            GenerateSpaceAndFragmentIfNotNull(node.Column);
            GenerateSpaceAndFragmentIfNotNull(node.TextPointer);
            GenerateSpaceAndFragmentIfNotNull(node.Offset);
            GenerateSpaceAndFragmentIfNotNull(node.Size);
            if (node.HoldLock)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.HoldLock); 
            }
        }
    }
}
