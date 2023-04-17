using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ScriptGenSettingsGenerator
{
	public class SettingGroupDescriptior
	{
        #region Private Fields

        private string groupName;
        private List<SettingDescriptor> settings = new List<SettingDescriptor>(); 

        #endregion

        #region Public Properties

        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; }
        }

        public IList<SettingDescriptor> Settings
        {
            get { return settings; }
        }

        public bool HasMax
        {
            get
            {
                return settings.Exists(delegate(SettingDescriptor setting) { return setting.HasMax; });
            }
        }

        public bool HasMin
        {
            get
            {
                return settings.Exists(delegate(SettingDescriptor setting) { return setting.HasMin; });
            }
        }

        public bool HasDefault
        {
            get
            {
                return settings.Exists(delegate(SettingDescriptor setting) { return setting.HasDefault; });
            }
        }

        #endregion

        #region Methods

        public void WriteMax(TextWriter writer)
        {
            if (HasMax)
            {
                writer.Write("\t\t#region ");
                writer.WriteLine(groupName);
                writer.WriteLine();
                foreach (SettingDescriptor setting in settings)
                {
                    setting.WriteMax(writer);
                }
                writer.WriteLine();
                writer.WriteLine("\t\t#endregion");
            }
        }

        public void WriteMin(TextWriter writer)
        {
            if (HasMin)
            {
                writer.Write("\t\t#region ");
                writer.WriteLine(groupName);
                writer.WriteLine();
                foreach (SettingDescriptor setting in settings)
                {
                    setting.WriteMin(writer);
                }
                writer.WriteLine();
                writer.WriteLine("\t\t#endregion");
            }
        }

        public void WriteDefault(TextWriter writer)
        {
            if (HasDefault)
            {
                writer.Write("\t\t#region ");
                writer.WriteLine(groupName);
                writer.WriteLine();
                foreach (SettingDescriptor setting in settings)
                {
                    setting.WriteDefault(writer);
                }
                writer.WriteLine();
                writer.WriteLine("\t\t#endregion");
            }
        }

        public void WriteResetLines(TextWriter writer)
        {
            if (HasDefault)
            {
                foreach (SettingDescriptor setting in settings)
                {
                    setting.WriteResetLine(writer);
                }
            }
        }

        public void WriteAdapterProperty(TextWriter writer)
        {
            bool first = true;
            foreach (SettingDescriptor setting in settings)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.WriteLine();
                }

                setting.WriteAdapterProperty(writer);
            }
        }

        public void WriteProperty(TextWriter writer)
        {
            writer.Write("\t\t#region ");
            writer.WriteLine(groupName);

            foreach (SettingDescriptor setting in settings)
            {
                writer.WriteLine();
                setting.WriteProperty(writer);
            }
            writer.WriteLine();
            writer.WriteLine("\t\t#endregion");
        }

        #endregion
    }
}
