//------------------------------------------------------------------------------
// <copyright file="SqlStudioTestCategoryAttribute.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlStudio.Tests.AssemblyTools.TestCategory
{
    public enum Category
    {
        UnitTest = 0,
        Functional,
        Performance,
        Stress,
        Fuzzing,
        GQL,
        GQLCandidate,
        EndToEnd,
        DACCompatibility,
        BVT
    }

    public enum Priority
    {
        P0,
        P1,
        P2,
        P3
    }

    public enum Feature
    {
        AlwaysEncrypted,
        AlwaysEncryptedEnclaveEnabled,
        AppDBProj,
        ASTBuilder, // was scriptdom
        Build,
        DAC2_0,
        DAC,
        DataToolsOperations,
        DatabaseUnitTesting,
        DataCompare,
        DataGeneration,
        DataWarehouseUnitTesting, //To execute test by Feature:DataWarehouseUnitTesting switch in commandline
        Debugger, // TSQL or SQL CLR
        Deploy,
        EntityCodeGeneration,
        EntityDesigner,
        EntityModel,
        FindAllReferences,
        GoToDefinition,
        GraphDb,
        ImportToProject,
        ImportExportService,
        Interpreter,
        Ledger,
        LocalDB,
        ModelCompare,
        ModelUpdater,
        ModelValidation,
        PowerBuffer,
        PublicApi,
        PublicContributors,
        PublicModel,
        Publish,
        ProjectSystem,
        Refactoring,
        ReverseEngineer,
        SCCI,
        SchemaCompare,
        SchemaView,
        ServerExplorer,
        Setup,
        SqlClr,
        StaticCodeAnalysis,
        StressTesting,
        TableDesigner,
        TSqlEditor,
        TSqlModel, // DataSchemaModel, SqlSchemaModel, etc.
        Utility,
        ExternalStream
    }

    // NOTE: This enum is defined to facilitate the load balancing of the CIT execution in the SNAP queue
    // and should not be explicity used, added or changed by developers when creating or modifying test cases.
    //
    // The value of this enum is used as property for SqlStudioTestCategoryAttribute to provide
    // the capability of filtering groups of tests within a given test assembly. This in turn allows each group
    // of tests to run on an individual VM in the queue.
    //
    // When creating a new test, you should *not* populate this property. Providing the test is qualified for
    // execution in the queue, the test will automatically go to Group0 without explicitly setting this property.
    //
    // A tool will be run periodically, as needed, that will automatically populate or change this property
    // based data that is obtained from previous test runs.
    public enum TestGroup
    {
        GroupNotSpecified,
        Group0,
        Group1,
        Group2,
        Group3,
        Group4,
        Group5,
        Group6,
        Group7,
        Group8,
        Group9,
        Group10,
        Group11,
        Group12,
        Group13,
        Group14,
        Group15,
        Group16,
        Group17,
        Group18,
        Group19,
    }

    [Flags]
    public enum SqlTestPlatform
    {
        OnPremises,
        SQLAzureV12,
        SQLDw,
        SQLServerless,
        AllPlatforms
    }

    [Flags]
    public enum Requires
    {
        None = 0,
        HeavyCPULoad = 1,
        HeavyMemoryLoad = 2,
        HeavyHDLoad = 4,
        MutateServerConfiguration = 8,
        Performance = 16,
    }

    public class SqlStudioTestCategoryAttribute : TestCategoryBaseAttribute
    {
        private static readonly string IntegrationTestCategory = "SSDTIntegration";
        public static readonly string SQLAzureTestCategory = "SQLAzure";

        private TestGroup _testGroup;
        private SqlTestPlatform _testPlatform;
        private IList<string> testCategories = new System.Collections.Generic.List<string>();
        private IList<Feature> _testFeatures = new List<Feature>();

        public TestGroup TestGroup
        {
            set
            {
                if (value != _testGroup)
                {
                    _testGroup = value;
                    this.OverwriteTestGroup(this.TestPlatform, _testGroup);
                }
            }
            get
            {
                return _testGroup;
            }
        }

        public SqlTestPlatform TestPlatform
        {
            set
            {
                if (_testPlatform != value)
                {
                    _testPlatform = value;
                    this.OverwriteTestGroup(_testPlatform, this.TestGroup);
                }
            }
            get
            {
                return _testPlatform;
            }
        }

        public Requires ParallelConstraints { get; set; }

        public SqlStudioTestCategoryAttribute(Category tc, params Feature[] fc)
        {
            _testGroup = TestCategory.TestGroup.GroupNotSpecified;

            testCategories.Add(tc.ToString());

            if (fc != null)
            {
                foreach (Feature f in fc)
                {
                    testCategories.Add(f.ToString());
                    _testFeatures.Add(f);
                }
            }
        }

        public SqlStudioTestCategoryAttribute(Category tc)
        {
            testCategories.Add(tc.ToString());
        }

        public SqlStudioTestCategoryAttribute(Feature tc)
        {
            testCategories.Add(tc.ToString());
        }

        public override IList<String> TestCategories { get {return testCategories;}  }

        public IList<Feature> TestFeatures { get { return _testFeatures; } }

        public bool IsIntegration
        {
            get
            {
                return testCategories.Contains(IntegrationTestCategory);
            }
            set
            {
                if (value && testCategories.Contains(IntegrationTestCategory) == false)
                {
                    testCategories.Add(IntegrationTestCategory);
                }
                else if (value == false && testCategories.Contains(IntegrationTestCategory))
                {
                    testCategories.Remove(IntegrationTestCategory);
                }
            }
        }

        /// <summary>
        /// Overwrite the test group based on the value from TestGroup, IsAzureTest and the priority
        /// </summary>
        private void OverwriteTestGroup(SqlTestPlatform testPlatform, TestGroup testGroup)
        {
            if (testPlatform == TestCategory.SqlTestPlatform.SQLAzureV12)
            {
                this.RemoveExistingTestGroup();
                this.AddAzureGroup();
            }
            else
            {
                if (testGroup == TestCategory.TestGroup.GroupNotSpecified)
                {
                    // Do nothing, keep the default group added by the constructor
                }
                else
                {
                    this.RemoveExistingAzureGroup();
                    this.AddTestGroup(testGroup);
                }
            }
        }

        private void AddTestGroup(TestGroup testGroup)
        {
            this.RemoveExistingTestGroup();
            testCategories.Add(testGroup.ToString());
        }

        private void AddAzureGroup()
        {
            if (!testCategories.Contains(SQLAzureTestCategory))
            {
                testCategories.Add(SQLAzureTestCategory);
            }
        }

        private void RemoveExistingTestGroup()
        {
            foreach (string testGroup in Enum.GetNames(typeof(TestGroup)))
            {
                testCategories.Remove(testGroup);
            }
        }

        private void RemoveExistingAzureGroup()
        {
            testCategories.Remove(SQLAzureTestCategory);
        }
    }

    // This attribute is used to tag test classes whose contituent tests should not be executed in different threads.
    // This is useful for test classes that have large initialization or cleanup costs.
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class NonParallelizableTestClass : Attribute
    {

    }

    // This attribute is used to tag test methods to order the execution of tests.  All lower priority tests will
    // execute before a higher priority tests.  This attribute only applies to classes with the NonParallelizableTestClass
    // attribute.
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ExecutionPriority : Attribute
    {
        public ExecutionPriority(int priority)
        {
            this.Priority = priority;
        }

        public int Priority { get; set; }
    }
}
