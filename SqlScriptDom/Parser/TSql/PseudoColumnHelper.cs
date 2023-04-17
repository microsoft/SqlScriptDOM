//------------------------------------------------------------------------------
// <copyright file="PseudoColumnHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class PseudoColumnHelper : OptionsHelper<ColumnType>
    {
        private PseudoColumnHelper()
        {
            AddOptionMapping(ColumnType.PseudoColumnIdentity, CodeGenerationSupporter.DollarSign + CodeGenerationSupporter.Identity, SqlVersionFlags.TSqlAll);
            AddOptionMapping(ColumnType.PseudoColumnRowGuid, CodeGenerationSupporter.DollarSign + CodeGenerationSupporter.Rowguid, SqlVersionFlags.TSqlAll);

            AddOptionMapping(ColumnType.PseudoColumnAction, CodeGenerationSupporter.DollarSign + CodeGenerationSupporter.Action, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(ColumnType.PseudoColumnCuid, CodeGenerationSupporter.DollarSign + CodeGenerationSupporter.Cuid, SqlVersionFlags.TSql100AndAbove);

            AddOptionMapping(ColumnType.PseudoColumnGraphNodeId, CodeGenerationSupporter.DollarSign + CodeGenerationSupporter.GraphNodeId, SqlVersionFlags.TSql140AndAbove);
            AddOptionMapping(ColumnType.PseudoColumnGraphEdgeId, CodeGenerationSupporter.DollarSign + CodeGenerationSupporter.GraphEdgeId, SqlVersionFlags.TSql140AndAbove);
            AddOptionMapping(ColumnType.PseudoColumnGraphFromId, CodeGenerationSupporter.DollarSign + CodeGenerationSupporter.GraphFromId, SqlVersionFlags.TSql140AndAbove);
            AddOptionMapping(ColumnType.PseudoColumnGraphToId, CodeGenerationSupporter.DollarSign + CodeGenerationSupporter.GraphToId, SqlVersionFlags.TSql140AndAbove);
        }

        internal static readonly PseudoColumnHelper Instance = new PseudoColumnHelper();
    }
}
