using System;
using System.Collections.Generic;
using System.Text;
using MMEContracts;

namespace MMEVS2010
{
    public class MenuTree
    {
        Dictionary<string, MenuTreeNode> m_RootNodes = new Dictionary<string, MenuTreeNode>();
        Dictionary<Guid, MenuTreeNode> m_AllNodes = new Dictionary<Guid, MenuTreeNode>();

        /// <summary>
        /// The RootNodes dictionary contains all the Root Nodes i.e. the Main menus added by MME to the Solution Explorer.
        /// </summary>
        public Dictionary<string, MenuTreeNode> RootNodes
        {
            get { return m_RootNodes; }
        }

        /// <summary>
        /// The AllNodes dictionary contains all the nodes of all menu trees added by MME to the Solution Explorer.
        /// Also includes the RootNodes.
        /// </summary>
        public Dictionary<Guid, MenuTreeNode> AllNodes
        {
            get { return m_AllNodes; }
        }

        /// <summary>
        /// Number of nodes in the MenuTree.
        /// </summary>
        public int Count
        {
            get
            {
                if (m_AllNodes == null)
                    return 0;

                return m_AllNodes.Count;
            }
        }

        /// <summary>
        /// Returns the number of RootNotes (i.e. Main menus added to the solution explorer by MME)
        /// </summary>
        public int CountRoots
        {
            get
            {
                if (m_RootNodes == null)
                    return 0;

                return m_RootNodes.Count;
            }
        }

        /// <summary>
        /// Adds a Mainmenu (rootnode) to the MenuTree.
        /// </summary>
        /// <param name="mainMenuCaption"></param>
        /// <returns></returns>
        public MenuTreeNode Add(string mainMenuCaption)
        {
            if (!m_RootNodes.ContainsKey(mainMenuCaption))
                return AddNode(mainMenuCaption);
            
            return m_RootNodes[mainMenuCaption];
        }

        public MenuTreeNode Add(string mainMenuCaption, IMenuItem menuItem)
        {
            if (!m_AllNodes.ContainsKey(menuItem.Id))
                return AddNode(mainMenuCaption, menuItem);
            
            return m_AllNodes[menuItem.Id];
        }

        private MenuTreeNode AddNode(string mainMenuCaption)
        {
            MenuTreeNode menuTreeNode = new MenuTreeNode(mainMenuCaption);
            m_RootNodes.Add(mainMenuCaption, menuTreeNode);
            m_AllNodes.Add(menuTreeNode.MenuItem.Id, menuTreeNode);
            return menuTreeNode;
        }

        private MenuTreeNode AddNode(string mainMenuCaption, IMenuItem menuItem)
        {
            MenuTreeNode menuTreeNode = new MenuTreeNode(menuItem);
            m_AllNodes.Add(menuTreeNode.MenuItem.Id, menuTreeNode);
            MenuTreeNode parentTreeNode = Add(mainMenuCaption);
            parentTreeNode.Children.Add(menuItem.Id, menuTreeNode);
            return menuTreeNode;
        }
    }
}
