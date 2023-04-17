//------------------------------------------------------------------------------
// <copyright file="IndexAffectingStatements.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// List of statement which can affect index
    /// Used to check that only valid options/table elements were specified
    /// </summary>
    
    internal enum IndexAffectingStatement
    {
        AlterTableAddElement,
        AlterTableAlterIndexRebuild,
        AlterTableRebuildOnePartition,
        AlterTableRebuildAllPartitions,
        AlterIndexSet,
        AlterIndexRebuildOnePartition,
        AlterIndexRebuildAllPartitions,
        AlterIndexReorganize,
        CreateColumnStoreIndex,
        CreateIndex,
        CreateTable,
        CreateType,
        CreateXmlIndex,
        CreateOrAlterFunction,
        DeclareTableVariable,
        CreateSpatialIndex,
        CreateTableInlineIndex,
        AlterTableAlterColumn,
        AlterIndexResume
        // DROP INDEX handled separately - there are just two options to check for...
        //DropIndex,
    }
}
