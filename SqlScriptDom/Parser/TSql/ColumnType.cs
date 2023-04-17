//------------------------------------------------------------------------------
// <copyright file="ColumnType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Column Types, used for class Column.
    /// </summary>
    public enum ColumnType
    {
        /// <summary>
        /// The column is a regular identifier.
        /// </summary>
        Regular         = 0,
        /// <summary>
        /// IDENTITYCOL was used to define the column.
        /// </summary>
        IdentityCol     = 1,
        /// <summary>
        /// ROWGUIDCOL was used to define the column.
        /// </summary>
        RowGuidCol      = 2,
        /// <summary>
        /// '*' the STAR reserved word was used to define the column.
        /// </summary>
        Wildcard        = 3,
        /// <summary>
        /// $Identity was used to define the column.
        /// </summary>
        PseudoColumnIdentity = 4,
        /// <summary>
        /// $Rowguid was used to define the column.
        /// </summary>
        PseudoColumnRowGuid = 5,
        /// <summary>
        /// $ACTION was used to define the column.
        /// </summary>
        PseudoColumnAction = 6,
        /// <summary>
        /// $CUID was used to define the column.
        /// </summary>
        PseudoColumnCuid   = 7,
        /// <summary>
        /// $node_id was used to define the column.
        /// </summary>
        PseudoColumnGraphNodeId = 8,
        /// <summary>
        /// $edge_id was used to define the column.
        /// </summary>
        PseudoColumnGraphEdgeId = 9,
        /// <summary>
        /// $from_id was used to define the column.
        /// </summary>
        PseudoColumnGraphFromId = 10,
        /// <summary>
        /// $to_id was used to define the column.
        /// </summary>
        PseudoColumnGraphToId = 11,
    }
}
