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

namespace MMEVS2010
{
    public class MMHost  
    {
        [ImportMany]
        List<IMenuManager> m_MenuManagers;
        Dictionary<Guid, IMenuManager> m_AssociatedMenuManagers = new Dictionary<Guid, IMenuManager>();
        Dictionary<ContextLevels, MenuTree> m_MenusByContext = new Dictionary<ContextLevels, MenuTree>();
        

 
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
                menuManagerHost.MenuClicked(GetMenus(menuContext.Level).AllNodes[clickedMenuId].MenuItem, menuContext);
            }
            catch (Exception ex)
            {
                MessageBox.Show("MMHost.MenuClicked(): " + ex.ToString());
            }
        }

        //private Collection<AddInToken> GetAddIns()
        //{
        //    string addInRoot = AddInProperties.MainDirectory;
        //    string[] messages = AddInStore.Rebuild(addInRoot);
        //    WriteMessages(messages);
        //    return AddInStore.FindAddIns(typeof(MenuManagerHostView), addInRoot);
        //}

        //private List<IMenuManager> GetMenuManagers(Collection<AddInToken> addIns)
        //{
        //    List<MenuManagerHostView> menuManagers = new List<MenuManagerHostView>();
        //    if (addIns != null && addIns.Count > 0)
        //    {
        //        foreach (AddInToken addIn in addIns)
        //        {
        //            menuManagers.Add(addIn.Activate<MenuManagerHostView>(AppDomain.CurrentDomain));
        //        }
        //    }
        //    return menuManagers;
        //}

    }
}
