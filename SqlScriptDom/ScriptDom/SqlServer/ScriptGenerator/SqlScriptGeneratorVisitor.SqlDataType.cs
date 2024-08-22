//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SqlDataType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<SqlDataTypeOption, TokenGenerator> _sqlDataTypeOptionGenerators =
            new Dictionary<SqlDataTypeOption, TokenGenerator>()
        {
                { SqlDataTypeOption.BigInt, new IdentifierGenerator(CodeGenerationSupporter.BigInt) },
                { SqlDataTypeOption.Binary, new IdentifierGenerator(CodeGenerationSupporter.Binary) },
                { SqlDataTypeOption.Bit, new IdentifierGenerator(CodeGenerationSupporter.Bit) },
                { SqlDataTypeOption.Char, new IdentifierGenerator(CodeGenerationSupporter.Char) },
                { SqlDataTypeOption.Cursor, new KeywordGenerator(TSqlTokenType.Cursor) },
                { SqlDataTypeOption.DateTime, new IdentifierGenerator(CodeGenerationSupporter.DateTime) },
                { SqlDataTypeOption.Decimal, new IdentifierGenerator(CodeGenerationSupporter.Decimal) },
                { SqlDataTypeOption.Float, new IdentifierGenerator(CodeGenerationSupporter.Float) },
                { SqlDataTypeOption.Image, new IdentifierGenerator(CodeGenerationSupporter.Image) },
                { SqlDataTypeOption.Int, new IdentifierGenerator(CodeGenerationSupporter.Int) },
                { SqlDataTypeOption.Money, new IdentifierGenerator(CodeGenerationSupporter.Money) },
                { SqlDataTypeOption.NChar, new IdentifierGenerator(CodeGenerationSupporter.NChar) },
                { SqlDataTypeOption.NText, new IdentifierGenerator(CodeGenerationSupporter.NText) },
                { SqlDataTypeOption.NVarChar, new IdentifierGenerator(CodeGenerationSupporter.NVarChar) },
                { SqlDataTypeOption.Numeric, new IdentifierGenerator(CodeGenerationSupporter.Numeric) },
                { SqlDataTypeOption.Real, new IdentifierGenerator(CodeGenerationSupporter.Real) },
                { SqlDataTypeOption.SmallDateTime, new IdentifierGenerator(CodeGenerationSupporter.SmallDateTime) },
                { SqlDataTypeOption.SmallInt, new IdentifierGenerator(CodeGenerationSupporter.SmallInt) },
                { SqlDataTypeOption.SmallMoney, new IdentifierGenerator(CodeGenerationSupporter.SmallMoney) },
                { SqlDataTypeOption.Sql_Variant, new IdentifierGenerator(CodeGenerationSupporter.Sql_Variant) },
                { SqlDataTypeOption.Table, new KeywordGenerator(TSqlTokenType.Table) },
                { SqlDataTypeOption.Text, new IdentifierGenerator(CodeGenerationSupporter.Text) },
                { SqlDataTypeOption.Timestamp, new IdentifierGenerator(CodeGenerationSupporter.TimeStamp) },
                { SqlDataTypeOption.TinyInt, new IdentifierGenerator(CodeGenerationSupporter.TinyInt) },
                { SqlDataTypeOption.UniqueIdentifier, new IdentifierGenerator(CodeGenerationSupporter.UniqueIdentifier) },
                { SqlDataTypeOption.VarBinary, new IdentifierGenerator(CodeGenerationSupporter.VarBinary) },
                { SqlDataTypeOption.VarChar, new IdentifierGenerator(CodeGenerationSupporter.VarChar) },
                { SqlDataTypeOption.Date, new IdentifierGenerator(CodeGenerationSupporter.Date) },
                { SqlDataTypeOption.Time, new IdentifierGenerator(CodeGenerationSupporter.Time) },
                { SqlDataTypeOption.DateTime2, new IdentifierGenerator(CodeGenerationSupporter.DateTime2) },
                { SqlDataTypeOption.DateTimeOffset, new IdentifierGenerator(CodeGenerationSupporter.DateTimeOffset) },
                { SqlDataTypeOption.Rowversion, new IdentifierGenerator(CodeGenerationSupporter.Rowversion) },
                { SqlDataTypeOption.Json, new IdentifierGenerator(CodeGenerationSupporter.Json) },
                { SqlDataTypeOption.Vector, new IdentifierGenerator(CodeGenerationSupporter.Vector) },
        };

        public override void ExplicitVisit(SqlDataTypeReference node)
        {
            TokenGenerator generator = GetValueForEnumKey(_sqlDataTypeOptionGenerators, node.SqlDataTypeOption);
            if (generator != null)
            {
                GenerateToken(generator);
            }

            GenerateParameters(node);
        }
    }
}
