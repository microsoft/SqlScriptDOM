//------------------------------------------------------------------------------
// <copyright file="ServerEnumTests.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

// TODO: Decoupling Script DOM 
// Move out this test from ScriptDOM

// using System;
// using System.Data;
// using System.Collections.Generic;
// using System.IO;
// using Microsoft.Data.SqlClient;
// using ScriptDom=Microsoft.SqlServer.TransactSql.ScriptDom;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using SqlStudio.Tests.AssemblyTools.TestCategory;

// namespace SqlStudio.Tests.UTSqlScriptDom
// {
//     public partial class SqlDomTests
//     {
 
//         [TestMethod] 
//         [Priority(0)]
//         [SqlStudioTestCategory(Category.UnitTest)]
//         public void EventNotificationEventTypeTest()
//         {
//             // Some event types are excluded as these are unsupported at the time when we switched to a SQL 160 server
//             // If these become supported in the future and added to ScriptDom, this test will fail. You can simply remove them from the ignore list for it to pass again.
//             string[] ignoreTypes = new string[]
//             {
//                 "CREATE_EXTERNAL_STREAM",
//                 "DROP_EXTERNAL_STREAM",
//                 "CREATE_POOL",
//                 "ALTER_POOL",
//                 "DROP_POOL",
//                 "CREATE_SYNAPSE_WLG",
//                 "ALTER_SYNAPSE_WLG",
//                 "DROP_SYNAPSE_WLG",
//                 "CREATE_SYNAPSE_WLC",
//                 "ALTER_SYNAPSE_WLC",
//                 "DROP_SYNAPSE_WLC",
//                 "UNDO_DROP"
//             };

//             VerifyEnumValuesAgainstServer(
// @"select type, type_name from sys.event_notification_event_types
// where type < 10000 and type_name not in('CM_ENLISTMENT', 'CMA_ENLISTMENT', 'CM', 'CMA', 'XEVENT')
// union
// SELECT type, type_name FROM sys.trigger_event_types 
// where type < 10000
// union 
// select 0, 'unknown'
// order by type", 
//                 typeof(ScriptDom.EventNotificationEventType),
//                 ignoreTypes);
//         }


//         [TestMethod]
//         [Priority(0)]
//         [SqlStudioTestCategory(Category.UnitTest)]
//         public void EventNotificationEventGroupTest()
//         {
//             // Some event types are excluded as these are unsupported at the time when we switched to a SQL 160 server
//             // If these become supported in the future and added to ScriptDom, this test will fail. You can simply remove them from the ignore list for it to pass again.
//             string[] ignoreTypes = new string[]
//             {
//                 "DDL_EXTERNAL_STREAM_EVENTS",
//                 "DDL_SYNAPSE_POOL_EVENTS",
//                 "DDL_SYNAPSE_WLG_EVENTS",
//                 "DDL_SYNAPSE_WLC_EVENTS",
//                 "DDL_FIDO_EVENTS"
//             };

//             VerifyEnumValuesAgainstServer(
// @"select type, type_name from sys.event_notification_event_types
// where type >= 10000 and type_name not in ('TRC_SCALE_OUT_CLUSTER')
// union
// SELECT type, type_name FROM sys.trigger_event_types 
// where type >= 10000
// union 
// select 0, 'unknown'", 
//                 typeof(ScriptDom.EventNotificationEventGroup),
//                 ignoreTypes);
//         }

        // TODO: Decoupling Script DOM
        // private SqlDataReader GetDataReader(string command)
        // {
        //     // Connection string used for local VS testing
        //     //
        //     string connectionString = InstanceManager.DefaultSqlvNext.BuildConnectionString();
        //     SqlConnection conn = new SqlConnection(connectionString);
        //     conn.Open();
        //     SqlCommand cmd = new SqlCommand();
        //     cmd.Connection = conn;
        //     cmd.CommandText = command;
        //     return cmd.ExecuteReader();
        // }

        //private PimodDataReader GetDataReader(string command)
        //{
        //    PimodConnection conn = Context.TestEnvironment.TargetSqlEnvironment.CreatePimodConnection(SqlAuthenticationType.WindowsIntegrated);
        //    return conn.ExecuteReader(command);
        //}


        // private void VerifyEnumValuesAgainstServer(string command, Type enumType, IList<string> ignoreTypes)
        // {
            // TODO: Decoupling Script DOM

            // using (IDataReader reader = GetDataReader(command))
            // {
            //     List<Enum> extraEnums = new List<Enum>();
            //     foreach (Enum e in Enum.GetValues(enumType))
            //     {
            //         extraEnums.Add(e);
            //     }
            //     List<string> notIncludedInEnum = new List<string>();

            //     while (reader.Read())
            //     {
            //         int value = reader.GetInt32(0);
            //         Enum enumValue = (Enum)Enum.ToObject(enumType, value);
            //         if (extraEnums.Contains(enumValue))
            //         {
            //             extraEnums.Remove(enumValue);
            //         }
            //         else
            //         {
            //             string enumAsString = reader.GetString(1);
            //             if (!ignoreTypes.Contains(enumAsString))
            //             {
            //                 notIncludedInEnum.Add(enumAsString);
            //             }
            //         }
            //     }

            //     Assert.AreEqual(0, extraEnums.Count, "Extra enums: {0}", String.Join(Environment.NewLine, extraEnums));
            //     Assert.AreEqual(0, notIncludedInEnum.Count, "Missing in enums: {0}", String.Join(Environment.NewLine, notIncludedInEnum));
            // }
//         }
//     }
// }
