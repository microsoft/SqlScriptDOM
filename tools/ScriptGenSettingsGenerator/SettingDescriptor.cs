using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace ScriptGenSettingsGenerator
{
    public class SettingDescriptor
    {
        #region Private Fields

        private string type;
        private string name;
        private string defaultValue;
        private string min;
        private string max;
        private string summary;
        private string title;
        private string description;
        private bool browsable = true;
        private SettingGroupDescriptior group;
        private Dictionary<string, string> optionValues = new Dictionary<string, string>();

        #endregion

        #region Public Properties

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        public string Min
        {
            get { return min; }
            set { min = value; }
        }

        public string Max
        {
            get { return max; }
            set { max = value; }
        }

        public string Summary
        {
            get { return summary; }
            set { summary = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public bool Browsable
        {
            get { return browsable; }
            set { browsable = value; }
        }

        public SettingGroupDescriptior Group
        {
            get { return group; }
            set { group = value; }
        }

        public Dictionary<string, string> OptionValues
        {
            get { return optionValues; }
        }

        public bool HasMax
        {
            get { return !String.IsNullOrEmpty(max); }
        }

        public bool HasMin
        {
            get { return !String.IsNullOrEmpty(min); }
        }

        public bool HasDefault
        {
            get { return !String.IsNullOrEmpty(defaultValue); }
        }

        #endregion

        #region Methods

        public void WriteMax(TextWriter writer)
        {
            if (HasMax)
            {
                writer.WriteLine("\t\tprivate const {0} Max{1} = {2};",
                    type, name, max);
            }
        }

        public void WriteMin(TextWriter writer)
        {
            if (HasMin)
            {
                writer.WriteLine("\t\tprivate const {0} Min{1} = {2};",
                    type, name, min);
            }
        }

        public void WriteDefault(TextWriter writer)
        {
            if (HasDefault)
            {
                writer.WriteLine("\t\tprivate const {0} Default{1} = {2};",
                    type, name, defaultValue);
            }
        }

        public void WriteResetLine(TextWriter writer)
        {
            if (HasDefault)
            {
                writer.WriteLine("\t\t\tthis.{0} = Default{0};", name);
            }
        }

        public void WriteAdapterProperty(TextWriter writer)
        {
            WriteSummary(writer);
            WriteAttributes(writer);

            writer.WriteLine("\t\tpublic {0} {1}", type, name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget {{ return adaptee.{0}; }}", Name);
            writer.WriteLine("\t\t\tset {{ adaptee.{0} = value; }}", Name);
            writer.WriteLine("\t\t}");
        }

        public void WriteProperty(TextWriter writer)
        {
            writer.WriteLine("\t\t#region {0} {1}", type, name);

            string fieldName = String.Concat(Char.ToLower(name[0]), name.Substring(1));

            writer.WriteLine();
            writer.Write("\t\tprivate {0} {1}", type, fieldName);
            if (!String.IsNullOrEmpty(defaultValue))
            {
                writer.Write(" = Default{0}", name);
            }
            writer.WriteLine(";");

            writer.WriteLine();

            WriteSummary(writer);
            writer.WriteLine("\t\tpublic {0} {1}", type, name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget {{ return {0}; }}", fieldName);

            if ((String.IsNullOrEmpty(max)) && (String.IsNullOrEmpty(min)))
            {
                writer.WriteLine("\t\t\tset {{ {0} = value; }}", fieldName);
            }
            else
            {
                writer.WriteLine("\t\t\tset");
                writer.WriteLine("\t\t\t{");
                string ifType = "if";
                if (!String.IsNullOrEmpty(min))
                {
                    writer.WriteLine("\t\t\t\t{0}(value < Min{1})", ifType, name);
                    writer.WriteLine("\t\t\t\t{");
                    writer.WriteLine("\t\t\t\t\t{0} = Min{1};", fieldName, name);
                    writer.WriteLine("\t\t\t\t}");
                    ifType = "else if";
                }
                if (!String.IsNullOrEmpty(max))
                {
                    writer.WriteLine("\t\t\t\t{0}(value > Max{1})", ifType, name);
                    writer.WriteLine("\t\t\t\t{");
                    writer.WriteLine("\t\t\t\t\t{0} = Max{1};", fieldName, name);
                    writer.WriteLine("\t\t\t\t}");
                }
                writer.WriteLine("\t\t\t\telse");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t{0} = value;", fieldName);
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t}");
            }
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t#endregion");
        }

        private void WriteSummary(TextWriter writer)
        {
            if (!String.IsNullOrEmpty(summary))
            {
                writer.WriteLine("\t\t/// <summary>{0}</summary>", summary);
            }
        }

        private void WriteAttributes(TextWriter writer)
        {
            writer.WriteLine("\t\t[Setting]");
            foreach (KeyValuePair<string, string> optionPair in optionValues)
            {
                writer.WriteLine("\t\t[OptionValue({0}, \"{1}\")]", optionPair.Key, optionPair.Value);
            }

            if (!String.IsNullOrEmpty(description))
            {
                writer.WriteLine("\t\t[OptionDescription(\"{0}\")]", description);
            }

            if (!String.IsNullOrEmpty(title))
            {
                writer.WriteLine("\t\t[OptionTitle(\"{0}\")]", title);
            }

            if (!browsable)
            {
                writer.WriteLine("\t\t[Browsable(false)]");
            }

            if ((group != null) && (!String.IsNullOrEmpty(group.GroupName)))
            {
                writer.WriteLine("\t\t[OptionGroup(\"{0}\")]", group.GroupName);
            }
        }

        #endregion
    }
}
