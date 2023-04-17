using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ScriptGenSettingsGenerator
{
    class SettingsXmlReader
    {
        private enum CurrentState
        {
            Main,
            InSummary,
            InTitle,
            InDescription,
            InOption,
            InUsing
        }

        public List<SettingGroupDescriptior> SettingGroups = new List<SettingGroupDescriptior>();
        public List<string> Usings = new List<string>();
        private SettingDescriptor currentSetting = null;
        private SettingGroupDescriptior currentGroup = null;

        public void ReadSettings(XmlReader reader)
        {
            SettingGroups.Clear();
            Usings.Clear();

            GroupStart(null);

            CurrentState state = CurrentState.Main;
            string currentValue = null;
            string currentDescription = null;
            
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.LocalName == "SettingGroup")
                    {
                        SettingEnd();
                        GroupStart(reader.GetAttribute("name"));
                    }
                    else if (reader.LocalName == "Setting")
                    {
                        SettingEnd();

                        currentSetting = new SettingDescriptor();
                        currentSetting.Name = reader.GetAttribute("name");
                        currentSetting.Type = reader.GetAttribute("type");

                        string format = "{0}";
                        if (currentSetting.Type.Equals("string", StringComparison.InvariantCultureIgnoreCase) || currentSetting.Type.Equals("System.String", StringComparison.InvariantCultureIgnoreCase))
                            format = "\"{0}\"";

                        string def = reader.GetAttribute("default");
                        string max = reader.GetAttribute("max");
                        string min = reader.GetAttribute("min");
                        string browsable = reader.GetAttribute("browsable");

                        if (!String.IsNullOrEmpty(def))
                            currentSetting.DefaultValue = String.Format(format, def);
                        if (!String.IsNullOrEmpty(min))
                            currentSetting.Min = String.Format(format, min);
                        if (!String.IsNullOrEmpty(max))
                            currentSetting.Max = String.Format(format, max);
                        if (!String.IsNullOrEmpty(browsable))
                            currentSetting.Browsable = Boolean.Parse(browsable);
                    }
                    else if (reader.LocalName == "Summary")
                    {
                        state = CurrentState.InSummary;
                    }
                    else if(reader.LocalName == "Title")
                    {
                        state = CurrentState.InTitle;
                    }
                    else if(reader.LocalName == "Using")
                    {
                        state = CurrentState.InUsing;
                    }
                    else if (reader.LocalName == "Description")
                    {
                        state = CurrentState.InDescription;
                    }
                    else if (reader.LocalName == "Option")
                    {
                        currentValue = reader.GetAttribute("value");
                        currentDescription = "";
                        state = CurrentState.InOption;
                    }
                }
                else if(reader.NodeType == XmlNodeType.EndElement)
                {
                    if ((reader.LocalName == "Settings") || (reader.LocalName == "Setting") || (reader.LocalName == "SettingGroup"))
                        SettingEnd();
                    state = CurrentState.Main;
                    if (reader.LocalName == "Option")
                    {
                        string format = "{0}";
                        if (currentSetting.Type.Equals("string", StringComparison.InvariantCultureIgnoreCase) || currentSetting.Type.Equals("System.String", StringComparison.InvariantCultureIgnoreCase))
                            format = "\"{0}\"";

                        currentSetting.OptionValues.Add(String.Format(format, currentValue), currentDescription);
                        currentValue = null;
                        currentDescription = null;
                    }
                }
                else if(reader.NodeType == XmlNodeType.Text)
                {
                    if (state == CurrentState.InSummary && currentSetting != null)
                        currentSetting.Summary += reader.Value.Trim();
                    else if (state == CurrentState.InUsing)
                        Usings.Add(reader.Value.Trim());
                    else if (state == CurrentState.InTitle && currentSetting != null)
                        currentSetting.Title += reader.Value.Trim();
                    else if (state == CurrentState.InOption && currentValue != null)
                        currentDescription += reader.Value.Trim();
                    else if (state == CurrentState.InDescription && currentSetting != null)
                        currentSetting.Description += reader.Value.Trim();
                }
            }
        }

        void GroupStart(string group)
        {
            currentGroup = new SettingGroupDescriptior();
            currentGroup.GroupName = group ?? "General";
            SettingGroups.Add(currentGroup);
        }

        void SettingEnd()
        {
            if (currentSetting != null)
            {
                currentSetting.Group = currentGroup;
                currentGroup.Settings.Add(currentSetting);
                currentSetting = null;
            }
        }
    }
}
