//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TemporalAsOfClause" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(TemporalClause node)
        {
            GenerateSpaceAndKeyword(TSqlTokenType.For);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.SystemTime);

            // Before start time
            //
            switch (node.TemporalClauseType)
            {
                case TemporalClauseType.AsOf:
                    GenerateSpaceAndKeyword(TSqlTokenType.As);
                    GenerateSpaceAndKeyword(TSqlTokenType.Of);
                    GenerateSpace();
                    break;
                    
                case TemporalClauseType.FromTo:
                    GenerateSpaceAndKeyword(TSqlTokenType.From);
                    GenerateSpace();
                    break;

                case TemporalClauseType.Between:
                    GenerateSpaceAndKeyword(TSqlTokenType.Between);
                    GenerateSpace();
                    break;

                case TemporalClauseType.ContainedIn:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Contained);
                    GenerateSpaceAndKeyword(TSqlTokenType.In);
                    GenerateSpaceAndKeyword(TSqlTokenType.LeftParenthesis);
                    break;

                case TemporalClauseType.TemporalAll:
                    GenerateSpaceAndKeyword(TSqlTokenType.All);
                    break;

                default:
                    Debug.Assert(false, "Unknown enum value");
                    break;    
            }

            if (node.TemporalClauseType == TemporalClauseType.TemporalAll)
            {
                return; // For ALL clause there's nothing more to generate (there is no StartTime)
            }

            GenerateFragmentIfNotNull(node.StartTime);

            if (node.TemporalClauseType == TemporalClauseType.AsOf)
            {
                return; // For AS OF clause there's nothing more to generate.
            }

            // Between start and end time
            //
            switch (node.TemporalClauseType)
            {
                case TemporalClauseType.AsOf:
                    Debug.Assert(false, "This code path should not be reached.");
                    break;

                case TemporalClauseType.FromTo:
                    GenerateSpaceAndKeyword(TSqlTokenType.To);
                    break;

                case TemporalClauseType.Between:
                    GenerateSpaceAndKeyword(TSqlTokenType.And);
                    break;

                case TemporalClauseType.ContainedIn:
                    GenerateKeyword(TSqlTokenType.Comma);
                    break;

                case TemporalClauseType.TemporalAll:
                    Debug.Assert(false, "This code path should not be reached.");
                    break;

                default:
                    Debug.Assert(false, "Unknown enum value.");
                    break;
            }

            GenerateSpaceAndFragmentIfNotNull(node.EndTime);

            // After end time
            //
            if (node.TemporalClauseType == TemporalClauseType.ContainedIn)
            {
                GenerateKeyword(TSqlTokenType.RightParenthesis);
            }
        }
    }
}