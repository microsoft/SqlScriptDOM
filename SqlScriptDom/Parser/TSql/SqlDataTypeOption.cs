//------------------------------------------------------------------------------
// <copyright file="SqlDataTypeOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// These are the possible data types in Sql.
    /// The capitalization of names are influenced by System.Data.SqlDbType.
    /// </summary>
    
    [Serializable]
    public enum SqlDataTypeOption
    {
        /// <summary>
        /// Nothing was defined.
        /// </summary>
        None = 0,

        #region Integers

        /// <summary>
        /// Integer (whole number) data from -2^63 (-9,223,372,036,854,775,808) 
        /// through 2^63-1 (9,223,372,036,854,775,807).
        /// </summary>
        BigInt = 1,

        /// <summary>
        /// Integer (whole number) data from -2^31 (-2,147,483,648) 
        /// through 2^31 - 1 (2,147,483,647).
        /// </summary>
        Int = 2,

        /// <summary>
        /// Integer data from -2^15 (-32,768) through 2^15 - 1 (32,767).
        /// </summary>
        SmallInt = 3,

        /// <summary>
        /// Integer data from 0 through 255.
        /// </summary>
        TinyInt = 4,

        #endregion

        #region Bit

        /// <summary>
        /// Integer data with either a 1 or 0 value.
        /// </summary>
        Bit = 5,

        #endregion 

        #region Decimal and Numeric

        /// <summary>
        /// Fixed precision and scale numeric data from -10^38 +1 through 10^38 �1. 
        /// </summary>
        Decimal = 6,

        /// <summary>
        /// Functionally equivalent to decimal.
        /// </summary>
        Numeric = 7,

        #endregion

        #region Money and SmallMoney

        /// <summary>
        /// Monetary data values from -2^63 (-922,337,203,685,477.5808) 
        /// through 2^63 - 1 (+922,337,203,685,477.5807), with accuracy 
        /// to a ten-thousandth of a monetary unit.
        /// </summary>
        Money = 8,

        /// <summary>
        /// Monetary data values from -214,748.3648 through +214,748.3647, 
        /// with accuracy to a ten-thousandth of a monetary unit.
        /// </summary>
        SmallMoney = 9,

        #endregion

        #region Approximate Numerics
        
        /// <summary>
        /// Floating precision number data with the following valid values: 
        /// -1.79E + 308 through -2.23E - 308, 0 and 2.23E + 308 through 1.79E + 308.
        /// </summary>
        Float = 10,

        /// <summary>
        /// Floating precision number data with the following valid values: 
        /// -3.40E + 38 through -1.18E - 38, 0 and 1.18E - 38 through 3.40E + 38.
        /// </summary>
        Real = 11,

        #endregion

        #region DateTime and SmallDateTime

        /// <summary>
        /// Date and time data from January 1, 1753, through December 31, 9999, 
        /// with an accuracy of three-hundredths of a second, or 3.33 milliseconds.
        /// </summary>
        DateTime = 12,

        /// <summary>
        /// Date and time data from January 1, 1900, through June 6, 2079, 
        /// with an accuracy of one minute.
        /// </summary>
        SmallDateTime = 13,

        #endregion

        #region Character Strings
        
        /// <summary>
        /// Fixed-length non-Unicode character data with a maximum 
        /// length of 8,000 characters.
        /// </summary>
        Char = 14,

        /// <summary>
        /// Variable-length non-Unicode data with a maximum of 8,000 characters.
        /// </summary>
        VarChar = 15,

        /// <summary>
        /// Variable-length non-Unicode data with a maximum length of 
        /// 2^31 - 1 (2,147,483,647) characters.
        /// </summary>
        Text = 16,

        #endregion

        #region Unicode Character Strings

        /// <summary>
        /// Fixed-length Unicode data with a maximum length of 4,000 characters. 
        /// </summary>
        NChar = 17,

        /// <summary>
        /// Variable-length Unicode data with a maximum length of 4,000 characters. 
        /// sysname is a system-supplied user-defined data type that is functionally 
        /// equivalent to nvarchar(128) and is used to reference database object names.
        /// </summary>
        NVarChar = 18,

        /// <summary>
        /// Variable-length Unicode data with a maximum length of 2^30 - 1 (1,073,741,823) characters.
        /// </summary>
        NText = 19,

        #endregion

        #region Binary Strings
        
        /// <summary>
        /// Fixed-length binary data with a maximum length of 8,000 bytes.
        /// </summary>
        Binary = 20,

        /// <summary>
        /// Variable-length binary data with a maximum length of 8,000 bytes.
        /// </summary>
        VarBinary = 21,

        /// <summary>
        /// Variable-length binary data with a maximum length of 
        /// 2^31 - 1 (2,147,483,647) bytes.
        /// </summary>
        Image = 22,

        #endregion

        #region Other Data Types

        /// <summary>
        /// A reference to a cursor.
        /// </summary>
        Cursor = 23,
        
        /// <summary>
        /// A data type that stores values of various SQL Server-supported data types, 
        /// except text, ntext, timestamp, and sql_variant.
        /// </summary>
        Sql_Variant = 24,

        /// <summary>
        /// A special data type used to store a result set for later processing.
        /// </summary>
        Table = 25,

        /// <summary>
        /// A database-wide unique number that gets updated every time a row gets updated.
        /// </summary>
        Timestamp = 26,

        /// <summary>
        /// Is a 16-byte GUID
        /// </summary>
        UniqueIdentifier = 27,

        // SqlType enum has Xml value, so, putting it here to explain gap in values
        // Script DOM and parser use XmlDataType AST instead of SqlDataType.SqlDataTypeOption
        // Xml = 28,

        #endregion

        #region Katmai data types
        /// <summary>
        /// Date-only
        /// </summary>
        Date = 29,

        /// <summary>
        /// Time only
        /// </summary>
        Time = 30,

        /// <summary>
        /// Combination of data and time types, better precision than DataTime
        /// </summary>
        DateTime2 = 31,

        /// <summary>
        /// Same as DateTime2 with timezone offset added
        /// </summary>
        DateTimeOffset = 32,

        /// <summary>
        /// A database-wide unique number that gets updated every time a row gets updated.
        /// A synonym for Timestamp with different semantics requiring a named column of this type.
        /// </summary>
        Rowversion = 33,

        /// <summary>
        /// JSON data type
        /// </summary>
        Json = 34,

        #endregion

    }
}
