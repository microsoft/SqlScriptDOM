//------------------------------------------------------------------------------
// <copyright file="SqlVersionFlags.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Used in conjuction with OptionHelper class to specify, in what T-SQL version option is supported
    /// </summary>
    [Flags]
    internal enum SqlVersionFlags
    {
        None        = 0x0,
        TSql80      = 0x01,
        TSql90      = 0x02,
        TSql100     = 0x04,
        TSql110     = 0x08,
        TSql120     = 0x10,
        TSql130     = 0x20,
        TSql140     = 0x40,
        TSql150     = 0x80,
        TSql160     = 0x100,
        TSql170     = 0x200,
        TSqlFabricDW = 0x400,

        TSqlAll         = TSql80 | TSql90 | TSql100 | TSql110 | TSql120 | TSql130 | TSql140 | TSql150 | TSql160 | TSqlFabricDW | TSql170,
        TSql90AndAbove  = TSql90 | TSql100 | TSql110 | TSql120 | TSql130 | TSql140 | TSql150 | TSql160 | TSqlFabricDW | TSql170,
        TSql100AndAbove = TSql100 | TSql110 | TSql120 | TSql130 | TSql140 | TSql150 | TSql160 | TSqlFabricDW | TSql170,
        TSql110AndAbove = TSql110 | TSql120 | TSql130 | TSql140 | TSql150 | TSql160 | TSqlFabricDW | TSql170,
        TSql120AndAbove = TSql120 | TSql130 | TSql140 | TSql150 | TSql160 | TSqlFabricDW | TSql170,
        TSql130AndAbove = TSql130 | TSql140 | TSql150 | TSql160 | TSqlFabricDW | TSql170,
        TSql140AndAbove = TSql140 | TSql150 | TSql160 | TSqlFabricDW | TSql170,
        TSql150AndAbove = TSql150 | TSql160 | TSqlFabricDW | TSql170,
        TSql160AndAbove = TSql160 | TSqlFabricDW | TSql170,
        TSqlFabricDWAndAbove = TSql160 | TSqlFabricDW | TSql170,
        TSqlUnder110 = TSql80 | TSql90 | TSql100,
        TSqlUnder120 = TSql80 | TSql90 | TSql100 | TSql110,
        TSqlUnder130 = TSql80 | TSql90 | TSql100 | TSql110 | TSql120,
        TSqlUnder140 = TSql80 | TSql90 | TSql100 | TSql110 | TSql120 | TSql130,
        TSqlUnder150 = TSql80 | TSql90 | TSql100 | TSql110 | TSql120 | TSql130 | TSql140,
        TSqlUnder160 = TSql80 | TSql90 | TSql100 | TSql110 | TSql120 | TSql130 | TSql140 | TSql150,
        TSqlUnder170 = TSql80 | TSql90 | TSql100 | TSql110 | TSql120 | TSql130 | TSql140 | TSql150 | TSql160 | TSqlFabricDW,
    }
}