//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AIGenerateFixedChunksTableReference.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
	partial class SqlScriptGeneratorVisitor
	{
		public override void ExplicitVisit(AIGenerateFixedChunksTableReference node)
		{
			List<(string, TSqlFragment)> specificParameters = new List<(string, TSqlFragment)>();
			specificParameters.Add((CodeGenerationSupporter.ChunkSize, node.ChunkSize));

			if (node.Overlap != null)
			{
				specificParameters.Add((CodeGenerationSupporter.Overlap, node.Overlap));
			}

			if (node.EnableChunkSetId != null)
			{
				specificParameters.Add((CodeGenerationSupporter.EnableChunkSetId, node.EnableChunkSetId));
			}

			GenerateAIChunkTableReference(node, specificParameters);
		}
	}
}
