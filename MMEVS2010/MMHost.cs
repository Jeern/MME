using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using MMEContracts;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using EnvDTE;

namespace MMEVS2010
{
    public class MMHost  
    {
        [ImportMany]
        List<IMenuManager> m_MenuManagers = null;

        private Dictionary<Guid, IMenuManager> m_AssociatedMenuManagers = new Dictionary<Guid, IMenuManager>();
        private Dictionary<ContextLevels, MenuTree> m_MenusByContext = new Dictionary<ContextLevels, MenuTree>();
        
        /// <summary>
        /// The Constructor of MMHost uses MEF to load all the Plugins that contains menus.
        /// </summary>
        /// <param name="parentFolder"></param>
        /// <param name="solutionFolder"></param>
        public MMHost(string parentFolder, string solutionFolder)
        {
            try
            {
                AggregateCatalog catalog = null;
                if (Directory.Exists(parentFolder))
                {
                    catalog = new AggregateCatalog(new DirectoryCatalog(parentFolder), new DirectoryCatalog(solutionFolder));
                }
                else
                {
                    catalog = new AggregateCatalog(new DirectoryCatalog(solutionFolder));
                }
                var container = new CompositionContainer(catalog, true);
                container.ComposeParts(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("MMHost.Ctor(): " + ex.ToString());
            }
        }

        public MenuTree GetMenus(ContextLevels level)
        {
            if (!m_MenusByContext.ContainsKey(level))
                m_MenusByContext[level] = GetMenusFromAddIns(level);

            return m_MenusByContext[level];
        }

        private MenuTree GetMenusFromAddIns(ContextLevels level)
        {
            MenuTree menuTree = new MenuTree();
            foreach (IMenuManager menuManager in m_MenuManagers)
            {
                string mainMenu = menuManager.MainMenu();
                if (mainMenu != null && mainMenu.Trim().Length > 0)
                {
                    menuTree.Add(mainMenu);
                    IEnumerable<IMenuItem> menuList = menuManager.GetMenus(level);
                    foreach (IMenuItem menu in menuList)
                    {
                        menuTree.Add(mainMenu, menu);
                        if (!m_AssociatedMenuManagers.ContainsKey(menu.Id))
                            m_AssociatedMenuManagers.Add(menu.Id, menuManager);
                    }
                }
            }
            return menuTree;
        }

        public void MenuClicked(Guid clickedMenuId, MenuContext menuContext)
        {
            try
            {
                IMenuManager menuManagerHost = m_AssociatedMenuManagers[clickedMenuId];
                IMenuItem menuItem = GetMenus(menuContext.Level).AllNodes[clickedMenuId].MenuItem;
                menuItem.OnClick(menuItem, menuContext);
            }
            catch (Exception ex)
            {
                MessageBox.Show("MMHost.MenuClicked(): " + ex.ToString());
            }
        }
    }
}
