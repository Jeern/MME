using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections.ObjectModel;
using MMEContracts;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using EnvDTE;
using System.Windows;

namespace MMEVS2010
{
    /// <summary>
    /// The MMHosst is responsible for loading all the MEF Plugins conatining MenuManagers
    /// </summary>
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
                var directoryCatalogs = new List<DirectoryCatalog>();
                AddToDirectoryCatalog(directoryCatalogs, MMEProperties.PluginMainDirectory);
                AddToDirectoryCatalog(directoryCatalogs, parentFolder);
                AddToDirectoryCatalog(directoryCatalogs, solutionFolder);
                var catalog = new AggregateCatalog(directoryCatalogs);
                var container = new CompositionContainer(catalog, true);
                container.ComposeParts(this);
                catalog.Dispose();
                container.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("MMHost.Ctor(): " + ex.ToString());
            }
        }

        private void AddToDirectoryCatalog(List<DirectoryCatalog> list, string folder)
        {
            if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
            {
                list.Add(new DirectoryCatalog(folder));
            }
        }

        /// <summary>
        /// Get all menus from the MEF Plugins
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public MenuTree GetMenus(ContextLevels level)
        {
            if (!m_MenusByContext.ContainsKey(level))
                m_MenusByContext[level] = GetMenusFromPlugins(level);

            return m_MenusByContext[level];
        }

        private MenuTree GetMenusFromPlugins(ContextLevels level)
        {
            MenuTree menuTree = new MenuTree();
            foreach (IMenuManager menuManager in m_MenuManagers)
            {
                string mainMenu = menuManager.MainMenu();
                if (mainMenu != null && mainMenu.Trim().Length > 0)
                {
                    menuTree.Add(mainMenu);
                    IEnumerable<IMenuItem> menuList = menuManager.GetMenus(level);
                    if (menuList != null)
                    {
                        foreach (IMenuItem menu in menuList)
                        {
                            menuTree.Add(mainMenu, menu);
                            if (!m_AssociatedMenuManagers.ContainsKey(menu.Id))
                                m_AssociatedMenuManagers.Add(menu.Id, menuManager);
                        }
                    }
                }
            }
            return menuTree;
        }

        /// <summary>
        /// Called when a VS Menu is clicked. Passes it on to the MME MenuItem.
        /// </summary>
        /// <param name="clickedMenuId"></param>
        /// <param name="menuContext"></param>
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
