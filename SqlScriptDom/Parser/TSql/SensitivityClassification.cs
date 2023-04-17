//------------------------------------------------------------------------------
// <copyright file="SensitivityClassification.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Utility class to expose sensitivity classification enums and methods
    /// </summary>
    public sealed class SensitivityClassification
    {
        /// <summary>
        /// Sensitivity classification option type used in AddSensitivityClassificationStatement
        /// </summary>
        public enum OptionType
        {
            Undefined         = -1,
            Label             =  0,
            LabelId           =  1,
            InformationType   =  2,
            InformationTypeId =  3,
            Rank              =  4
        }

        /// <summary>
        /// Sensitivity classification rank values
        /// </summary>
        public enum Rank
        {
            None     =  0,
            Low      = 10,
            Medium   = 20,
            High     = 30,
            Critical = 40
        }

        /// <summary>
        /// Returns sensitivity option type by option name
        /// </summary>
        public static OptionType GetOptionTypeByName(string option)
        {
            OptionType optionType;

            if (optionTypes.TryGetValue(option, out optionType))
            {
                return optionType;
            }

            return OptionType.Undefined;
        }

        private static readonly Dictionary<string, OptionType> optionTypes = new Dictionary<string, OptionType>(StringComparer.OrdinalIgnoreCase)
        {
            {CodeGenerationSupporter.Label, OptionType.Label},
            {CodeGenerationSupporter.LabelId, OptionType.LabelId},
            {CodeGenerationSupporter.InformationType, OptionType.InformationType},
            {CodeGenerationSupporter.InformationTypeId, OptionType.InformationTypeId},
            {CodeGenerationSupporter.Rank, OptionType.Rank}
        };
    }

#pragma warning restore 1591
}
