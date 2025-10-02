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
                // Generate ORDER BY clause if present
                GenerateSpaceAndFragmentIfNotNull(node.JsonOrderByClause);
                if (node.Parameters?.Count > 0 && node?.AbsentOrNullOnNull?.Count > 0) //If there are values and null on null or absent on null present then generate space in between them
                    GenerateSpace();
                GenerateNullOnNullOrAbsentOnNull(node?.AbsentOrNullOnNull);
				if (node.ReturnType?.Count > 0) //If there are values and null on null or absent on null present then generate space in between them
                    GenerateSpace();
                GenerateReturnType(node?.ReturnType);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
            else if (node.FunctionName.Value.ToUpper(CultureInfo.InvariantCulture) == CodeGenerationSupporter.JsonQuery)
            {
                GenerateCommaSeparatedList(node.Parameters);
                
                // Handle WITH ARRAY WRAPPER clause - inside parentheses
                if (node.WithArrayWrapper)
                {
                    GenerateSpace();
                    GenerateKeyword(TSqlTokenType.With);
                    GenerateSpace();
                    GenerateIdentifier(CodeGenerationSupporter.Array);
                    GenerateSpace();
                    GenerateIdentifier(CodeGenerationSupporter.Wrapper);
                }
                
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
            else if (node.FunctionName.Value.ToUpper(CultureInfo.InvariantCulture) == CodeGenerationSupporter.JsonValue)
            {
                GenerateCommaSeparatedList(node.Parameters);
                if (node.ReturnType?.Count > 0) //If there are return types then generate space and return type clause
                {
                    GenerateSpace();
                    GenerateReturnType(node?.ReturnType);
                }
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

        // Generate returning clause with SQLType.
        private void GenerateReturnType(IList<DataTypeReference> list)
        {
            if (list?.Count > 0)
            {
                GenerateIdentifier("RETURNING");
                GenerateSpace();

                // Generate each data type correctly
                for (int i = 0; i < list.Count; i++)
                {
                    if (i > 0)
                        GenerateSpace();

                    // Handle SqlDataTypeReference properly - need to generate the type name and parameters separately
                    if (list[i] is SqlDataTypeReference sqlDataType)
                    {
                        // Generate the data type name (e.g., NVARCHAR)
                        string dataTypeName = sqlDataType.SqlDataTypeOption.ToString().ToUpper(CultureInfo.InvariantCulture);
                        GenerateIdentifier(dataTypeName);

                        // Generate parameters if any (e.g., (50))
                        if (sqlDataType.Parameters?.Count > 0)
                        {
                            GenerateSymbol(TSqlTokenType.LeftParenthesis);
                            for (int j = 0; j < sqlDataType.Parameters.Count; j++)
                            {
                                if (j > 0)
                                    GenerateSymbol(TSqlTokenType.Comma);
                                GenerateFragmentIfNotNull(sqlDataType.Parameters[j]);
                            }
                            GenerateSymbol(TSqlTokenType.RightParenthesis);
                        }
                    }
                    else
                    {
                        // For other data type references, use the default generation
                        GenerateFragmentIfNotNull(list[i]);
                    }
                }
            }
        }
    }
}
