using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    using System;
    using System.IO;

    public partial class SqlDomTests
	{
        /// <summary>
        /// Test for RESTORE ... FROM URL ... syntax
        /// Also tests the related BACKUP ... TO URL ... syntax
        /// See https://github.com/microsoft/SqlScriptDOM/issues/29 for details on the issue.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void BackupRestoreUrl()
        {
            ParserTestUtils.ExecuteTestForAllParsers(parser =>
            {
                string script = @"BACKUP DATABASE AdventureWorks2016
TO URL = 'https://mystorageaccount.blob.core.windows.net/mycontainername/AdventureWorks2016.bak'
WITH COMPRESSION
,STATS = 5;

RESTORE DATABASE Sales
FROM URL = 'https://mystorageaccount.blob.core.windows.net/mysecondcontainer/Sales.bak'
WITH MOVE 'Sales_Data1' to 'https://mystorageaccount.blob.core.windows.net/myfirstcontainer/Sales_Data1.mdf',
MOVE 'Sales_log' to 'https://mystorageaccount.blob.core.windows.net/myfirstcontainer/Sales_log.ldf',
STATS = 10;

RESTORE DATABASE AdventureWorks2012
    FROM DISK = 'Z:\SQLServerBackups\AdventureWorks2012.bak'
    WITH FILE = 6,
      NORECOVERY;
";
                using (var scriptReader = new StringReader(script))
                {
                    var fragment = parser.Parse(scriptReader, out IList<ParseError> errors) as TSqlScript;
                    if (!(parser is TSql80Parser || parser is TSql90Parser || parser is TSql100Parser))
                    {
                        // for these parser versions we expect success
                        Assert.AreEqual(0, errors.Count);
                        Assert.IsTrue(fragment is TSqlScript);
                        Assert.IsTrue(fragment.Batches[0].Statements[0] is BackupStatement);
                        Assert.AreEqual(DeviceType.Url, (fragment.Batches[0].Statements[0] as BackupStatement).Devices[0].DeviceType);
                        Assert.AreEqual("https://mystorageaccount.blob.core.windows.net/mycontainername/AdventureWorks2016.bak", ((fragment.Batches[0].Statements[0] as BackupStatement).Devices[0].PhysicalDevice as StringLiteral).Value);
                        Assert.IsTrue(fragment.Batches[0].Statements[1] is RestoreStatement);
                        Assert.AreEqual(DeviceType.Url, (fragment.Batches[0].Statements[1] as RestoreStatement).Devices[0].DeviceType);
                        Assert.AreEqual("https://mystorageaccount.blob.core.windows.net/mysecondcontainer/Sales.bak", ((fragment.Batches[0].Statements[1] as RestoreStatement).Devices[0].PhysicalDevice as StringLiteral).Value);

                        // regression test to ensure that DISK device type is correctly handled
                        Assert.IsTrue(fragment.Batches[0].Statements[2] is RestoreStatement);
                        Assert.AreEqual(DeviceType.Disk, (fragment.Batches[0].Statements[2] as RestoreStatement).Devices[0].DeviceType);
                        Assert.AreEqual("Z:\\SQLServerBackups\\AdventureWorks2012.bak", ((fragment.Batches[0].Statements[2] as RestoreStatement).Devices[0].PhysicalDevice as StringLiteral).Value);
                        Assert.AreEqual(RestoreOptionKind.File, ((fragment.Batches[0].Statements[2] as RestoreStatement).Options[0] as ScalarExpressionRestoreOption).OptionKind);
                        Assert.AreEqual("6", (((fragment.Batches[0].Statements[2] as RestoreStatement).Options[0] as ScalarExpressionRestoreOption).Value as IntegerLiteral).Value);
                        Assert.AreEqual("Z:\\SQLServerBackups\\AdventureWorks2012.bak", ((fragment.Batches[0].Statements[2] as RestoreStatement).Devices[0].PhysicalDevice as StringLiteral).Value);

                        if (parser is TSql110Parser)
                        {
                            SqlScriptGenerator scriptGen = ParserTestUtils.CreateScriptGen(SqlVersion.Sql110);
                            scriptGen.GenerateScript(fragment, out string prettyPrinted);
                            Assert.AreEqual(@"BACKUP DATABASE AdventureWorks2016
    TO URL = 'https://mystorageaccount.blob.core.windows.net/mycontainername/AdventureWorks2016.bak'
    WITH COMPRESSION, STATS = 5;

RESTORE DATABASE Sales FROM URL = 'https://mystorageaccount.blob.core.windows.net/mysecondcontainer/Sales.bak'
    WITH MOVE 'Sales_Data1' TO 'https://mystorageaccount.blob.core.windows.net/myfirstcontainer/Sales_Data1.mdf', MOVE 'Sales_log' TO 'https://mystorageaccount.blob.core.windows.net/myfirstcontainer/Sales_log.ldf', STATS = 10;

RESTORE DATABASE AdventureWorks2012 FROM DISK = 'Z:\SQLServerBackups\AdventureWorks2012.bak'
    WITH FILE = 6, NORECOVERY;"
, prettyPrinted.Trim());
                        }
                    }
                    else
                    {
                        // for the older parsers this syntax is expected to return errors
                        Assert.AreEqual(2, errors.Count);
                        Assert.AreEqual("Incorrect syntax near URL.", errors[0].Message);
                        Assert.AreEqual("Incorrect syntax near URL.", errors[1].Message);
                    }
                }
            }, true);
        }

        /// <summary>
        /// Test for RESTORE ... FROM DATABASE_SNAPSHOT ... syntax. This is supported in SQL Server 2005+, so parser version 90+.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void RestoreFromDBSnapshot()
        {
            ParserTestUtils.ExecuteTestForAllParsers(parser =>
            {
                string script = @"RESTORE DATABASE AdventureWorks2012 FROM DATABASE_SNAPSHOT = 'AdventureWorks_dbss1800'";
                using (var scriptReader = new StringReader(script))
                {
                    var fragment = parser.Parse(scriptReader, out IList<ParseError> errors) as TSqlScript;
                    if (parser is TSql80Parser)
                    {
                        // for the older parsers this syntax is expected to return errors
                        Assert.AreEqual(1, errors.Count);
                        Assert.AreEqual("Incorrect syntax near DATABASE_SNAPSHOT.", errors[0].Message);
                    }
                    else
                    {
                        // for these parser versions we expect success
                        Assert.AreEqual(0, errors.Count);
                        Assert.IsTrue(fragment is TSqlScript);
                        Assert.IsTrue(fragment.Batches[0].Statements[0] is RestoreStatement);
                    }
                }
            }, true);
        }

        /// <summary>
        /// Test for BACKUP / RESTORE ... VIRTUAL_DEVICE ... syntax. This is supported in SQL Server 2005+, so parser version 90+.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void BackupRestoreVirtualDevice()
        {
            ParserTestUtils.ExecuteTestForAllParsers(parser =>
            {
                string script = @"BACKUP DATABASE MyDB TO VIRTUAL_DEVICE='{A9FD7855-A978-4931-8027-F0EFB1D4F4EA}' WITH STATS = 10;
RESTORE DATABASE MyDB FROM VIRTUAL_DEVICE='{A9FD7855-A978-4931-8027-F0EFB1D4F4EA}' WITH NORECOVERY, CHECKSUM, REPLACE, BUFFERCOUNT=16, MAXTRANSFERSIZE=2097152";
                using (var scriptReader = new StringReader(script))
                {
                    var fragment = parser.Parse(scriptReader, out IList<ParseError> errors) as TSqlScript;
                    if (parser is TSql80Parser)
                    {
                        // for the older parsers this syntax is expected to return errors
                        Assert.AreEqual(2, errors.Count);
                        Assert.AreEqual("Incorrect syntax near VIRTUAL_DEVICE.", errors[0].Message);
                        Assert.AreEqual("Incorrect syntax near VIRTUAL_DEVICE.", errors[1].Message);
                    }
                    else
                    {
                        // for these parser versions we expect success
                        Assert.AreEqual(0, errors.Count);
                        Assert.IsTrue(fragment is TSqlScript);
                        Assert.IsTrue(fragment.Batches[0].Statements[0] is BackupStatement);
                        Assert.IsTrue(fragment.Batches[0].Statements[1] is RestoreStatement);
                    }
                }
            }, true);
        }
	}
}
