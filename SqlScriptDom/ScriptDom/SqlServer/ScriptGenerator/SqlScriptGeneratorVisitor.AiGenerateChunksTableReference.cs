//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AIGenerateChunksTableReference.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
	partial class SqlScriptGeneratorVisitor
	{
		public void GenerateNameValuePairs(IList<(string Name, TSqlFragment Value)> pairs)
		{
			for (int index = 0; index < pairs.Count; index++)
			{
				(string name, TSqlFragment value) = pairs[index];

				if (index > 0)
				{
					GenerateSymbol(TSqlTokenType.Comma);
					GenerateSpace();
				}

				GenerateIdentifierWithoutCasing(name);
				GenerateSpace();
				GenerateSymbol(TSqlTokenType.EqualsSign);
				GenerateSpace();
				GenerateFragmentIfNotNull(value);
			}
		}

		public void GenerateAIChunkTableReference(AIGenerateChunksTableReference node, IList<(string, TSqlFragment)> additionalParameters)
		{
			GenerateIdentifierWithoutCasing(CodeGenerationSupporter.AiGenerateChunks);
			GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);

			// Include common parameters: source, chunk_type
			List<(string, TSqlFragment)> parameters = new List<(string, TSqlFragment)>();
			parameters.Add((CodeGenerationSupporter.Source, node.Source));
			parameters.Add((CodeGenerationSupporter.ChunkType, node.ChunkType));
			parameters.AddRange(additionalParameters);

			GenerateNameValuePairs(parameters);

			GenerateSymbol(TSqlTokenType.RightParenthesis);

			GenerateSpaceAndAlias(node.Alias);
		}
	}
}
