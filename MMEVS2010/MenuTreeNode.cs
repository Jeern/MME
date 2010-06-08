using System;
using System.Collections.Generic;
using System.Text;
using MMEContracts;

namespace MMEVS2010
{
    /// <summary>
    /// A MenuTreeNode is a node in a menu tree. It can have children (Submenus)
    /// </summary>
    public class MenuTreeNode
    {
        /// <summary>
        /// Creates a Root Node or so called Main menu.
        /// </summary>
        /// <param name="mainMenuCaption"></param>
        public MenuTreeNode(string mainMenuCaption)
        {
            m_MenuItem = new MenuItem(mainMenuCaption); 
        }

        /// <summary>
        /// Creates a Leaf node or so called Sub menu.
        /// </summary>
        /// <param name="menuItem"></param>
        public MenuTreeNode(IMenuItem menuItem)
        {
            m_MenuItem = menuItem;
        }

        private IMenuItem m_MenuItem;
        private Dictionary<Guid, MenuTreeNode> m_Children = new Dictionary<Guid, MenuTreeNode>();

        /// <summary>
        /// The MME MenuItem of this TreeNode
        /// </summary>
        public IMenuItem MenuItem
        {
            get { return m_MenuItem; }
        }

        /// <summary>
        /// Contains all the Child nodes of this tree node.
        /// </summary>
        public Dictionary<Guid, MenuTreeNode> Children
        {
            get { return m_Children; }
        }

    }
}
