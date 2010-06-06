using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE80;
using EnvDTE;

namespace MMEContracts
{
    /// <summary>
    /// Contains all details about a clicked menuItem.
    /// </summary>
    public class DetailedContextInformation
    {
        DTE2 m_VSStudio;
        Solution m_Solution;
        Project m_Project;
        ProjectItem m_Item;

        public DetailedContextInformation(DTE2 vsStudio, Solution solution, Project project, ProjectItem item)
        {
            m_VSStudio = vsStudio;
            m_Solution = solution;
            m_Project = project;
            m_Item = item;
        }

        public DTE2 VSStudio
        {
            get { return m_VSStudio; }
        }

        /// <summary>
        /// If the Solution was clicked, this contains the Solution. Otherwise null.
        /// </summary>
        public Solution Solution
        {
            get { return m_Solution; }
        }

        /// <summary>
        /// If a Project was clicked, this contains the Project. Otherwise null.
        /// </summary>
        public Project Project
        {
            get { return m_Project; }
        }

        /// <summary>
        /// If an item was clicked, this contains the item. Otherwise null.
        /// </summary>
        public ProjectItem Item
        {
            get { return m_Item; }
        }

        private const string SEP = "£$£";

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (m_Solution != null)
            {
                sb.Append(m_Solution.FullName);
                sb.Append(SEP);
            }
            if (m_Project != null)
            {
                sb.Append(m_Project.FullName);
                sb.Append(SEP);
            }
            if (m_Item != null)
            {
                sb.Append(m_Item.Name);
                sb.Append(SEP);
            }
            return sb.ToString();
        }

    }
}
