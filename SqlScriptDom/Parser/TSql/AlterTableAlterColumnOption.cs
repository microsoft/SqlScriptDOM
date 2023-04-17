//------------------------------------------------------------------------------
// <copyright file="AlterTableAlterColumnOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The options for alter column version of alter table statement.
    /// AddRowguidcol, DropRowguidcol can only happen if there is no
    /// DataType.
    /// </summary>
    public enum AlterTableAlterColumnOption
    {
        /// <summary>
        /// No option defined
        /// </summary>
        NoOptionDefined = 0,
        /// <summary>
        /// Add Rowguidcol for the column.
        /// </summary>
        AddRowGuidCol = 1,
        /// <summary>
        /// Drop Rowguidcol for the column.
        /// </summary>
        DropRowGuidCol = 2,
        /// <summary>
        /// Null was defined.
        /// </summary>
        Null = 3,
        /// <summary>
        /// Not null was defined.
        /// </summary>
        NotNull = 4,
        /// <summary>
        /// Add persisted for the column.
        /// </summary>
        AddPersisted = 5,
        /// <summary>
        /// Drop persisted for the column.
        /// </summary>
        DropPersisted = 6,
        /// <summary>
        /// Add not for replication for the column.
        /// </summary>
        AddNotForReplication = 7,
        /// <summary>
        /// Drop not for replication for the column.
        /// </summary>
        DropNotForReplication = 8,
        /// <summary>
        /// Add Sparse for the column.
        /// </summary>
        AddSparse = 9,
        /// <summary>
        /// Drop Sparse for the column.
        /// </summary>
        DropSparse = 10,
        /// <summary>
        /// Add Masking Function for the column.
        /// </summary>
        AddMaskingFunction = 11,
        /// <summary>
        /// Drop Masking Function for the column.
        /// </summary>
        DropMaskingFunction = 12,
        /// <summary>
        /// Add hidden flag to temporal generated always column
        /// </summary>
        AddHidden = 13,
        /// <summary>
        /// Drops hidden flag to temporal generated always column
        /// </summary>
        DropHidden = 14,
        /// <summary>
        /// Add Always Encryption details for column
        /// </summary>
        Encryption = 15
    }
}
