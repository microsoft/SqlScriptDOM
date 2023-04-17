using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The supported file types for copy command.
    /// </summary>
    public enum CopyCommandFileFormatType
    {
        /// <summary>
        /// ORC File created through hive.
        /// </summary>
        Orc,

        /// <summary>
        /// For files following CSV standards.
        /// </summary>
        Csv,

        /// <summary>
        /// Parquet files created through Hive or Impala.
        /// </summary>
        Parquet,
    }

    /// <summary>
    /// Compression type supported by copy command.
    /// </summary>
    public enum CopyCommandCompressionType
    {
        /// <summary>
        /// No compression.
        /// </summary>
        None,

        /// <summary>
        /// Gzip compression.
        /// </summary>
        Gzip,

        /// <summary>
        /// Default codec.
        /// </summary>
        DefaultCodec,

        /// <summary>
        /// Snappy codec.
        /// </summary>
        Snappy
    }

    /// <summary>
    /// Credential identity supported by COPY statement.
    /// </summary>
    public enum CopyCommandCredentialType
    {
        /// <summary>
        /// Shared Access Signature.
        /// </summary>
        Sas,

        /// <summary>
        /// Storage Account Key.
        /// </summary>
        AccountKey,

        /// <summary>
        /// OAUTH2 / Service Principal.
        /// </summary>
        ServicePrincipal,

        /// <summary>
        /// Managed Identity.
        /// </summary>
        ManagedIdentity,

        /// <summary>
        /// AAD Identity Paas Through.
        /// </summary>
        PassThrough,

        /// <summary>
        /// Public Account.
        /// </summary>
        None
    }

    /// <summary>
    /// The supported external file formats.
    /// </summary>
    public enum FileFormatType
    {
        /// <summary>
        /// A default value to represent the uninitialized state.
        /// </summary>
        Undefined,

        /// <summary>
        /// RC File created through hive.
        /// </summary>
        RcFile,

        /// <summary>
        /// ORC File created through hive.
        /// </summary>
        Orc,

        /// <summary>
        /// Delimited text files used by polybase.
        /// </summary>
        DelimitedText,

        /// <summary>
        /// Parquet files created through Hive or Impala.
        /// </summary>
        Parquet,

        /// <summary>
        /// CSV only used by copy statement.
        /// </summary>
        Csv,

        /// <summary>
        /// CSV only used by copy statement to enable multithreading.
        /// </summary>
        Parallel_Csv
    }

    /// <summary>
    /// Dateformat supported for copy command.
    /// </summary>
    public enum CopyCommandDateFormat
    {
        /// <summary>
        /// Month day year.
        /// </summary>
        mdy,

        /// <summary>
        /// Day month year.
        /// </summary>
        dmy,

        /// <summary>
        /// Year month day.
        /// </summary>
        ymd,

        /// <summary>
        /// Year day month.
        /// </summary>
        ydm,

        /// <summary>
        /// Month year day.
        /// </summary>
        myd,

        /// <summary>
        /// Day year month.
        /// </summary>
        dym
    }

    /// <summary>
    /// Encoding supported for copy command.
    /// </summary>
    public enum CopyCommandEncoding
    {
        /// <summary>
        /// UTF8 encoding.
        /// </summary>
        UTF8,

        /// <summary>
        /// UTF16 encoding.
        /// </summary>
        UTF16
    }

    /// <summary>
    /// Identity insert supported by copy command.
    /// </summary>
    public enum CopyCommandIdentityInsert
    {
        /// <summary>
        /// ON.
        /// </summary>
        ON,

        /// <summary>
        /// OFF.
        /// </summary>
        OFF
    }
}
