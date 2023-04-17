//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.IgnoreDupKeyIndexOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(IgnoreDupKeyIndexOption node)
        {
            IndexOptionHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            GenerateOptionStateOnOff(node.OptionState);
            
            if (_options.SqlVersion >= SqlVersion.Sql140 &&
                OptionState.On == node.OptionState &&
                null != node.SuppressMessagesOption)
            {
                GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
                GenerateIdentifier(CodeGenerationSupporter.SuppressMessages);
                GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                GenerateSpace();
                if (true == node.SuppressMessagesOption)
                {
                   GenerateOptionStateOnOff(OptionState.On);
                }
                else
                {
                   GenerateOptionStateOnOff(OptionState.Off);
                }
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
        }
    }
}
