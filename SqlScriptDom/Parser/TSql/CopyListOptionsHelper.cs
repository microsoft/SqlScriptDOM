//------------------------------------------------------------------------------
// <copyright file="CopyListOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Purpose:
// Parse helper utility for copy command list type options.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
        /// <summary>
        /// Class to validate and assign values to copy options present in the form of list.
        /// </summary>
        internal class CopyListOptionsHelper : OptionsHelper<CopyOptionKind>
        {
            public static readonly CopyListOptionsHelper Instance = new CopyListOptionsHelper();

            /// <summary>
            /// Assigns the values to CopyColumnOption object after basic validation.
            /// </summary>
            /// <param name="columnOption">Column option.</param>
            /// <param name="columnName">Name of the column</param>
            /// <param name="defaultColumnValue">Default column value if any</param>
            /// <param name="defaultSpecified">True if default vlaue is specified</param>
            /// <param name="fieldNumber">Corresponding column number in external file</param>
            /// <param name="columnCount">Count of this column in the list, specifically passed for better error message</param>
            internal void AssignCopyColumnOptions(CopyColumnOption columnOption, Identifier columnName, ScalarExpression defaultColumnValue, bool defaultSpecified, IntegerLiteral fieldNumber, int columnCount)
            {
                if (columnOption == null)
                {
                    throw new ArgumentNullException(string.Format(CultureInfo.CurrentCulture,"Missing columnOption"));
                }
                else if (columnName == null)
                {
                    throw new ArgumentNullException(string.Format(CultureInfo.CurrentCulture,"Missing columnName"));
                }
                else if (columnCount <= 0)
                {
                    throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, "Column count should be greater than/equal to 1."));
                }

                string errorMessageForInvalidColumnOption = string.Format(CultureInfo.CurrentCulture, "{{0}} of column {0} in the list", columnCount);
                columnOption.ColumnName = columnName as Identifier;
                if (columnOption.ColumnName == null || columnOption.ColumnName.Value == null || columnOption.ColumnName.Value.Length <= 0)
                {
                    throw new Exception(string.Format(CultureInfo.CurrentCulture, errorMessageForInvalidColumnOption, "Invalid column name"));
                }

                columnOption.DefaultValue = defaultSpecified ? defaultColumnValue : null;
                if (defaultSpecified)
                {
                    if (columnOption.DefaultValue == null)
                    {
                        throw new Exception(string.Format(CultureInfo.CurrentCulture, errorMessageForInvalidColumnOption, "Null default value"));
                    }
                }

                columnOption.FieldNumber = fieldNumber;
                int result = 0;
                if (columnOption.FieldNumber != null && (Int32.TryParse(columnOption.FieldNumber.Value, out result) == false || result <= 0))
                {
                    throw new Exception(string.Format(CultureInfo.CurrentCulture, errorMessageForInvalidColumnOption, "Invalid field number"));
                }
            }

    }
}

