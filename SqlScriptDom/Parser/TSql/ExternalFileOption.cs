//------------------------------------------------------------------------------
// <copyright file="ExternalFileOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Purpose:
// Helper for copy command options.
//
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// ExternalFileOption base class.
    /// </summary>
    public class ExternalFileOption
    {
        /// <summary>
        /// Checks whether sequence contains any unsupported xml character.
        /// </summary>
        /// <param name="sequence">Sequence of characters.</param>
        /// <param name="option">Name of the option.</param>
        public static void CheckXMLValidity(StringLiteral sequence, string option)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.CurrentCulture, "sequence"));
            }

            CheckXMLValidity(sequence.Value, option);
        }

        /// <summary>
        /// Checks whether sequence contains any unsupported xml character.
        /// </summary>
        /// <param name="sequence">Sequence of characters.</param>
        /// <param name="option">Name of the option.</param>
        public static void CheckXMLValidity(string sequence, string option)
        {
            if (sequence.Any(c => !XmlConvert.IsXmlChar(c)))
            {
                throw new Exception(string.Format(CultureInfo.CurrentCulture, "COPY statement failed because the value provided for option {0} has character(s) outside of XML 1.0 character range.", option));
            }
        }

        /// <summary>
        /// Check if a string is valid. We block using of characters
        /// unsupported by XML, and verify that if the string is in correct
        /// format if it is written in hexadecimal format.
        /// </summary>
        /// <param name="sequence">The string to be checked.</param>
        /// <param name="option">The type of the delimiter.</param>
        public static void CheckDelimiterValidity(string sequence, string option)
        {
            // Check if sequence has characters unsupported by XML
            ExternalFileOption.CheckXMLValidity(sequence, option);

            // Check if delimiter is in correct format if it is hexadecimal
            string pattern = @"^0[xX]";
            if (System.Text.RegularExpressions.Regex.IsMatch(sequence, pattern))
            {
                pattern = @"^0[xX]([0-9a-fA-F]{1,4})+$";

                if (!System.Text.RegularExpressions.Regex.IsMatch(sequence, pattern))
                {
                    throw new Exception(string.Format(CultureInfo.CurrentCulture, "COPY statement failed because the value provided for option '{0}' is not a valid hexadecimal. Please use the correct hexadecimal format: 0x[*]+, where each * is in the range 0-9, a-f, A-F, and the length of * is between 1 and 4.", option));
                }
            }

            // In case of null_values blocking null value delimiter i.e. ','
            if (option.Equals(CodeGenerationSupporter.NullValuesOption) && sequence.Contains("','"))
            {
                throw new Exception(string.Format(CultureInfo.CurrentCulture, "COPY statement failed because NULL_VALUES cannot contain STRING_DELIMITER, FIELD_TERMINATOR, ROW_DELIMITER (\\n, \\r, \\r\\n) and / or ','."));
            }
        }

    }
}
