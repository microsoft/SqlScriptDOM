//------------------------------------------------------------------------------
// <copyright file="ParserErrorsTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    public partial class SqlDomTests
    {
        #region Information about index options
        class IndexOptionInfo
        {
            public IndexOptionInfo(string name, string value)
            {
                _name = name;
                _value = value;
            }

            readonly string _name;
            readonly string _value;

            public string Name
            {
                get { return _name; }
            }

            public string GetFullOptionText()
            {
                return _name + " = " + _value;
            }
        }

        readonly static Dictionary<IndexOptionKind, IndexOptionInfo> _indexOptionInfos90 = new Dictionary<IndexOptionKind, IndexOptionInfo>();
        readonly static Dictionary<IndexOptionKind, IndexOptionInfo> _indexOptionInfos100 = new Dictionary<IndexOptionKind, IndexOptionInfo>();
        readonly static Dictionary<IndexOptionKind, IndexOptionInfo> _indexOptionInfos130 = new Dictionary<IndexOptionKind, IndexOptionInfo>();
        readonly static Dictionary<IndexOptionKind, IndexOptionInfo> _indexOptionInfos150 = new Dictionary<IndexOptionKind, IndexOptionInfo>();

        static SqlDomTests()
        {
            // Initialize all possible 90 index options
            _indexOptionInfos90.Add(IndexOptionKind.AllowPageLocks, new IndexOptionInfo("ALLOW_PAGE_LOCKS", "ON"));
            _indexOptionInfos90.Add(IndexOptionKind.AllowRowLocks, new IndexOptionInfo("ALLOW_ROW_LOCKS", "OFF"));
            _indexOptionInfos90.Add(IndexOptionKind.DropExisting, new IndexOptionInfo("DROP_EXISTING", "ON"));
            _indexOptionInfos90.Add(IndexOptionKind.FillFactor, new IndexOptionInfo("FILLFACTOR", "90"));
            _indexOptionInfos90.Add(IndexOptionKind.IgnoreDupKey, new IndexOptionInfo("IGNORE_DUP_KEY", "ON"));
            _indexOptionInfos90.Add(IndexOptionKind.MaxDop, new IndexOptionInfo("MAXDOP", "2"));
            _indexOptionInfos90.Add(IndexOptionKind.Online, new IndexOptionInfo("ONLINE", "OFF"));
            _indexOptionInfos90.Add(IndexOptionKind.PadIndex, new IndexOptionInfo("PAD_INDEX", "ON"));
            _indexOptionInfos90.Add(IndexOptionKind.SortInTempDB, new IndexOptionInfo("SORT_IN_TEMPDB", "OFF"));
            _indexOptionInfos90.Add(IndexOptionKind.StatisticsNoRecompute, new IndexOptionInfo("STATISTICS_NORECOMPUTE", "ON"));
            _indexOptionInfos90.Add(IndexOptionKind.LobCompaction, new IndexOptionInfo("LOB_COMPACTION", "ON"));

            // Copy them to 100 list
            foreach (KeyValuePair<IndexOptionKind, IndexOptionInfo> keyVal in _indexOptionInfos90)
                _indexOptionInfos100.Add(keyVal.Key, keyVal.Value);

            // ... and add few Katmai-only ones
            _indexOptionInfos100.Add(IndexOptionKind.DataCompression, new IndexOptionInfo("DATA_COMPRESSION", "ROW"));
            _indexOptionInfos100.Add(IndexOptionKind.FileStreamOn, new IndexOptionInfo("FILESTREAM_ON", "OFF"));

            // Copy them to 130 list
            foreach (KeyValuePair<IndexOptionKind, IndexOptionInfo> keyVal in _indexOptionInfos100)
                _indexOptionInfos130.Add(keyVal.Key, keyVal.Value);

            // -- add options for TSql130
            _indexOptionInfos130.Add(IndexOptionKind.CompressAllRowGroups, new IndexOptionInfo("COMPRESS_ALL_ROW_GROUPS", "ON"));
            _indexOptionInfos130.Add(IndexOptionKind.CompressionDelay, new IndexOptionInfo("COMPRESSION_DELAY", "27"));

            // Copy them to 150 list
            foreach (KeyValuePair<IndexOptionKind, IndexOptionInfo> keyVal in _indexOptionInfos130)
                _indexOptionInfos150.Add(keyVal.Key, keyVal.Value);

            // -- add options for TSql150
            _indexOptionInfos150.Add(IndexOptionKind.OptimizeForSequentialKey, new IndexOptionInfo("OPTIMIZE_FOR_SEQUENTIAL_KEY", "ON"));
        }

        #endregion

        static void IndexOptionErrorTest(TSqlParser parser, Dictionary<IndexOptionKind, IndexOptionInfo> optionInfos,
            string template, string statementName, params IndexOptionKind[] allowedOptions)
        {
            // Find a spot where option script would be inserted...
            int errorPosition = template.IndexOf('{');

            foreach (KeyValuePair<IndexOptionKind, IndexOptionInfo> tuple in optionInfos)
            {
                string fullScript = string.Format(CultureInfo.InvariantCulture, template, tuple.Value.GetFullOptionText());

                if (Array.IndexOf<IndexOptionKind>(allowedOptions, tuple.Key) != -1)
                {
                    ParserTestUtils.ErrorTest(parser, fullScript);
                }
                else
                {
                    ParserTestUtils.ErrorTest(parser, fullScript,
                        new ParserErrorInfo(errorPosition, "SQL46057", tuple.Value.Name, statementName));
                }
            }
        }

        #region Statement templates

        const string CreateTypeColumnConstraint = "CREATE TYPE t1 AS TABLE (c1 INT PRIMARY KEY WITH ( {0} ))";
        const string CreateTypeTableConstraint = "CREATE TYPE t1 AS TABLE (c1 INT, UNIQUE(c1) WITH ( {0} ))";
        const string AlterTableAddElement = "ALTER TABLE t1 ADD c1 int PRIMARY KEY WITH ({0})";
        const string AlterTableRebuildOne = "ALTER TABLE t1 REBUILD PARTITION = 1 WITH ({0})";
        const string AlterTableRebuildAll = "ALTER TABLE t1 REBUILD PARTITION = ALL WITH ({0})";
        const string AlterIndexSet = "ALTER INDEX i1 ON t1 SET ({0})";
        const string AlterIndexRebuildOne = "ALTER INDEX i1 ON t1 REBUILD PARTITION = 1 WITH ({0})";
        const string AlterIndexRebuildAll = "ALTER INDEX i1 ON t1 REBUILD WITH ({0})";
        const string AlterIndexReorganize = "ALTER INDEX i1 ON t1 REORGANIZE WITH ({0})";
        const string CreateIndex = "CREATE INDEX i1 ON t1(c1) WITH ({0})";
        const string CreateTableColumnConstraint = "CREATE TABLE t1(c1 INT PRIMARY KEY WITH ({0}))";
        const string CreateTableTableConstraint = "CREATE TABLE t1(c1 INT, UNIQUE (c1) WITH ({0}))";
        const string DeclareTableVarColumnConstraint = "DECLARE @v1 AS TABLE (c1 INT UNIQUE WITH ({0}))";
        const string DeclareTableVarTableConstraint = "DECLARE @v1 AS TABLE (c1 INT, PRIMARY KEY (c1) WITH ({0}))";
        const string CreateXmlIndex = "CREATE PRIMARY XML INDEX i1 ON t1(c1) WITH ({0})";
        const string CreateFunctionColumnConstraint = "CREATE FUNCTION f1() RETURNS @v1 TABLE (c1 INT PRIMARY KEY WITH ({0})) BEGIN RETURN END";
        const string AlterFunctionTableConstraint = "ALTER FUNCTION f1() RETURNS @v1 TABLE (c1 INT, UNIQUE(c1) WITH ({0})) BEGIN RETURN END";
        const string CreateSpatialIndex = "CREATE SPATIAL INDEX s1 ON t1(c1) WITH ({0})";
        const string CreateColumnStoreIndex = "CREATE COLUMNSTORE INDEX s1 ON t1(c1) WITH ({0})";

        #endregion

        #region Option sets used multiple times

        static IndexOptionKind[] _alterTableAddIndexOptions = new IndexOptionKind[] 
            {
                IndexOptionKind.AllowPageLocks, IndexOptionKind.AllowRowLocks, 
                IndexOptionKind.DataCompression, IndexOptionKind.IgnoreDupKey, 
                IndexOptionKind.FillFactor, IndexOptionKind.FileStreamOn, 
                IndexOptionKind.MaxDop, IndexOptionKind.Online,
                IndexOptionKind.PadIndex, IndexOptionKind.SortInTempDB, 
                IndexOptionKind.StatisticsNoRecompute
            };

        static IndexOptionKind[] _onePartitionRebuildOptions = new IndexOptionKind[]
            {
                IndexOptionKind.DataCompression, IndexOptionKind.SortInTempDB, IndexOptionKind.MaxDop
            };

        static IndexOptionKind[] _allPartitionsRebuildOptions = new IndexOptionKind[]
            {
                IndexOptionKind.AllowPageLocks, IndexOptionKind.AllowRowLocks, 
                IndexOptionKind.DataCompression, IndexOptionKind.IgnoreDupKey, 
                IndexOptionKind.FillFactor,  IndexOptionKind.MaxDop, 
                IndexOptionKind.Online, IndexOptionKind.PadIndex, 
                IndexOptionKind.SortInTempDB, IndexOptionKind.StatisticsNoRecompute
            };

        static IndexOptionKind[] _createIndexOptions = new IndexOptionKind[]
            {
                IndexOptionKind.AllowPageLocks, IndexOptionKind.AllowRowLocks, 
                IndexOptionKind.DataCompression, IndexOptionKind.IgnoreDupKey, 
                IndexOptionKind.FillFactor,  IndexOptionKind.DropExisting, 
                IndexOptionKind.MaxDop, IndexOptionKind.Online,
                IndexOptionKind.PadIndex, IndexOptionKind.SortInTempDB, 
                IndexOptionKind.StatisticsNoRecompute
            };

        static IndexOptionKind[] _createTableIndexOptions = new IndexOptionKind[]
            {
                IndexOptionKind.AllowPageLocks, IndexOptionKind.AllowRowLocks, 
                IndexOptionKind.DataCompression, IndexOptionKind.IgnoreDupKey, 
                IndexOptionKind.FillFactor,  IndexOptionKind.PadIndex, 
                IndexOptionKind.StatisticsNoRecompute
            };

        static IndexOptionKind[] _createXmlIndexOptions = new IndexOptionKind[]
            {
                IndexOptionKind.AllowPageLocks, IndexOptionKind.AllowRowLocks,
                IndexOptionKind.FillFactor,
                IndexOptionKind.DropExisting, IndexOptionKind.MaxDop, 
                IndexOptionKind.Online, IndexOptionKind.PadIndex, 
                IndexOptionKind.SortInTempDB, IndexOptionKind.StatisticsNoRecompute
            };

        static IndexOptionKind[] _alterIndexSetOptions = new IndexOptionKind[]
            {
                IndexOptionKind.AllowPageLocks, IndexOptionKind.AllowRowLocks,
                IndexOptionKind.IgnoreDupKey, IndexOptionKind.StatisticsNoRecompute
            };

        static IndexOptionKind[] _alterIndexSetOptionsTSql130 = new IndexOptionKind[]
            {
                IndexOptionKind.AllowPageLocks, IndexOptionKind.AllowRowLocks,
                IndexOptionKind.IgnoreDupKey, IndexOptionKind.StatisticsNoRecompute,
                IndexOptionKind.CompressionDelay
            };

        static IndexOptionKind[] _createSpatialIndexOptions = new IndexOptionKind[]
            {
                IndexOptionKind.AllowPageLocks, IndexOptionKind.AllowRowLocks, 
                IndexOptionKind.FillFactor, IndexOptionKind.DropExisting, 
                IndexOptionKind.MaxDop, IndexOptionKind.Online, 
                IndexOptionKind.PadIndex, IndexOptionKind.SortInTempDB, 
                IndexOptionKind.StatisticsNoRecompute
            };

        static IndexOptionKind[] _createSpatialIndexOptions110 = new IndexOptionKind[]
            {
                IndexOptionKind.AllowPageLocks, IndexOptionKind.AllowRowLocks,
                IndexOptionKind.FillFactor, IndexOptionKind.DropExisting,
                IndexOptionKind.MaxDop, IndexOptionKind.Online,
                IndexOptionKind.PadIndex, IndexOptionKind.SortInTempDB,
                IndexOptionKind.StatisticsNoRecompute, IndexOptionKind.DataCompression
            };

        static IndexOptionKind[] _alterIndexReorganizeOptionsTSql130 = new IndexOptionKind[]
            {
                IndexOptionKind.LobCompaction, IndexOptionKind.CompressAllRowGroups
            };

        static IndexOptionKind[] _createColumnstoreIndexOptionsTSql130 = new IndexOptionKind[]
            {
                IndexOptionKind.MaxDop, IndexOptionKind.DropExisting, IndexOptionKind.CompressionDelay, IndexOptionKind.SortInTempDB
            };

        static IndexOptionKind[] _createColumnstoreIndexOptionsTSql140 = new IndexOptionKind[]
            {
                IndexOptionKind.MaxDop, IndexOptionKind.DropExisting,
                IndexOptionKind.CompressionDelay, IndexOptionKind.SortInTempDB,
                IndexOptionKind.Online
            };

        #endregion

        [TestMethod]
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void ProhibitedIndexOptionsErrorTest150()
        {
            TSql150Parser parser150 = new TSql150Parser(true);

            IndexOptionErrorTest(parser150, _indexOptionInfos150,
                CreateColumnStoreIndex, "CREATE COLUMNSTORE INDEX", _createColumnstoreIndexOptionsTSql140);
            IndexOptionErrorTest(parser150, _indexOptionInfos150,
                CreateXmlIndex, "CREATE XML INDEX", _createXmlIndexOptions);
            IndexOptionErrorTest(parser150, _indexOptionInfos150,
                CreateSpatialIndex, "CREATE SPATIAL INDEX", _createSpatialIndexOptions110);
        }

        [TestMethod] 
        [Priority(0)]
        [SqlStudioTestCategory(Category.UnitTest)]
        public void ProhibitedIndexOptionsErrorTest130()
        {
            TSql130Parser parser130 = new TSql130Parser(true);
            IndexOptionErrorTest(parser130, _indexOptionInfos130, 
                AlterIndexReorganize, "ALTER INDEX REORGANIZE", _alterIndexReorganizeOptionsTSql130);
            IndexOptionErrorTest(parser130, _indexOptionInfos130,
                AlterIndexSet, "ALTER INDEX", _alterIndexSetOptionsTSql130);
            IndexOptionErrorTest(parser130, _indexOptionInfos130,
                CreateColumnStoreIndex, "CREATE COLUMNSTORE INDEX", _createColumnstoreIndexOptionsTSql130);
        }


        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void ProhibitedIndexOptionsErrorTest110()
        {
            TSql110Parser parser110 = new TSql110Parser(true);
            IndexOptionErrorTest(parser110, _indexOptionInfos100, CreateColumnStoreIndex, "CREATE COLUMNSTORE INDEX", IndexOptionKind.MaxDop, IndexOptionKind.DropExisting);
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void ProhibitedIndexOptionsErrorTest100()
        {
            TSql100Parser parser100 = new TSql100Parser(true);

            IndexOptionErrorTest(parser100, _indexOptionInfos100,
                CreateTypeColumnConstraint, "CREATE TYPE", IndexOptionKind.IgnoreDupKey);
            IndexOptionErrorTest(parser100, _indexOptionInfos100,
                CreateTypeTableConstraint, "CREATE TYPE", IndexOptionKind.IgnoreDupKey);

            IndexOptionErrorTest(parser100, _indexOptionInfos100,
                AlterTableAddElement, "ALTER TABLE", _alterTableAddIndexOptions);

            IndexOptionErrorTest(parser100, _indexOptionInfos100,
                AlterTableRebuildOne, "ALTER TABLE REBUILD PARTITION", _onePartitionRebuildOptions);
            IndexOptionErrorTest(parser100, _indexOptionInfos100,
                AlterTableRebuildAll, "ALTER TABLE REBUILD PARTITION", _allPartitionsRebuildOptions);

            IndexOptionErrorTest(parser100, _indexOptionInfos100, AlterIndexSet,
                "ALTER INDEX", _alterIndexSetOptions);

            IndexOptionErrorTest(parser100, _indexOptionInfos100,
                AlterIndexRebuildOne, "ALTER INDEX REBUILD PARTITION", _onePartitionRebuildOptions);

            IndexOptionErrorTest(parser100, _indexOptionInfos100,
                AlterIndexRebuildAll, "ALTER INDEX REBUILD PARTITION", _allPartitionsRebuildOptions);

            IndexOptionErrorTest(parser100, _indexOptionInfos100,
                AlterIndexReorganize, "ALTER INDEX REORGANIZE", IndexOptionKind.LobCompaction);

            IndexOptionErrorTest(parser100, _indexOptionInfos100,
                CreateIndex, "CREATE INDEX", _createIndexOptions);

            IndexOptionErrorTest(parser100, _indexOptionInfos100,
                CreateTableColumnConstraint, "CREATE TABLE", _createTableIndexOptions);
            IndexOptionErrorTest(parser100, _indexOptionInfos100,
                CreateTableTableConstraint, "CREATE TABLE", _createTableIndexOptions);
            IndexOptionErrorTest(parser100, _indexOptionInfos100,
                DeclareTableVarColumnConstraint, "DECLARE", _createTableIndexOptions);
            IndexOptionErrorTest(parser100, _indexOptionInfos100,
                DeclareTableVarTableConstraint, "DECLARE", _createTableIndexOptions);

            IndexOptionErrorTest(parser100, _indexOptionInfos100,
                CreateXmlIndex, "CREATE XML INDEX", _createXmlIndexOptions);

            IndexOptionErrorTest(parser100, _indexOptionInfos100,
                CreateFunctionColumnConstraint, "CREATE/ALTER FUNCTION", _createTableIndexOptions);
            IndexOptionErrorTest(parser100, _indexOptionInfos100,
                AlterFunctionTableConstraint, "CREATE/ALTER FUNCTION", _createTableIndexOptions);
            
            IndexOptionErrorTest(parser100, _indexOptionInfos100,
                CreateSpatialIndex, "CREATE SPATIAL INDEX", _createSpatialIndexOptions);
        }

 
        [TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void ProhibitedIndexOptionsErrorTest90()
        {
            TSql90Parser parser90 = new TSql90Parser(true);

            IndexOptionErrorTest(parser90, _indexOptionInfos90,
                AlterTableAddElement, "ALTER TABLE", _alterTableAddIndexOptions);

            IndexOptionErrorTest(parser90, _indexOptionInfos90, AlterIndexSet, 
                "ALTER INDEX", _alterIndexSetOptions);

            IndexOptionErrorTest(parser90, _indexOptionInfos90,
                AlterIndexRebuildOne, "ALTER INDEX REBUILD PARTITION", _onePartitionRebuildOptions);

            IndexOptionErrorTest(parser90, _indexOptionInfos90,
                AlterIndexRebuildAll, "ALTER INDEX REBUILD PARTITION", _allPartitionsRebuildOptions);

            IndexOptionErrorTest(parser90, _indexOptionInfos90,
                AlterIndexReorganize, "ALTER INDEX REORGANIZE", IndexOptionKind.LobCompaction);

            IndexOptionErrorTest(parser90, _indexOptionInfos90,
                CreateIndex, "CREATE INDEX", _createIndexOptions);

            IndexOptionErrorTest(parser90, _indexOptionInfos90,
                CreateTableColumnConstraint, "CREATE TABLE", _createTableIndexOptions);
            IndexOptionErrorTest(parser90, _indexOptionInfos90,
                CreateTableTableConstraint, "CREATE TABLE", _createTableIndexOptions);
            IndexOptionErrorTest(parser90, _indexOptionInfos90,
                DeclareTableVarColumnConstraint, "DECLARE", _createTableIndexOptions);
            IndexOptionErrorTest(parser90, _indexOptionInfos90,
                DeclareTableVarTableConstraint, "DECLARE", _createTableIndexOptions);

            IndexOptionErrorTest(parser90, _indexOptionInfos90,
                CreateXmlIndex, "CREATE XML INDEX", _createXmlIndexOptions);

            IndexOptionErrorTest(parser90, _indexOptionInfos90,
                CreateFunctionColumnConstraint, "CREATE/ALTER FUNCTION", _createTableIndexOptions);
            IndexOptionErrorTest(parser90, _indexOptionInfos90,
                AlterFunctionTableConstraint, "CREATE/ALTER FUNCTION", _createTableIndexOptions);
        }
    }
}
