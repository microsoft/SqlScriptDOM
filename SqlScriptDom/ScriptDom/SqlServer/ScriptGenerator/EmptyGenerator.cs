//------------------------------------------------------------------------------
// <copyright file="EmptyGenerator.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    internal class EmptyGenerator : TokenGenerator
    {
        public EmptyGenerator()
            : base(false)
        {
        }

        public override void Generate(ScriptWriter writer)
        {
            // generate nothing
        }
    }
}
