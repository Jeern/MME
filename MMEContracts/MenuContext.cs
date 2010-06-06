using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MMEContracts
{
    public class MenuContext : IMenuContext
    {

        private string m_ItemName;
        private string m_FullFileName;
        private ContextLevels m_Level;
        private DetailedContextInformation m_Details;

        public MenuContext(string itemName, string fullFileName, ContextLevels level, DetailedContextInformation details)
        {
            m_ItemName = itemName;
            m_FullFileName = fullFileName;
            m_Level = level;
            m_Details = details;
        }


        public string ItemName
        {
            get { return m_ItemName; }
        }

        public string FileName
        {
            get 
            {
                return Path.GetFileName(m_FullFileName);
            }
        }

        public string FilePath
        {
            get
            {
                return Path.GetDirectoryName(m_FullFileName);
            }
        }

        public string FullFileName
        {
            get { return m_FullFileName; }
        }

        public ContextLevels Level
        {
            get { return m_Level; }
        }

        public DetailedContextInformation Details
        {
            get { return m_Details; }
        }

        private const string SEP = "£$£";

        internal string DictionaryKey
        {
            get 
            {
                var sb = new StringBuilder();
                sb.Append(ItemName);
                sb.Append(SEP);
                sb.Append(FileName);
                sb.Append(SEP);
                sb.Append(FilePath);
                sb.Append(SEP);
                sb.Append(FullFileName);
                sb.Append(SEP);
                sb.Append(Level.ToString());
                sb.Append(SEP);
                sb.Append(Details.ToString());
                return sb.ToString();
            }
        }
    }
}
