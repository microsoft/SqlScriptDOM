//------------------------------------------------------------------------------
// <copyright file="ParserTestOutput.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    /// <summary>
    /// Describes output for one test
    /// </summary>
    internal class ParserTestOutput
    {
        private readonly string _baselineFolder;
        private readonly int _numberOfErrors;
        readonly ParserErrorInfo[] _errorInfos;

        private ParserTestOutput(string baselineFolder, int numberOfErrors, ParserErrorInfo[] errorInfos)
        {
            _baselineFolder = baselineFolder;
            _numberOfErrors = numberOfErrors;
            _errorInfos = errorInfos;
        }

        public ParserTestOutput(string baselineFolder)
            : this(baselineFolder, 0, null)
        { }

        public ParserTestOutput(int numberOfErrors)
            : this(null, numberOfErrors, null)
        { }

        public ParserTestOutput(params ParserErrorInfo[] errorInfos)
            : this(null, errorInfos.Length, errorInfos)
        { }

        public void VerifyResult(string testScriptName, string prettyPrinted, IList<ParseError> errors)
        {
            // Errors case - verify number/exact error texts
            if (_numberOfErrors != errors.Count)
                ParserTestUtils.LogErrors(errors);

            Assert.AreEqual<int>(_numberOfErrors, errors.Count, testScriptName + ": number of errors after parsing is different from expected.");
            if (_errorInfos != null)
            {
                for (int i = 0; i < _errorInfos.Length; ++i)
                {
                    _errorInfos[i].VerifyError(errors[i]);
                }
            }

            // Check with baseline...
            if (_baselineFolder != null)
            {
                string baseline = ParserTestUtils.GetStringFromResource(GlobalConstants.TSqlNameSpace + "." + _baselineFolder+"." + testScriptName);
                baseline = baseline.Trim();
                prettyPrinted = prettyPrinted.Trim();

                string[] baselineLines = baseline.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.None);
                string[] prettyPrintedLines = prettyPrinted.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.None);

                Assert.AreEqual<int>
                (
                    baselineLines.Length,
                    prettyPrintedLines.Length,
                    string.Format
                    (
                        "Number of lines of baseline \"{0}\" and generated script does not match!. Expected: <{1}>. Actual: <{2}>.",
                        testScriptName, baseline, prettyPrinted
                    )
                );

                for(int lineCounter = 0; lineCounter <baselineLines.Length; lineCounter++)
                {
                    Assert.AreEqual<string>
                    (
                        baselineLines[lineCounter],
                        prettyPrintedLines[lineCounter], 
                        string.Format
                        (
                            "Different lines encountered. Pretty printed ASTs don't match the baseline \"{0}\". Different line number: {1}",
                            testScriptName,
                            lineCounter + 1
                        )
                    );
                }
            }
        }

        public void VerifyScriptRecreatedFromPositionInfo(string testScriptName, string prettyPrinted)
        {
            if (!string.IsNullOrEmpty(_baselineFolder))
            {
                string baseline = ParserTestUtils.GetStringFromResource(GlobalConstants.TSqlNameSpace + "." + _baselineFolder + "." + testScriptName);
                baseline = baseline.Trim();

                string[] baselineLines = baseline.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries);
                string[] prettyPrintedLines = prettyPrinted.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries);

                Assert.AreEqual<int>
                (
                    baselineLines.Length,
                    prettyPrintedLines.Length,
                    string.Format
                    (
                        "Number of lines of baseline \"{0}\" and generated script does not match!",
                        testScriptName
                    )
                );

                for (int lineCounter = 0; lineCounter < baselineLines.Length; lineCounter++)
                {
                    Assert.AreEqual<string>
                    (
                        baselineLines[lineCounter],
                        prettyPrintedLines[lineCounter],
                        string.Format
                        (
                            "Different lines encountered. Pretty printed ASTs don't match the baseline \"{0}\". Different Line number: {1}",
                            testScriptName,
                            lineCounter + 1
                        )
                    );
                }
           }
        }
    }
}
