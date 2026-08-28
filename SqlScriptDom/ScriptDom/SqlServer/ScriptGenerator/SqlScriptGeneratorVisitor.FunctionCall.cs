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
        private int _multilineFunctionCallDepth;

        public override void ExplicitVisit(LeftFunctionCall node)
        {
            if (!ShouldFormatFunctionCallParameterList(node.Parameters))
            {
                if (!TryGenerateBuiltInFunctionName(TSqlTokenType.Left))
                {
                    GenerateKeyword(TSqlTokenType.Left);
                }
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateCommaSeparatedList(node.Parameters);
                GenerateFunctionCallRightParenthesis();
                GenerateSpaceAndCollation(node.Collation);
                return;
            }

            AlignmentPoint functionCallStart = PushFunctionCallAlignmentPoint();
            try
            {
                if (!TryGenerateBuiltInFunctionName(TSqlTokenType.Left))
                {
                    GenerateKeyword(TSqlTokenType.Left);
                }
                GenerateMultilineFunctionCallParameterList(node.Parameters);
                GenerateSpaceAndCollation(node.Collation);
            }
            finally
            {
                PopFunctionCallAlignmentPoint(functionCallStart);
            }
        }

        public override void ExplicitVisit(RightFunctionCall node)
        {
            if (!ShouldFormatFunctionCallParameterList(node.Parameters))
            {
                if (!TryGenerateBuiltInFunctionName(TSqlTokenType.Right))
                {
                    GenerateKeyword(TSqlTokenType.Right);
                }
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateCommaSeparatedList(node.Parameters);
                GenerateFunctionCallRightParenthesis();
                GenerateSpaceAndCollation(node.Collation);
                return;
            }

            AlignmentPoint functionCallStart = PushFunctionCallAlignmentPoint();
            try
            {
                if (!TryGenerateBuiltInFunctionName(TSqlTokenType.Right))
                {
                    GenerateKeyword(TSqlTokenType.Right);
                }
                GenerateMultilineFunctionCallParameterList(node.Parameters);
                GenerateSpaceAndCollation(node.Collation);
            }
            finally
            {
                PopFunctionCallAlignmentPoint(functionCallStart);
            }
        }

        public override void ExplicitVisit(FunctionCall node)
        {
            if (ShouldFormatFunctionCall(node))
            {
                GenerateMultilineFunctionCall(node);
                return;
            }

            GenerateFragmentIfNotNull(node.CallTarget);

            // Recognized, unqualified built-in function names follow the BuiltInFunctionCasing
            // option. When that option is Preserve (the default) or the name is a user-defined
            // function, fall back to emitting the name with its original casing.
            //
            // Function names are not affected by the IdentifierCasing / IdentifierBracketing options
            // (their casing is governed by BuiltInFunctionCasing), so emit the function name with
            // identifier formatting suppressed. This has no effect under default options.
            if (!TryGenerateBuiltInFunctionName(node))
            {
                GenerateWithoutIdentifierFormatting(() => GenerateFragmentIfNotNull(node.FunctionName));
            }

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
                    // LEADING/TRAILING/BOTH are stored as an Identifier fragment but are keywords, so
                    // they must not be bracketed or recased by the identifier options.
                    GenerateWithoutIdentifierFormatting(() => GenerateFragmentIfNotNull(node.TrimOptions));
                    GenerateSpace();
                }
                GenerateFragmentIfNotNull(node.Parameters[0]);
                if (HasDeferredTrailingSingleLineComments)
                {
                    NewLine();
                }
                else
                {
                    GenerateSpace();
                }
                GenerateKeyword(TSqlTokenType.From);
                GenerateSpace();
                GenerateFragmentIfNotNull(node.Parameters[1]);
                GenerateFunctionCallRightParenthesis();
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
                GenerateFunctionCallRightParenthesis();
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
                GenerateFunctionCallRightParenthesis();
                // Generate OVER clause for windowed json_objectagg
                GenerateSpaceAndFragmentIfNotNull(node.OverClause);
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
                GenerateFunctionCallRightParenthesis();
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
                GenerateFunctionCallRightParenthesis();
                // Generate OVER clause for windowed json_arrayagg
                GenerateSpaceAndFragmentIfNotNull(node.OverClause);
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
                
                GenerateFunctionCallRightParenthesis();
            }
            else if (node.FunctionName.Value.ToUpper(CultureInfo.InvariantCulture) == CodeGenerationSupporter.JsonValue)
            {
                GenerateCommaSeparatedList(node.Parameters);
                if (node.ReturnType?.Count > 0) //If there are return types then generate space and return type clause
                {
                    GenerateSpace();
                    GenerateReturnType(node?.ReturnType);
                }
                GenerateFunctionCallRightParenthesis();
            }
            else
            {
                GenerateUniqueRowFilter(node.UniqueRowFilter, false);
                if (node.UniqueRowFilter != UniqueRowFilter.NotSpecified && node.Parameters.Count > 0)
                    GenerateSpace();

                GenerateCommaSeparatedList(node.Parameters);
                GenerateFunctionCallRightParenthesis();

                if (node.IgnoreRespectNulls?.Count > 0)
                {
                    GenerateSpace();
                    // IGNORE/RESPECT NULLS are stored as Identifier fragments but are keywords, so they
                    // must not be bracketed or recased by the identifier options.
                    GenerateWithoutIdentifierFormatting(() => GenerateSpaceSeparatedList(node.IgnoreRespectNulls));
                }

                GenerateSpaceAndFragmentIfNotNull(node.WithinGroupClause);

                GenerateSpaceAndFragmentIfNotNull(node.OverClause);
            }

            GenerateSpaceAndCollation(node.Collation);
        }

        /// <summary>
        /// Generates an ordinary function call using multiline parameter formatting while preserving
        /// its optional null-handling, grouping, windowing, and collation clauses.
        /// </summary>
        private void GenerateMultilineFunctionCall(FunctionCall node)
        {
            AlignmentPoint functionCallStart = PushFunctionCallAlignmentPoint();
            try
            {
                GenerateFragmentIfNotNull(node.CallTarget);
                if (!TryGenerateBuiltInFunctionName(node))
                {
                    GenerateWithoutIdentifierFormatting(() => GenerateFragmentIfNotNull(node.FunctionName));
                }

                GenerateMultilineFunctionCallParameterList(node.Parameters);

                if (node.IgnoreRespectNulls?.Count > 0)
                {
                    GenerateSpace();
                    GenerateWithoutIdentifierFormatting(() => GenerateSpaceSeparatedList(node.IgnoreRespectNulls));
                }

                GenerateSpaceAndFragmentIfNotNull(node.WithinGroupClause);
                GenerateSpaceAndFragmentIfNotNull(node.OverClause);
                GenerateSpaceAndCollation(node.Collation);
            }
            finally
            {
                PopFunctionCallAlignmentPoint(functionCallStart);
            }
        }

        /// <summary>
        /// Determines whether to render a function's parameters with the generic multiline layout.
        /// The option must be enabled, the function must use an ordinary comma-separated parameter
        /// list without a DISTINCT or ALL qualifier, and the function must either be inside an
        /// already-formatted nested call tree or contain another function in its parameter subtree.
        /// </summary>
        private bool ShouldFormatFunctionCall(FunctionCall node)
        {
            return node.UniqueRowFilter == UniqueRowFilter.NotSpecified
                && !UsesSpecialFunctionCallSyntax(node)
                && ShouldFormatFunctionCallParameterList(node.Parameters);
        }

        /// <summary>
        /// Determines whether a function contains nonstandard argument grammar, such as TRIM's FROM
        /// syntax or JSON-specific key/value, null-handling, ordering, wrapper, and RETURNING clauses.
        /// Such functions must bypass generic comma-separated multiline parameter rendering to
        /// preserve valid SQL.
        /// </summary>
        private static bool UsesSpecialFunctionCallSyntax(FunctionCall node)
        {
            string functionName = node.FunctionName.Value.ToUpper(CultureInfo.InvariantCulture);
            return (functionName == CodeGenerationSupporter.Trim && node.Parameters.Count == 2)
                || functionName == CodeGenerationSupporter.JsonObject
                || functionName == CodeGenerationSupporter.JsonObjectAgg
                || functionName == CodeGenerationSupporter.JsonArray
                || functionName == CodeGenerationSupporter.JsonArrayAgg
                || functionName == CodeGenerationSupporter.JsonQuery
                || functionName == CodeGenerationSupporter.JsonValue;
        }

        /// <summary>
        /// Determines whether a parameter list belongs to the nested function tree that should use
        /// multiline formatting. The option must be enabled, and the list must either be inside an
        /// already-formatted function call or contain another function in its parameter subtree.
        /// This overload is also used by LEFT and RIGHT, which have dedicated AST node types.
        /// </summary>
        private bool ShouldFormatFunctionCallParameterList<T>(IList<T> parameters) where T : TSqlFragment
        {
            return _options.MultilineNestedFunctionCalls
                && parameters.Count > 0
                && (_multilineFunctionCallDepth > 0 || ContainsFunctionCall(parameters));
        }

        /// <summary>
        /// Anchors the outermost multiline function call so new lines return to its starting column.
        /// Nested calls reuse that alignment scope.
        /// </summary>
        private AlignmentPoint PushFunctionCallAlignmentPoint()
        {
            if (_multilineFunctionCallDepth > 0)
            {
                return null;
            }

            var functionCallStart = new AlignmentPoint();
            MarkAndPushAlignmentPointKeepingNameScope(functionCallStart);
            return functionCallStart;
        }

        /// <summary>
        /// Removes the alignment scope created for an outermost multiline function call.
        /// </summary>
        private void PopFunctionCallAlignmentPoint(AlignmentPoint functionCallStart)
        {
            if (functionCallStart != null)
            {
                PopAlignmentPoint();
            }
        }

        /// <summary>
        /// Generates a multiline parameter list and tracks nested calls. A sole non-function
        /// parameter remains beside the opening parenthesis.
        /// </summary>
        private void GenerateMultilineFunctionCallParameterList<T>(IList<T> parameters) where T : TSqlFragment
        {
            _multilineFunctionCallDepth++;
            try
            {
                ListGenerationOption option = ListGenerationOption.CreateMultilineFunctionCallOption(_options);
                if (parameters.Count == 1 && !ContainsFunctionCall(parameters))
                {
                    option.NewLineAfterOpenParenthesis = false;
                    option.NewLineBeforeCloseParenthesis = false;
                    option.NewLineBeforeItems = false;
                    option.MultipleIndentItems = 0;
                }

                GenerateFragmentList(parameters, option);
            }
            finally
            {
                _multilineFunctionCallDepth--;
            }
        }

        /// <summary>
        /// Determines whether any parameter subtree contains a supported function-call node.
        /// </summary>
        private static bool ContainsFunctionCall<T>(IList<T> parameters) where T : TSqlFragment
        {
            var visitor = new FunctionCallFindingVisitor();
            foreach (T parameter in parameters)
            {
                parameter.Accept(visitor);
                if (visitor.Found)
                {
                    return true;
                }
            }

            return false;
        }

        private sealed class FunctionCallFindingVisitor : TSqlFragmentVisitor
        {
            public bool Found { get; private set; }

            public override void Visit(FunctionCall node)
            {
                Found = true;
            }

            public override void ExplicitVisit(LeftFunctionCall node)
            {
                Found = true;
            }

            public override void ExplicitVisit(RightFunctionCall node)
            {
                Found = true;
            }
        }

        /// <summary>
        /// Generates a closing parenthesis on a new line when required to terminate a deferred
        /// trailing single-line comment first.
        /// </summary>
        private void GenerateFunctionCallRightParenthesis()
        {
            if (HasDeferredTrailingSingleLineComments)
            {
                NewLine();
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis);
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
                // ABSENT is stored as an Identifier fragment but is a keyword, so it must not be
                // bracketed or recased by the identifier options ('[ABSENT] ON NULL' is invalid).
                GenerateWithoutIdentifierFormatting(() => GenerateSpaceSeparatedList(list));
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
