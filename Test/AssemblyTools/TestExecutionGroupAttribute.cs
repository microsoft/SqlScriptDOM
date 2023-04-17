using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlStudio.Tests.AssemblyTools.TestCategory
{
    /// <summary>
    /// This optional test attribute allows testers to have some control over the grouping algorithm
    /// that the QA test case deployment tool uses. Applying this attribute to a test method will force
    /// the tool to create a separate lab job for that test method. If you specify a group name, any
    /// test methods within the same class with the same group name will be contained within
    /// the same WTT job (if possible based on lab constraints). 
    /// The default behavior is to have 1 job for an entire test class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestExecutionGroup : Attribute
    {
        private string _group;

        public TestExecutionGroup()
        {
            _group = String.Empty;
        }

        public TestExecutionGroup(String group)
        {
            _group = group;
        }

        public string Group { get { return _group; } }
    }
}

