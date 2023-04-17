//------------------------------------------------------------------------------
// <copyright file="ExternalFileFormatOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The external file format optional property mapptings.
    /// </summary>
    internal class ExternalFileFormatOptionHelper : OptionsHelper<ExternalFileFormatOptionKind>
    {
        private ExternalFileFormatOptionHelper()
        {
            AddOptionMapping(ExternalFileFormatOptionKind.SerDeMethod, CodeGenerationSupporter.SerDeMethod);
            AddOptionMapping(ExternalFileFormatOptionKind.FormatOptions, CodeGenerationSupporter.FormatOptions);
            AddOptionMapping(ExternalFileFormatOptionKind.FieldTerminator, CodeGenerationSupporter.FieldTerminator2);
            AddOptionMapping(ExternalFileFormatOptionKind.StringDelimiter, CodeGenerationSupporter.StringDelimiter);
            AddOptionMapping(ExternalFileFormatOptionKind.DateFormat, CodeGenerationSupporter.DateFormat2);
            AddOptionMapping(ExternalFileFormatOptionKind.UseTypeDefault, CodeGenerationSupporter.UseTypeDefault);
            AddOptionMapping(ExternalFileFormatOptionKind.DataCompression, CodeGenerationSupporter.DataCompression);
            AddOptionMapping(ExternalFileFormatOptionKind.FirstRow, CodeGenerationSupporter.FirstRow2);
            AddOptionMapping(ExternalFileFormatOptionKind.Encoding, CodeGenerationSupporter.Encoding);
        }

        internal static readonly ExternalFileFormatOptionHelper Instance = new ExternalFileFormatOptionHelper();
    }
}
