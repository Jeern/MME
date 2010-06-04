using System;
using System.Collections.Generic;
using System.Text;
using MMEContracts;

namespace MMEVS2010
{
    public class MenuTreeNode
    {
        public MenuTreeNode(string mainMenuCaption)
        {
            m_MenuItem = new MenuItem(mainMenuCaption); 
        }

        public MenuTreeNode(IMenuItem menuItem)
        {
            m_MenuItem = menuItem;
        }

        private IMenuItem m_MenuItem;
        private Dictionary<Guid, MenuTreeNode> m_Children = new Dictionary<Guid, MenuTreeNode>();

        public IMenuItem MenuItem
        {
            get { return m_MenuItem; }
        }

        public Dictionary<Guid, MenuTreeNode> Children
        {
            get { return m_Children; }
        }

    }
}
