using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace ScriptGenSettingsGenerator
{
    class Program
    {
        private static SettingsXmlReader settings = null;
        public static string GenerateFileHeader(string fileName, bool isAdapter)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"//------------------------------------------------------------------------------");
            sb.AppendFormat("// <copyright file=\"{0}\" company=\"Microsoft\">\r\n", Path.GetFileName(fileName));
            sb.AppendLine(@"//         Copyright (c) Microsoft Corporation.  All rights reserved.");
            sb.AppendLine(@"// </copyright>");
            sb.AppendLine(@"//------------------------------------------------------------------------------");
            sb.AppendLine();
            sb.AppendLine(@"using System;");
            sb.AppendLine(@"using System.Collections.Generic;");
            sb.AppendLine(@"using System.Collections.ObjectModel;");
            sb.AppendLine(@"using System.ComponentModel;");
            if (isAdapter)
            {
                sb.AppendLine(@"using Microsoft.Data.Schema.Common.Settings;");
                sb.AppendLine(@"using Microsoft.SqlServer.TransactSql.ScriptDom;");
            }
            sb.AppendLine();
            foreach (string str in settings.Usings)
            {
                sb.Append(@"using ");
                sb.AppendLine(str);
            }
            sb.AppendLine();
            if (isAdapter)
                sb.AppendLine("namespace Microsoft.SqlServer.TransactSql.ScriptDom.Settings");
            else
                sb.AppendLine("namespace Microsoft.SqlServer.TransactSql.ScriptDom");
            sb.AppendLine("{");
            return sb.ToString();
        }

        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: ScriptGenSettingsGenerator.exe [Settings XML File] [Settings Class File] [SettingsAdapter Class File]");
                return;
            }

            string settingsFile = args[0];
            string settingsOutputFile = args[1];
            string adapterOutputFile = args[2];

            // Read the settings file
            XmlReader reader = XmlReader.Create(settingsFile);
            settings = new SettingsXmlReader();
            settings.ReadSettings(reader);

            // Open the output files
            if (File.Exists(settingsOutputFile))
                File.Delete(settingsOutputFile);
            if (File.Exists(adapterOutputFile))
                File.Delete(adapterOutputFile);
            FileStream settingsFS = File.OpenWrite(settingsOutputFile);
            FileStream adapterFS = File.OpenWrite(adapterOutputFile);

            // Write the classes
            WriteSettingsFile(settingsOutputFile, new StreamWriter(settingsFS));
            WriteAdapterFile(adapterOutputFile, new StreamWriter(adapterFS));

            settingsFS.Close();
            adapterFS.Close();
        }

        private static void WriteAdapterFile(string adapterOutputFile, TextWriter writer)
        {
            writer.WriteLine(GenerateFileHeader(adapterOutputFile, true));
            writer.WriteLine("\tinternal partial class SqlScriptGeneratorOptionsAdapter : VSDBToolsOptionsSettingsBase");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tprivate SqlScriptGeneratorOptions adaptee;");
            writer.WriteLine();
            writer.WriteLine("\t\tpublic SqlScriptGeneratorOptionsAdapter(SqlScriptGeneratorOptions adaptee)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.adaptee = adaptee;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            foreach (SettingGroupDescriptior groups in settings.SettingGroups)
            {
                groups.WriteAdapterProperty(writer);
                writer.WriteLine();
            }
            writer.WriteLine("\t\tpublic override void Reset()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tadaptee.Reset();");
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t}");
            writer.WriteLine("}");
            writer.Flush();
        }

        private static void WriteSettingsFile(string settingsOutputFile, TextWriter writer)
        {
            writer.WriteLine(GenerateFileHeader(settingsOutputFile, false));
            writer.WriteLine("\t///<summary>");
            writer.WriteLine("\t///Controls the options for Sql Script Generation");
            writer.WriteLine("\t///</summary>");
            writer.WriteLine("\tpublic partial class SqlScriptGeneratorOptions");
            writer.WriteLine("\t{");

            StringBuilder propertiesBuilder = new StringBuilder();
            StringBuilder defaultsBuilder = new StringBuilder();
            StringBuilder maxsBuilder = new StringBuilder();
            StringBuilder minsBuilder = new StringBuilder();
            StringBuilder resetBuilder = new StringBuilder();

            StringWriter propertiesWriter = new StringWriter(propertiesBuilder);
            StringWriter defaultsWriter = new StringWriter(defaultsBuilder);
            StringWriter maxsWriter = new StringWriter(maxsBuilder);
            StringWriter minsWriter = new StringWriter(minsBuilder);
            StringWriter resetWriter = new StringWriter(resetBuilder);

            bool hasDefault = false;
            bool hasMin = false;
            bool hasMax = false;

            resetWriter.WriteLine("\t\t///<summary>");
            resetWriter.WriteLine("\t\t///Resets the options to their default value.");
            resetWriter.WriteLine("\t\t///</summary>");
            resetWriter.WriteLine("\t\tpublic void Reset()");
            resetWriter.WriteLine("\t\t{");

            foreach (SettingGroupDescriptior group in settings.SettingGroups)
            {
                if (group.Settings.Count == 0)
                    continue;

                hasDefault |= group.HasDefault;
                hasMin |= group.HasMin;
                hasMax |= group.HasMax;
                group.WriteResetLines(resetWriter);
                group.WriteProperty(propertiesWriter);
                group.WriteMax(maxsWriter);
                group.WriteMin(minsWriter);
                group.WriteDefault(defaultsWriter);

                propertiesWriter.WriteLine();

                if(group.HasMax)
                    maxsWriter.WriteLine();

                if(group.HasMin)
                    minsWriter.WriteLine();

                if(group.HasDefault)
                    defaultsWriter.WriteLine();
            }
            
            resetWriter.WriteLine("\t\t}");
            resetWriter.WriteLine();

            propertiesWriter.Close();
            defaultsWriter.Close();
            maxsWriter.Close();
            minsWriter.Close();
            resetWriter.Close();
            
            WriteRegion(writer, "Default Value Constants", defaultsBuilder.ToString(), hasDefault);
            WriteRegion(writer, "Maximum Constants", maxsBuilder.ToString(), hasMax);
            WriteRegion(writer, "Minimum Constants", minsBuilder.ToString(), hasMin);
            WriteRegion(writer, "Settings", propertiesBuilder.ToString(), true);
            WriteRegion(writer, "public void Reset()", resetBuilder.ToString(), true);
            
            writer.WriteLine("\t}");
            writer.WriteLine("}");
            writer.Flush();
        }

        private static void WriteRegion(TextWriter writer, string name, string contents, bool writeRegion)
        {
            if (writeRegion)
            {
                writer.WriteLine("\t\t#region {0}", name);
                writer.WriteLine();
                writer.Write(contents);
                writer.WriteLine("\t\t#endregion");
                writer.WriteLine();
            }
        }
    }
}
