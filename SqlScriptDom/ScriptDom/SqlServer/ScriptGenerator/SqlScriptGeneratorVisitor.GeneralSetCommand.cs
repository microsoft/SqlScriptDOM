//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.GeneralSetCommand.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<GeneralSetCommandType, TokenGenerator> _generalSetCommandTypeGenerators = new Dictionary<GeneralSetCommandType, TokenGenerator>()
        {
            { GeneralSetCommandType.ContextInfo, new IdentifierGenerator(CodeGenerationSupporter.ContextInfo) },
            { GeneralSetCommandType.DateFirst, new IdentifierGenerator(CodeGenerationSupporter.DateFirst) },
            { GeneralSetCommandType.DateFormat, new IdentifierGenerator(CodeGenerationSupporter.DateFormat) },
            { GeneralSetCommandType.DeadlockPriority, new IdentifierGenerator(CodeGenerationSupporter.DeadlockPriority) },
            { GeneralSetCommandType.Language, new IdentifierGenerator(CodeGenerationSupporter.Language) },
            { GeneralSetCommandType.LockTimeout, new IdentifierGenerator(CodeGenerationSupporter.LockTimeout) },
            { GeneralSetCommandType.None, new EmptyGenerator() },
            { GeneralSetCommandType.QueryGovernorCostLimit, new IdentifierGenerator(CodeGenerationSupporter.QueryGovernorCostLimit) },
        };

        public override void ExplicitVisit(GeneralSetCommand node)
        {
            TokenGenerator generator = GetValueForEnumKey(_generalSetCommandTypeGenerators, node.CommandType);
            if (generator != null)
            {
                GenerateToken(generator);
            }

            // could be
            //      nagative constant
            //      Identifier
            GenerateSpaceAndFragmentIfNotNull(node.Parameter);
        }
    }
}
