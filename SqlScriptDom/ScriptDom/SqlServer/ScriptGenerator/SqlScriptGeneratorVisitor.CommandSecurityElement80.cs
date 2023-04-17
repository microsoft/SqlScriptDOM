//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CommandSecurityElement80.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        private static readonly Array _commandOptions = Enum.GetValues(typeof(CommandOptions));

        public override void ExplicitVisit(CommandSecurityElement80 node)
        {
            if (node.All)
            {
                GenerateKeyword(TSqlTokenType.All);
            }
            else
            {
                bool first = true;
                foreach (CommandOptions option in _commandOptions)
                {
                    if (option != CommandOptions.None && (node.CommandOptions & option) == option)
                    {
                        if (first == true)
                        {
                            first = false;
                        }
                        else
                        {
                            GenerateSymbol(TSqlTokenType.Comma);
                            GenerateSpace();
                        }

                        GenerateCommandOptions(option);
                    }
                }
            }
        }

        private void GenerateCommandOptions(CommandOptions option)
        {
            switch (option)
            {
                case CommandOptions.CreateDatabase:
                    GenerateKeyword(TSqlTokenType.Create);
                    GenerateSpaceAndKeyword(TSqlTokenType.Database);
                    break;
                case CommandOptions.CreateDefault:
                    GenerateKeyword(TSqlTokenType.Create);
                    GenerateSpaceAndKeyword(TSqlTokenType.Default);
                    break;
                case CommandOptions.CreateProcedure:
                    GenerateKeyword(TSqlTokenType.Create);
                    GenerateSpaceAndKeyword(TSqlTokenType.Procedure);
                    break;
                case CommandOptions.CreateFunction:
                    GenerateKeyword(TSqlTokenType.Create);
                    GenerateSpaceAndKeyword(TSqlTokenType.Function);
                    break;
                case CommandOptions.CreateRule:
                    GenerateKeyword(TSqlTokenType.Create);
                    GenerateSpaceAndKeyword(TSqlTokenType.Rule);
                    break;
                case CommandOptions.CreateTable:
                    GenerateKeyword(TSqlTokenType.Create);
                    GenerateSpaceAndKeyword(TSqlTokenType.Table);
                    break;
                case CommandOptions.CreateView:
                    GenerateKeyword(TSqlTokenType.Create);
                    GenerateSpaceAndKeyword(TSqlTokenType.View);
                    break;
                case CommandOptions.BackupDatabase:
                    GenerateKeyword(TSqlTokenType.Backup);
                    GenerateSpaceAndKeyword(TSqlTokenType.Database);
                    break;
                case CommandOptions.BackupLog:
                    GenerateKeyword(TSqlTokenType.Backup);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Log);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }
    }
}
