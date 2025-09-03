//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.FunctionCall.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(LeftFunctionCall node)
        {
            GenerateKeyword(TSqlTokenType.Left);
            GenerateParenthesisedCommaSeparatedList(node.Parameters, true);
            GenerateSpaceAndCollation(node.Collation);
        }

        public override void ExplicitVisit(RightFunctionCall node)
        {
            GenerateKeyword(TSqlTokenType.Right);
            GenerateParenthesisedCommaSeparatedList(node.Parameters, true);
            GenerateSpaceAndCollation(node.Collation);
        }

        public override void ExplicitVisit(FunctionCall node)
        {
            GenerateFragmentIfNotNull(node.CallTarget);
            GenerateFragmentIfNotNull(node.FunctionName);
            GenerateSymbol(TSqlTokenType.LeftParenthesis);

            if (node.FunctionName.Value.ToUpper(CultureInfo.InvariantCulture) == CodeGenerationSupporter.Trim && 
               2 == node.Parameters.Count)
            {
                // If Trimoptions has a saved value. The Syntax is modified to TRIM (Identifier ARG2 FROM ARG3)
                // Trimoptions can only be LEADING/TRAILING/BOTH.
                //
                if (node.TrimOptions != null)
                {
                    GenerateSpace();
                    GenerateFragmentIfNotNull(node.TrimOptions);
                    GenerateSpace();
                }
                GenerateFragmentIfNotNull(node.Parameters[0]);
                GenerateSpace();
                GenerateKeyword(TSqlTokenType.From);
                GenerateSpace();
                GenerateFragmentIfNotNull(node.Parameters[1]);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
            else if (node.FunctionName.Value.ToUpper(CultureInfo.InvariantCulture) == CodeGenerationSupporter.JsonObject)
            {
                GenerateCommaSeparatedList(node.JsonParameters);
                if (node.JsonParameters?.Count > 0 && node.AbsentOrNullOnNull?.Count > 0) //If there are values and null on null or absent on null present then generate space in between them
                    GenerateSpace();
                GenerateNullOnNullOrAbsentOnNull(node?.AbsentOrNullOnNull);
                if (node.JsonParameters?.Count > 0 && node.ReturnType?.Count > 0) //If there are values and null on null or absent on null present then generate space in between them
                    GenerateSpace();
                GenerateReturnType(node?.ReturnType);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
            else if (node.FunctionName.Value.ToUpper(CultureInfo.InvariantCulture) == CodeGenerationSupporter.JsonObjectAgg)
            {
                GenerateCommaSeparatedList(node.JsonParameters);
                if (node.JsonParameters?.Count > 0 && node.AbsentOrNullOnNull?.Count > 0) //If there are values and null on null or absent on null present then generate space in between them
                    GenerateSpace();
                GenerateNullOnNullOrAbsentOnNull(node?.AbsentOrNullOnNull);
                if (node.JsonParameters?.Count > 0 && node.ReturnType?.Count > 0) //If there are values and null on null or absent on null present then generate space in between them
                    GenerateSpace();
                GenerateReturnType(node?.ReturnType);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
            else if (node.FunctionName.Value.ToUpper(CultureInfo.InvariantCulture) == CodeGenerationSupporter.JsonArray)
            {
                GenerateCommaSeparatedList(node.Parameters);
                if (node.Parameters?.Count > 0 && node?.AbsentOrNullOnNull?.Count > 0) //If there are values and null on null or absent on null present then generate space in between them
                    GenerateSpace();
                GenerateNullOnNullOrAbsentOnNull(node?.AbsentOrNullOnNull);
				if (node.ReturnType?.Count > 0) //If there are values and null on null or absent on null present then generate space in between them
                    GenerateSpace();
                GenerateReturnType(node?.ReturnType);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
			else if (node.FunctionName.Value.ToUpper(CultureInfo.InvariantCulture) == CodeGenerationSupporter.JsonArrayAgg)
            {
                GenerateCommaSeparatedList(node.Parameters);
                if (node.Parameters?.Count > 0 && node?.AbsentOrNullOnNull?.Count > 0) //If there are values and null on null or absent on null present then generate space in between them
                    GenerateSpace();
                GenerateNullOnNullOrAbsentOnNull(node?.AbsentOrNullOnNull);
				if (node.ReturnType?.Count > 0) //If there are values and null on null or absent on null present then generate space in between them
                    GenerateSpace();
                GenerateReturnType(node?.ReturnType);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
            else
            {
                GenerateUniqueRowFilter(node.UniqueRowFilter, false);
                if (node.UniqueRowFilter != UniqueRowFilter.NotSpecified && node.Parameters.Count > 0)
                    GenerateSpace();

                GenerateCommaSeparatedList(node.Parameters);
                GenerateSymbol(TSqlTokenType.RightParenthesis);

                if (node.IgnoreRespectNulls?.Count > 0)
                {
                    GenerateSpace();
                    GenerateSpaceSeparatedList(node.IgnoreRespectNulls);
                }

                GenerateSpaceAndFragmentIfNotNull(node.WithinGroupClause);

                GenerateSpaceAndFragmentIfNotNull(node.OverClause);
            }

            GenerateSpaceAndCollation(node.Collation);
        }

        public override void ExplicitVisit(JsonKeyValue pair)
        {
            GenerateFragmentIfNotNull(pair.JsonKeyName);
            //if key is not null, then add colon
            if (pair.JsonKeyName != null)
                GenerateSymbol(TSqlTokenType.Colon);
            GenerateFragmentIfNotNull(pair.JsonValue);
        }

        //Generate Absent on Null or Null on Null
        private void GenerateNullOnNullOrAbsentOnNull(IList<Identifier> list)
        {
            if (list?.Count > 0 && list[0].Value?.ToUpper(CultureInfo.InvariantCulture) == CodeGenerationSupporter.Absent)
            {
                GenerateSpaceSeparatedList(list);
                GenerateSpace();
                GenerateKeyword(TSqlTokenType.On);
                GenerateSpace();
                GenerateKeyword(TSqlTokenType.Null);
            }
            else if (list?.Count > 0 && list[0].Value?.ToUpper(CultureInfo.InvariantCulture) == CodeGenerationSupporter.Null)
            {
                GenerateKeyword(TSqlTokenType.Null);
                GenerateSpace();
                GenerateKeyword(TSqlTokenType.On);
                GenerateSpace();
                GenerateKeyword(TSqlTokenType.Null);
            }
        }
        private void GenerateReturnType(IList<Identifier> list)
        {
            if (list?.Count > 0 && list[0].Value?.ToUpper(CultureInfo.InvariantCulture) == CodeGenerationSupporter.Json)
            {
                GenerateIdentifier("RETURNING");
                GenerateSpace();
                GenerateSpaceSeparatedList(list);
            }
        }
    }
}
