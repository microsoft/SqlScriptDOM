//------------------------------------------------------------------------------
// <copyright file="StreamAnalyticsExtensionTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlStudio.Tests.AssemblyTools.TestCategory;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    public partial class SqlDomTests
    {
		/// <summary>
		/// Verifies that the SQLDOM that ships with SQL Server does not have the StreamAnalyticsExtension properties
		/// </summary> 
		[TestMethod] 
		[Priority(0)]
		[SqlStudioTestCategory(Category.UnitTest)]
        public void StreamAnalyticsExtensionNegativeTest()
        {
            List<string> streamAnalyticsExtensionProperties = new List<string>()
            {
                "PartitionBy",
                "TimestampBy",
                "Over",
            };

            foreach (string analyticsExtensionProperty in streamAnalyticsExtensionProperties)
            {
                PropertyInfo property = typeof(NamedTableReference).GetProperty(analyticsExtensionProperty);
                Assert.IsNull(property, string.Format("Property {0} should not exist in NamedTableReference class.", analyticsExtensionProperty));
            }
        } 
    }
}