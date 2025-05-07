//------------------------------------------------------------------------------
// <copyright file="ParserTest.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    /// <summary>
    /// One parser input file and expected outputs from different parsers
    /// </summary>
    internal class ParserTest
    {
        public readonly string _scriptFilename;
        public readonly ParserTestOutput _result80;
        public readonly ParserTestOutput _result90;
        public readonly ParserTestOutput _result100;
        public readonly ParserTestOutput _result110;
        public readonly ParserTestOutput _result120;
        public readonly ParserTestOutput _result130;
        public readonly ParserTestOutput _result140;
        public readonly ParserTestOutput _result150;
        public readonly ParserTestOutput _result160;
        public readonly ParserTestOutput _result170;
        public readonly ParserTestOutput _resultFabricDW;

        public ParserTest(
            string scriptFilename,
            ParserTestOutput result80, ParserTestOutput result90, ParserTestOutput result100,
            ParserTestOutput result110, ParserTestOutput result120, ParserTestOutput result130,
            ParserTestOutput result140, ParserTestOutput result150, ParserTestOutput result160, 
            ParserTestOutput result170, ParserTestOutput resultFabricDW)
        {
            _scriptFilename = scriptFilename;
            _result80 = result80;
            _result90 = result90;
            _result100 = result100;
            _result110 = result110;
            _result120 = result120;
            _result130 = result130;
            _result140 = result140;
            _result150 = result150;
            _result160 = result160;
            _result170 = result170;
            _resultFabricDW = resultFabricDW;
        }

        public ParserTest(
            string scriptFilename,
            ParserTestOutput result80, ParserTestOutput result90, ParserTestOutput result100,
            ParserTestOutput result110, ParserTestOutput result120, ParserTestOutput result130,
            ParserTestOutput result140, ParserTestOutput result150, ParserTestOutput result160)
            : this(
                scriptFilename,
                result80, result90, result100,
                result110, result120, result130,
                result140, result150, result160,
                result160, result160)
        {
        }

        public ParserTest(
            string scriptFilename,
            ParserTestOutput result80, ParserTestOutput result90, ParserTestOutput result100,
            ParserTestOutput result110, ParserTestOutput result120, ParserTestOutput result130,
            ParserTestOutput result140, ParserTestOutput result150)
            : this(
                scriptFilename,
                result80, result90, result100,
                result110, result120, result130,
                result140, result150, result150,
                result150, result150)
        {
        }

        public ParserTest(
           string scriptFilename,
           ParserTestOutput result80, ParserTestOutput result90, ParserTestOutput result100,
           ParserTestOutput result110, ParserTestOutput result120, ParserTestOutput result130,
           ParserTestOutput result140)
            : this(
                scriptFilename,
                result80, result90, result100,
                result110, result120, result130,
                result140, result140)
        {
        }

        public ParserTest(
            string scriptFilename,
            ParserTestOutput result80, ParserTestOutput result90, ParserTestOutput result100,
            ParserTestOutput result110, ParserTestOutput result120, ParserTestOutput result130)
            : this(
                scriptFilename,
                result80, result90, result100,
                result110, result120, result130, result130)
        {
        }

        public ParserTest(
            string scriptFilename,
            ParserTestOutput result80, ParserTestOutput result90, ParserTestOutput result100, 
            ParserTestOutput result110, ParserTestOutput result120)
            : this(
                scriptFilename,
                result80, result90, result100,
                result110, result120, result120)
        {
        }

        public ParserTest(
            string scriptFilename,
            ParserTestOutput result80, ParserTestOutput result90, ParserTestOutput result100,
            ParserTestOutput result110)
            : this(
                scriptFilename,
                result80, result90, result100,
                result110, result110)
        { }

        public ParserTest(
            string scriptFilename, 
            string baseline80, string baseline90, string baseline100, 
            string baseline110, string baseline120)
            : this(
                scriptFilename,
                new ParserTestOutput(baseline80), new ParserTestOutput(baseline90), new ParserTestOutput(baseline100), 
                new ParserTestOutput(baseline110), new ParserTestOutput(baseline120))
        { }

        public ParserTest(
            string scriptFilename,
            string baseline80, string baseline90, string baseline100,
            string baseline110, string baseline120, string baseline130)
            : this(
                scriptFilename,
                new ParserTestOutput(baseline80), new ParserTestOutput(baseline90), new ParserTestOutput(baseline100),
                new ParserTestOutput(baseline110), new ParserTestOutput(baseline120), new ParserTestOutput(baseline130))
        { }

        public ParserTest(
            string scriptFilename,
            string baseline80, string baseline90, string baseline100,
            string baseline110, string baseline120,
            string baseline130, string baseline140)
            : this(
                scriptFilename,
                new ParserTestOutput(baseline80), new ParserTestOutput(baseline90), new ParserTestOutput(baseline100),
                new ParserTestOutput(baseline110), new ParserTestOutput(baseline120), new ParserTestOutput(baseline130),
                new ParserTestOutput(baseline140))
        { }

        public ParserTest(
            string scriptFilename,
            string baseline80, string baseline90, string baseline100,
            string baseline110)
            : this(
                scriptFilename,
                new ParserTestOutput(baseline80), new ParserTestOutput(baseline90), new ParserTestOutput(baseline100),
                new ParserTestOutput(baseline110), new ParserTestOutput(baseline110))
        { }

        public ParserTest(
            string scriptFilename, 
            string baseline80, string baseline90, string baseline100)
            : this(
                scriptFilename,
                new ParserTestOutput(baseline80), new ParserTestOutput(baseline90), new ParserTestOutput(baseline100), 
                new ParserTestOutput(baseline100), new ParserTestOutput(baseline100))
        { }

        public static void ParseAndVerify(TSqlParser parser, SqlScriptGenerator scriptGen,
            string scriptFilename, ParserTestOutput expectedResult)
        {
            Trace.WriteLine("Script: " + scriptFilename);
            string script = ParserTestUtils.GetStringFromResource(GlobalConstants.TestScriptsNameSpace + "." + scriptFilename);
            IList<ParseError> errors;
            IList<ParseError> versioningErrors = null;
            TSqlFragment fragment = ParserTestUtils.ParseString(parser, script, out errors);

            string prettyPrinted = null;
            if (fragment != null)
            {
                scriptGen.GenerateScript(fragment, out prettyPrinted, out versioningErrors);

                foreach (ParseError e in versioningErrors)
                {
                    errors.Add(e);
                }
            }

            expectedResult.VerifyResult(scriptFilename, prettyPrinted, errors);

            if (errors.Count == 0)
            {
                TSqlFragment reparsed = ParserTestUtils.ParseString(parser, prettyPrinted, out errors);
                if (errors.Count != 0)
                {
                    ParserTestUtils.LogErrors(errors);
                    Assert.Fail("Errors found in reparsed script");
                }
                scriptGen.GenerateScript(reparsed, out prettyPrinted);
                expectedResult.VerifyResult(scriptFilename, prettyPrinted, errors);
                
                // Now, test statement positions...
                TSqlScript recreated = RecreateASTFromStatementPositions(script, scriptFilename, fragment, parser);
                scriptGen.GenerateScript(recreated, out prettyPrinted);
                expectedResult.VerifyScriptRecreatedFromPositionInfo(scriptFilename, prettyPrinted);
            }
        }

        private static TSqlScript RecreateASTFromStatementPositions(string source, string scriptFilename, TSqlFragment originalAST, TSqlParser parser)
        {
            TSqlScript originalScript = (TSqlScript)originalAST;
            TSqlScript newScript = new TSqlScript();
            foreach (TSqlBatch batch in originalScript.Batches)
            {
                TSqlBatch newBatch = new TSqlBatch();
                foreach (TSqlStatement statement in batch.Statements)
                {
                    string statementSource = source.Substring(statement.StartOffset, statement.FragmentLength);

                    IList<ParseError> errors;
                    TSqlScript reparsedScript = (TSqlScript)ParserTestUtils.ParseString(parser, statementSource, out errors);
                    ParserTestUtils.LogErrors(errors, statementSource);
                    Assert.AreEqual<int>(0, errors.Count, "Position testing: Parse errors in " + scriptFilename);

                    Assert.AreEqual<int>(1, reparsedScript.Batches.Count);
                    Assert.AreEqual<int>(1, reparsedScript.Batches[0].Statements.Count);

                    newBatch.Statements.Add(reparsedScript.Batches[0].Statements[0]);
                }
                newScript.Batches.Add(newBatch);
            }
            return newScript;
        }
    }

    internal class ParserTestCommon : ParserTest
    {
        public ParserTestCommon(string scriptFilename)
            : this(scriptFilename, new ParserTestOutput("BaselinesCommon"))
        { }

        public ParserTestCommon(string scriptFilename, ParserTestOutput testOutput)
            : base(scriptFilename, testOutput, testOutput, testOutput, testOutput)
        { }
    }

    internal class ParserTest80 : ParserTest
    {
        public ParserTest80(string scriptFilename, int nErrors90And100)
            : base(scriptFilename, new ParserTestOutput("Baselines80"), new ParserTestOutput(nErrors90And100), new ParserTestOutput(nErrors90And100), new ParserTestOutput(nErrors90And100))
        { }

        public ParserTest80(string scriptFilename, params ParserErrorInfo[] errors90And100)
            : base(scriptFilename, new ParserTestOutput("Baselines80"), new ParserTestOutput(errors90And100), new ParserTestOutput(errors90And100), new ParserTestOutput(errors90And100))
        { }
    }

    internal class ParserTest90 : ParserTest
    {
        public ParserTest90(string scriptFilename, ParserTestOutput output80, params ParserErrorInfo[] errors100)
            : base(scriptFilename, output80, new ParserTestOutput("Baselines90"), new ParserTestOutput(errors100), new ParserTestOutput(errors100))
        { }

        public ParserTest90(string scriptFilename, ParserTestOutput output80, ParserTestOutput output100, ParserTestOutput output110)
            : base(scriptFilename, output80, new ParserTestOutput("Baselines90"), output100, output110)
        { }
    }

    internal class ParserTest90And100 : ParserTest
    {
        public ParserTest90And100(string scriptFilename, int nErrors80)
            : base(scriptFilename, new ParserTestOutput(nErrors80), new ParserTestOutput("Baselines90"), new ParserTestOutput("Baselines90"), new ParserTestOutput("Baselines90"))
        { }

        public ParserTest90And100(string scriptFilename, params ParserErrorInfo[] errors80)
            : base(scriptFilename, new ParserTestOutput(errors80), new ParserTestOutput("Baselines90"), new ParserTestOutput("Baselines90"), new ParserTestOutput("Baselines90"))
        { }
    }

    internal class ParserTest100 : ParserTest
    {
        public ParserTest100(string scriptFilename, int nErrors80, int nErrors90)
            : base(scriptFilename, new ParserTestOutput(nErrors80), new ParserTestOutput(nErrors90), new ParserTestOutput("Baselines100"), new ParserTestOutput("Baselines100"))
        { }

        public ParserTest100(string scriptFilename, ParserTestOutput output80, ParserTestOutput output90)
            : base(scriptFilename, output80, output90, new ParserTestOutput("Baselines100"), new ParserTestOutput("Baselines100"))
        { }

        public ParserTest100(string scriptFilename, params ParserErrorInfo[] errors80And90)
            : base(scriptFilename, new ParserTestOutput(errors80And90), new ParserTestOutput(errors80And90), new ParserTestOutput("Baselines100"), new ParserTestOutput("Baselines100"))
        { }
    }

    internal class ParserTest110 : ParserTest
    {
        public ParserTest110(string scriptFilename, int nErrors80, int nErrors90, int nErrors100)
            : base(scriptFilename, new ParserTestOutput(nErrors80), new ParserTestOutput(nErrors90), new ParserTestOutput(nErrors100), new ParserTestOutput("Baselines110"))
        { }

        public ParserTest110(string scriptFilename, ParserTestOutput output80, ParserTestOutput output90, ParserTestOutput output100)
            : base(scriptFilename, output80, output90, output100, new ParserTestOutput("Baselines110"))
        { }

        public ParserTest110(string scriptFilename, params ParserErrorInfo[] errors80And90And100)
            : base(scriptFilename, new ParserTestOutput(errors80And90And100), new ParserTestOutput(errors80And90And100), new ParserTestOutput(errors80And90And100), new ParserTestOutput("Baselines110"))
        { }
    }

    internal class ParserTest120 : ParserTest
    {
        public ParserTest120(string scriptFilename, int nErrors80, int nErrors90, int nErrors100, int nErrors110)
            : base(
                scriptFilename, 
                new ParserTestOutput(nErrors80), new ParserTestOutput(nErrors90), new ParserTestOutput(nErrors100),
                new ParserTestOutput(nErrors110), new ParserTestOutput("Baselines120"))
        { }

        public ParserTest120(string scriptFilename, ParserTestOutput output80, ParserTestOutput output90, ParserTestOutput output100, ParserTestOutput output110)
            : base(
                scriptFilename, 
                output80, output90, output100,
                output110, new ParserTestOutput("Baselines120"))
        { }

        public ParserTest120(string scriptFilename, params ParserErrorInfo[] errors80And90And100And110)
            : base(
                scriptFilename,
                new ParserTestOutput(errors80And90And100And110), new ParserTestOutput(errors80And90And100And110), new ParserTestOutput(errors80And90And100And110), 
                new ParserTestOutput("Baselines120"))
        { }
    }

    internal class ParserTest130 : ParserTest
    {
        public ParserTest130(string scriptFilename, int nErrors80, int nErrors90, int nErrors100, int nErrors110, int nErrors120)
            : base(
                scriptFilename,
                new ParserTestOutput(nErrors80), new ParserTestOutput(nErrors90), new ParserTestOutput(nErrors100),
                new ParserTestOutput(nErrors110), new ParserTestOutput(nErrors120), new ParserTestOutput("Baselines130"))
        { }

        public ParserTest130(string scriptFilename, ParserTestOutput output80, ParserTestOutput output90, ParserTestOutput output100,
            ParserTestOutput output110, ParserTestOutput output120)
            : base(
                scriptFilename,
                output80, output90, output100,
                output110, output120, new ParserTestOutput("Baselines130"))
        { }

        public ParserTest130(string scriptFilename, params ParserErrorInfo[] errors80And90And100And110And120)
            : base(
                scriptFilename,
                new ParserTestOutput(errors80And90And100And110And120), 
                new ParserTestOutput(errors80And90And100And110And120),
                new ParserTestOutput(errors80And90And100And110And120),
                new ParserTestOutput(errors80And90And100And110And120),
                new ParserTestOutput("Baselines130"))
        { }
    }

    internal class ParserTest140 : ParserTest
    {
        public ParserTest140(string scriptFilename, int nErrors80, int nErrors90, int nErrors100, int nErrors110, int nErrors120, int nErrors130)
            : base(
                scriptFilename,
                new ParserTestOutput(nErrors80), new ParserTestOutput(nErrors90), new ParserTestOutput(nErrors100),
                new ParserTestOutput(nErrors110), new ParserTestOutput(nErrors120), new ParserTestOutput(nErrors130), new ParserTestOutput("Baselines140"))
        { }

        public ParserTest140(string scriptFilename, ParserTestOutput output80, ParserTestOutput output90, ParserTestOutput output100,
            ParserTestOutput output110, ParserTestOutput output120, ParserTestOutput output130)
            : base(
                scriptFilename,
                output80, output90, output100,
                output110, output120, 
                output130,
                new ParserTestOutput("Baselines140"))
        { }

        public ParserTest140(string scriptFilename, params ParserErrorInfo[] errors80And90And100And110And120and130)
            : base(
                scriptFilename,
                new ParserTestOutput(errors80And90And100And110And120and130),
                new ParserTestOutput(errors80And90And100And110And120and130),
                new ParserTestOutput(errors80And90And100And110And120and130),
                new ParserTestOutput(errors80And90And100And110And120and130),
                new ParserTestOutput(errors80And90And100And110And120and130),
                new ParserTestOutput("Baselines140"))
        { }
    }

    internal class ParserTest150 : ParserTest
    {
        public ParserTest150(string scriptFilename, int nErrors80, int nErrors90, int nErrors100, int nErrors110, int nErrors120, int nErrors130, int nErrors140)
            : base(
                scriptFilename,
                new ParserTestOutput(nErrors80), new ParserTestOutput(nErrors90), new ParserTestOutput(nErrors100),
                new ParserTestOutput(nErrors110), new ParserTestOutput(nErrors120), new ParserTestOutput(nErrors130),
                new ParserTestOutput(nErrors140), new ParserTestOutput("Baselines150"))
        { }

        public ParserTest150(string scriptFilename, ParserTestOutput output80, ParserTestOutput output90, ParserTestOutput output100,
            ParserTestOutput output110, ParserTestOutput output120, ParserTestOutput output130, ParserTestOutput output140)
            : base(
                scriptFilename,
                output80, output90, output100,
                output110, output120,
                output130,
                output140,
                new ParserTestOutput("Baselines150"))
        { }

        public ParserTest150(string scriptFilename, params ParserErrorInfo[] errors80And90And100And110And120and130and140)
            : base(
                scriptFilename,
                new ParserTestOutput(errors80And90And100And110And120and130and140),
                new ParserTestOutput(errors80And90And100And110And120and130and140),
                new ParserTestOutput(errors80And90And100And110And120and130and140),
                new ParserTestOutput(errors80And90And100And110And120and130and140),
                new ParserTestOutput(errors80And90And100And110And120and130and140),
                new ParserTestOutput("Baselines150"))
        { }
    }

    internal class ParserTest160 : ParserTest
    {
        public ParserTest160(string scriptFilename, int nErrors80, int nErrors90, int nErrors100, int nErrors110, int nErrors120, int nErrors130, int nErrors140, int nErrors150)
            : base(
                scriptFilename,
                new ParserTestOutput(nErrors80), new ParserTestOutput(nErrors90), new ParserTestOutput(nErrors100),
                new ParserTestOutput(nErrors110), new ParserTestOutput(nErrors120), new ParserTestOutput(nErrors130),
                new ParserTestOutput(nErrors140), new ParserTestOutput(nErrors150), new ParserTestOutput("Baselines160"))
        { }

        public ParserTest160(string scriptFilename, ParserTestOutput output80, ParserTestOutput output90, ParserTestOutput output100,
            ParserTestOutput output110, ParserTestOutput output120, ParserTestOutput output130, ParserTestOutput output140, ParserTestOutput output150)
            : base(
                scriptFilename,
                output80, output90, output100,
                output110, output120,
                output130,
                output140,
                output150,
                new ParserTestOutput("Baselines160"))
        { }

        public ParserTest160(string scriptFilename, params ParserErrorInfo[] errors80And90And100And110And120and130and140and150)
            : base(
                scriptFilename,
                new ParserTestOutput(errors80And90And100And110And120and130and140and150),
                new ParserTestOutput(errors80And90And100And110And120and130and140and150),
                new ParserTestOutput(errors80And90And100And110And120and130and140and150),
                new ParserTestOutput(errors80And90And100And110And120and130and140and150),
                new ParserTestOutput(errors80And90And100And110And120and130and140and150),
                new ParserTestOutput(errors80And90And100And110And120and130and140and150),
                new ParserTestOutput("Baselines160"))
        { }
    }

    internal class ParserTest170 : ParserTest
    {
        public ParserTest170(string scriptFilename, int nErrors80, int nErrors90, int nErrors100, int nErrors110, int nErrors120, int nErrors130, int nErrors140, int nErrors150, int nErrors160)
            : base(
                scriptFilename,
                new ParserTestOutput(nErrors80), new ParserTestOutput(nErrors90), new ParserTestOutput(nErrors100),
                new ParserTestOutput(nErrors110), new ParserTestOutput(nErrors120), new ParserTestOutput(nErrors130),
                new ParserTestOutput(nErrors140), new ParserTestOutput(nErrors150), new ParserTestOutput(nErrors160),
                new ParserTestOutput("Baselines170"), new ParserTestOutput(nErrors160))
        { }

        public ParserTest170(string scriptFilename, ParserTestOutput output80, ParserTestOutput output90, ParserTestOutput output100,
            ParserTestOutput output110, ParserTestOutput output120, ParserTestOutput output130, ParserTestOutput output140, ParserTestOutput output150, ParserTestOutput output160)
            : base(
                scriptFilename,
                output80, output90, output100,
                output110, output120,
                output130,
                output140,
                output150,
                output160,
                new ParserTestOutput("Baselines170"),
                output160)
        { }

        public ParserTest170(string scriptFilename, params ParserErrorInfo[] errors80And90And100And110And120and130and140and150and160)
            : base(
                scriptFilename,
                new ParserTestOutput(errors80And90And100And110And120and130and140and150and160),
                new ParserTestOutput(errors80And90And100And110And120and130and140and150and160),
                new ParserTestOutput(errors80And90And100And110And120and130and140and150and160),
                new ParserTestOutput(errors80And90And100And110And120and130and140and150and160),
                new ParserTestOutput(errors80And90And100And110And120and130and140and150and160),
                new ParserTestOutput(errors80And90And100And110And120and130and140and150and160),
                new ParserTestOutput("Baselines170"))
        { }
    }

    internal class ParserTestFabricDW : ParserTest
    {
        public ParserTestFabricDW(string scriptFilename, int nErrors80, int nErrors90, int nErrors100, int nErrors110, int nErrors120, int nErrors130, int nErrors140, int nErrors150, int nErrors160, int nErrors170)
            : base(
                scriptFilename,
                new ParserTestOutput(nErrors80), new ParserTestOutput(nErrors90), new ParserTestOutput(nErrors100),
                new ParserTestOutput(nErrors110), new ParserTestOutput(nErrors120), new ParserTestOutput(nErrors130),
                new ParserTestOutput(nErrors140), new ParserTestOutput(nErrors150), new ParserTestOutput(nErrors160),
                new ParserTestOutput(nErrors170), new ParserTestOutput("BaselinesFabricDW"))
        { }

        public ParserTestFabricDW(string scriptFilename, ParserTestOutput output80, ParserTestOutput output90, ParserTestOutput output100,
            ParserTestOutput output110, ParserTestOutput output120, ParserTestOutput output130, ParserTestOutput output140, ParserTestOutput output150, ParserTestOutput output160,
            ParserTestOutput output170)
            : base(
                scriptFilename,
                output80, output90, output100,
                output110, output120,
                output130,
                output140,
                output150,
                output160,
                output170,
                new ParserTestOutput("BaselinesFabricDW"))
        { }

        public ParserTestFabricDW(string scriptFilename, params ParserErrorInfo[] errors80And90And100And110And120and130and140and150and160and170)
            : base(
                scriptFilename,
                new ParserTestOutput(errors80And90And100And110And120and130and140and150and160and170),
                new ParserTestOutput(errors80And90And100And110And120and130and140and150and160and170),
                new ParserTestOutput(errors80And90And100And110And120and130and140and150and160and170),
                new ParserTestOutput(errors80And90And100And110And120and130and140and150and160and170),
                new ParserTestOutput(errors80And90And100And110And120and130and140and150and160and170),
                new ParserTestOutput(errors80And90And100And110And120and130and140and150and160and170),
                new ParserTestOutput(errors80And90And100And110And120and130and140and150and160and170),
                new ParserTestOutput(errors80And90And100And110And120and130and140and150and160and170),
                new ParserTestOutput(errors80And90And100And110And120and130and140and150and160and170),
                new ParserTestOutput(errors80And90And100And110And120and130and140and150and160and170),
                new ParserTestOutput("BaselinesFabricDW"))
        { }
    }

    internal class ParserTest80And90 : ParserTest
    {
        public ParserTest80And90(string scriptFilename, int nErrors100)
            : base(scriptFilename, new ParserTestOutput("Baselines90"), new ParserTestOutput("Baselines90"), new ParserTestOutput(nErrors100), new ParserTestOutput(nErrors100))
        { }

        public ParserTest80And90(string scriptFilename, params ParserErrorInfo[] errors100)
            : base(scriptFilename, new ParserTestOutput("Baselines90"), new ParserTestOutput("Baselines90"), new ParserTestOutput(errors100), new ParserTestOutput(errors100))
        { }
    }

    internal class ParserTest80And90And100 : ParserTest
    {
        public ParserTest80And90And100(string scriptFilename, int nErrors110)
            : base(scriptFilename, new ParserTestOutput("Baselines80"), new ParserTestOutput("Baselines80"), new ParserTestOutput("Baselines80"), new ParserTestOutput(nErrors110))
        { }

        public ParserTest80And90And100(string scriptFilename, params ParserErrorInfo[] errors110)
            : base(scriptFilename, new ParserTestOutput("Baselines80"), new ParserTestOutput("Baselines80"), new ParserTestOutput("Baselines80"), new ParserTestOutput(errors110))
        { }
    }

}
