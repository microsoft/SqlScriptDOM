//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.LedgerTableOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
   partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(LedgerTableOption node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Ledger);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);

            if (node.OptionState == OptionState.On)
            { 
                bool ledgerViewSpecified = false;
                bool appendOnlySpecified = false;
                bool comma = false;
                bool options = false;

                GenerateSpaceAndKeyword(TSqlTokenType.On);

                // If Ledger = ON checking for LedgerView & its Options
                //
                if (node.LedgerViewOption != null && node.LedgerViewOption.ViewName != null)
                {
                    GenerateSpaceAndKeyword(TSqlTokenType.LeftParenthesis);

                    if (node.LedgerViewOption.ViewName != null)
                    {
                        GenerateIdentifier(CodeGenerationSupporter.LedgerView);
                        GenerateKeyword(TSqlTokenType.EqualsSign);
                        GenerateFragmentIfNotNull(node.LedgerViewOption.ViewName);
                        ledgerViewSpecified = true;
                    }

                    // Checking for LedgerViewOption - TransactionIdColumnName
                    //
                    if (node.LedgerViewOption.TransactionIdColumnName != null)
                    {
                        GenerateSpaceAndKeyword(TSqlTokenType.LeftParenthesis);
                        GenerateIdentifier(CodeGenerationSupporter.TransactionIdColumnName);
                        GenerateKeyword(TSqlTokenType.EqualsSign);
                        GenerateFragmentIfNotNull(node.LedgerViewOption.TransactionIdColumnName);
                        comma = true;
                        options = true;
                    }

                    // Checking for LedgerViewOption - SequenceNumberColumnName
                    //
                    if (node.LedgerViewOption.SequenceNumberColumnName != null)
                    {
                        if (comma == true)
                        {
                            GenerateKeyword(TSqlTokenType.Comma);
                        }
                        else
                        {
                            GenerateSpaceAndKeyword(TSqlTokenType.LeftParenthesis);
                        }
                        GenerateIdentifier(CodeGenerationSupporter.SequenceNumberColumnName);
                        GenerateKeyword(TSqlTokenType.EqualsSign);
                        GenerateFragmentIfNotNull(node.LedgerViewOption.SequenceNumberColumnName);
                        comma = true;
                        options = true;
                    }

                    // Checking for LedgerViewOption - OperationTypeColumnName
                    //
                    if (node.LedgerViewOption.OperationTypeColumnName != null)
                    {
                        if (comma == true)
                        {
                            GenerateKeyword(TSqlTokenType.Comma);
                        }
                        else
                        {
                            GenerateSpaceAndKeyword(TSqlTokenType.LeftParenthesis);
                        }
                        GenerateIdentifier(CodeGenerationSupporter.OperationTypeColumnName);
                        GenerateKeyword(TSqlTokenType.EqualsSign);
                        GenerateFragmentIfNotNull(node.LedgerViewOption.OperationTypeColumnName);
                        comma = true;
                        options = true;
                    }

                    // Checking for LedgerViewOption - OperationTypeDescColumnName
                    //
                    if (node.LedgerViewOption.OperationTypeDescColumnName != null)
                    {
                        if (comma == true)
                        {
                            GenerateKeyword(TSqlTokenType.Comma);
                        }
                        else
                        {
                            GenerateSpaceAndKeyword(TSqlTokenType.LeftParenthesis);
                        }
                        GenerateIdentifier(CodeGenerationSupporter.OperationTypeDescColumnName);
                        GenerateKeyword(TSqlTokenType.EqualsSign);
                        GenerateFragmentIfNotNull(node.LedgerViewOption.OperationTypeDescColumnName);
                        options = true;
                    }

                    if (options == true)
                    {
                        GenerateKeyword(TSqlTokenType.RightParenthesis);
                    }
                }

                // If Ledger = ON checking if AppendOnly is specified.
                //
                if (node.AppendOnly == OptionState.On || node.AppendOnly == OptionState.Off)
                {
                    appendOnlySpecified = true; 

                    if (ledgerViewSpecified == true)
                    {
                        GenerateKeyword(TSqlTokenType.Comma);
                    }
                    else
                    {
                        GenerateSpaceAndKeyword(TSqlTokenType.LeftParenthesis);
                    }
                    GenerateIdentifier(CodeGenerationSupporter.AppendOnly);
                    GenerateKeyword(TSqlTokenType.EqualsSign);
                    if (node.AppendOnly == OptionState.On)
                    {
                        GenerateSpaceAndKeyword(TSqlTokenType.On);
                    }
                    else
                    {
                        GenerateSpaceAndKeyword(TSqlTokenType.Off);
                    }
                }

                if (ledgerViewSpecified == true || appendOnlySpecified == true)
                {
                    GenerateKeyword(TSqlTokenType.RightParenthesis);
                }
            }
            //Ledger = OFF scenario.
            //
            else
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Off);
            }
        }
    }
}
