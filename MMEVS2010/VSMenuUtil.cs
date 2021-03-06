﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE80;
using MMEContracts;
using Microsoft.VisualStudio.CommandBars;
using EnvDTE;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.Windows;

namespace MMEVS2010
{
    /// <summary>
    /// VSMenuUtil handles all Visual Studio Menu Stuff for this VS Studio AddIn.
    /// </summary>
    public class VSMenuUtil 
    {
        private DTE2 m_VSStudio;
        private AddIn m_AddIn;
        private Dictionary<string, IMenuItem> m_VSMenuToMenuItem = new Dictionary<string, IMenuItem>();
        private Dictionary<Guid, CommandBarControl> m_MenuItemToVSMenu = new Dictionary<Guid, CommandBarControl>();
        private Dictionary<string, MenuTreeNode> m_VSMainMenuToMenuTreeNode = new Dictionary<string, MenuTreeNode>();
        private Dictionary<string, CommandBarPopup> m_VSMainMenuToPopup = new Dictionary<string, CommandBarPopup>();
        private Dictionary<string, ContextLevels> m_ContextsFromMenus = new Dictionary<string, ContextLevels>();
        private List<CommandBarEvents> menuItemHandlerList = new List<CommandBarEvents>();
        private MMHost m_Host;

        /// <summary>
        /// Starts up the Host that loads the Plugins via MEF.
        /// </summary>
        /// <param name="vsStudio"></param>
        /// <param name="addIn"></param>
        public VSMenuUtil(DTE2 vsStudio, AddIn addIn)
        {
            m_VSStudio = vsStudio;
            m_AddIn = addIn;
            string solutionFolder = GetSolutionFolder(m_VSStudio);
            string parentFolder = GetParentFolder(solutionFolder);
            m_Host = new MMHost(parentFolder, solutionFolder);
        }

        private string GetSolutionFolder(DTE2 vsStudio)
        {
            if (vsStudio == null || vsStudio.Solution == null || string.IsNullOrEmpty(vsStudio.Solution.FullName))
                return null;

            return Path.GetDirectoryName(m_VSStudio.Solution.FullName);
        }

        private string GetParentFolder(string solutionFolder)
        {
            if (string.IsNullOrEmpty(solutionFolder))
                return null;

            return Path.GetDirectoryName(solutionFolder);
        }

        /// <summary>
        /// Builds the menu trees for each of the 7 context levels.
        /// </summary>
        public void BuildMenus()
        {
            BuildMenuTree(ContextLevels.Solution);
            BuildMenuTree(ContextLevels.SolutionFolder);
            BuildMenuTree(ContextLevels.Project);
            BuildMenuTree(ContextLevels.Folder);
            BuildMenuTree(ContextLevels.References);
            BuildMenuTree(ContextLevels.Item);
            BuildMenuTree(ContextLevels.WebReferences);
            BuildMenuTree(ContextLevels.MenuBar);
            BuildMenuTree(ContextLevels.CodeWindow);
        }

        private void BuildMenuTree(ContextLevels level)
        {
            MenuTree menus = m_Host.GetMenus(level);
            TraverseTree(menus, level);
        }

        private void TraverseTree(MenuTree tree, ContextLevels level)
        {
            foreach (MenuTreeNode node in tree.RootNodes.Values)
            {
                string mainMenuCommandName = GetMainMenuCommandName(level, node.MenuItem.Caption);
                CommandBarPopup menu = AddVSMainMenuItem(VSContextUtil.ContextToVSContext(level), node.MenuItem.Caption, level);
                SaveMainMenuInformation(mainMenuCommandName, menu, node);
                AddMainMenuClickEventHandler(menu, level, mainMenuCommandName, node.MenuItem.Caption);
                TraverseChildren(menu, node, level);
            }
        }

        private void TraverseChildren(CommandBarPopup vsmainMenu, MenuTreeNode treeNode, ContextLevels level)
        {
            if (treeNode.Children == null)
                return;

            int menuNumber = 1;
            bool seperator = false;
            foreach (MenuTreeNode node in treeNode.Children.Values)
            {
                if (!node.MenuItem.Seperator)
                {
                    CommandBarControl vsmenuItem = AddVSMenuItem(vsmainMenu, node.MenuItem, menuNumber, seperator, level);
                    AddClickEventHandler(vsmenuItem);
                    menuNumber++;
                    TraverseChildren(vsmainMenu, node, level);
                    seperator = false;
                }
                else
                {
                    seperator = true;
                }
            }
        }

        private CommandBar GetVSMainMenu(string commandBarName, int menuIndex)
        {
            CommandBar theBar = null;
            int index = 0;
            foreach (CommandBar bar in (CommandBars)m_VSStudio.DTE.CommandBars)
            {
                if (bar.Name == commandBarName)
                {
                    theBar = bar;
                    index++;
                    if (index == menuIndex)
                    {
                        return theBar;
                    }
                }
            }
            return theBar;
        }

        /// <summary>
        /// Adds a menu to the Visual Studio Solution explorer so that it is physically shown.
        /// </summary>
        /// <param name="commandBarName"></param>
        /// <param name="menuName"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public CommandBarPopup AddVSMainMenuItem(string commandBarName, string menuName, ContextLevels level)
        {
            var controls = GetVSMainMenu(commandBarName, 1).Controls;
            var vsmainMenu = controls.Add(MsoControlType.msoControlPopup, Missing.Value, Missing.Value,
                GetCommandbarIndex(controls, level), true) as CommandBarPopup;
            vsmainMenu.Caption = menuName;
            vsmainMenu.TooltipText = "";
            vsmainMenu.Tag = Guid.NewGuid().ToString();
            return vsmainMenu;
        }

        private int GetCommandbarIndex(CommandBarControls controls, ContextLevels level)
        {
            //Gets the Commandbar Index, Typically 1, but tries to place it right after the Tools menu, if the
            //menu is to be placed in the Top menu (MenuBar)
            switch (level)
            {
                case ContextLevels.MenuBar:
                    for (int i = 1; i <= controls.Count; i++)
                    {
                        if (controls[i].accName == "Tools")
                            return i + 1;
                    }
                    return controls.Count + 1;
                default:
                    return 1;
            }
        }

        private CommandBarControl AddVSMenuItem(CommandBarPopup vsmainMenu, IMenuItem menuToAdd, int position, bool seperator, ContextLevels level)
        {
            CommandBarControl vsmenuItem = vsmainMenu.Controls.Add(MsoControlType.msoControlButton, 1, "", position, true);
            vsmenuItem.BeginGroup = seperator;
            vsmenuItem.Tag = Guid.NewGuid().ToString();
            vsmenuItem.Caption = menuToAdd.Caption;
            vsmenuItem.TooltipText = "";
            SaveMenuInformation(vsmenuItem, menuToAdd, level);
            return vsmenuItem;
        }

        private void SaveMenuInformation(CommandBarControl vsMenu, IMenuItem menuToAdd, ContextLevels level)
        {
            m_VSMenuToMenuItem.Add(vsMenu.Tag, menuToAdd);
            m_MenuItemToVSMenu.Add(menuToAdd.Id, vsMenu);
            m_ContextsFromMenus.Add(vsMenu.Tag, level);
        }

        private void SaveMainMenuInformation(string mainMenuCommandName, CommandBarPopup mainMenu, MenuTreeNode node)
        {
            m_VSMainMenuToMenuTreeNode.Add(mainMenuCommandName, node);
            m_VSMainMenuToPopup.Add(mainMenuCommandName, mainMenu);
        }


        private void AddClickEventHandler(CommandBarControl menuItem)
        {
            CommandBarEvents menuItemHandler = (EnvDTE.CommandBarEvents)m_VSStudio.DTE.Events.get_CommandBarEvents(menuItem);
            menuItemHandler.Click += new _dispCommandBarControlEvents_ClickEventHandler(menuItemHandler_Click);
            menuItemHandlerList.Add(menuItemHandler);
        }

        private void AddMainMenuClickEventHandler(CommandBarPopup mainMenu, ContextLevels level, string commandName, string caption)
        {
            for (int i = m_VSStudio.Commands.Count; i >= 1; i--)
            {
                if (m_VSStudio.Commands.Item(i).Name == m_AddIn.ProgID + "." + commandName)
                {
                    m_VSStudio.Commands.Item(i).Delete();
                    break;
                }
            }

            //This makes the connect class'es QueryStatus method react to clicks on a main menu. A bit convoluted.
            //But the only way in VS2010 (in VS2008 it was accomplished with get_CommandBarEvents, but no longer possible -
            //see http://stackoverflow.com/questions/2977704/do-you-have-ideas-for-a-workaround-for-this-known-bug-in-visual-studio-2010s-add)
            Command mainMenuCommand = m_VSStudio.Commands.AddNamedCommand(m_AddIn, commandName, caption, "", false);
            mainMenuCommand.AddControl(mainMenu.CommandBar);
        }

        private string GetMainMenuCommandName(ContextLevels level, string name)
        {
            return "MME_" + Beautify(level.ToString()) + "_" + Beautify(name);
        }

        /// <summary>
        /// Removes _ and space
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string Beautify(string name)
        {
            return name.Replace(" ", string.Empty).Replace("_",string.Empty);
        }

        internal void menuItemHandler_Click(object CommandBarControl, ref bool Handled, ref bool CancelDefault)
        {
            try
            {
                CommandBarControl cbc = (CommandBarControl)CommandBarControl;
                string id = ((CommandBarControl)CommandBarControl).Tag;

                m_Host.MenuClicked(m_VSMenuToMenuItem[id].Id,
                    GetCurrentMenuContext(id));
            }
            catch (Exception ex)
            {
                MessageBox.Show("VSMenuUtil.menuItemHandler_Click(): " + ex.ToString());
            }
        }

        private MenuContext GetCurrentMenuContext(string vsMenuId)
        {
            return
                new MenuContext(SelectedItemName, SelectedItemFullPath, m_ContextsFromMenus[vsMenuId],
                new DetailedContextInformation(m_VSStudio, SelectedItemObj as Solution, GetProject(SelectedItemObj),
                    SelectedItemObj as ProjectItem, ActiveWindow, SelectedTextInActiveWindow));
        }

        private object SelectedItemObj
        {
            get
            {
                if (SelectedItem == null)
                    return null;

                return SelectedItem.Object;
            }
        }

        private EnvDTE.Window ActiveWindow
        {
            get
            {
                if (m_VSStudio == null)
                    return null;

                return m_VSStudio.ActiveWindow;
            }
        }

        private string SelectedTextInActiveWindow
        {
            get
            {
                if (ActiveWindow == null)
                    return null;

                var selected = ActiveWindow.Selection as TextSelection;
                if (selected != null)
                    return selected.Text;
                else
                    return null;
            }
        }

        

        private UIHierarchyItem SelectedItem
        {
            get
            {
                UIHierarchy uiHierarchy = m_VSStudio.ToolWindows.SolutionExplorer;
                if (uiHierarchy == null)
                    return null;

                object[] items = uiHierarchy.SelectedItems as object[];
                if (items == null || items.Length == 0)
                    return null;

                return items[0] as UIHierarchyItem;
            }
        }

        private string SelectedItemName
        {
            get
            {
                if (SelectedItem == null)
                    return string.Empty;

                return SelectedItem.Name;
            }
        }

        private string SelectedItemPath
        {
            get
            {
                var solution = SelectedItemObj as Solution;
                if (solution != null)
                    return GetPath(solution.FullName);

                if (IsSolutionFolder(SelectedItemObj))
                    return GetSolutionFolderPath(); //Currently just path of Solution (problem is what is really the path of Solution Folder ? Since Solution Folder is virtual it doesn't really have one.

                Project project = GetProject(SelectedItemObj);
                if (project != null)
                    return GetPath(GetProjectFullName(project));

                var item = SelectedItemObj as ProjectItem;
                if (item != null)
                    return GetPath(item.get_FileNames(1));

                return string.Empty;
            }
        }

        /// <summary>
        /// This method is really there to solve a problem with solution folders which does not have a filename, and therefore not a path. 
        /// Currently I just get the solution folders path as the SolutionFolder path (see SelectedItemPath)
        /// </summary>
        /// <returns></returns>
        private string GetSolutionFolderPath()
        {
            return GetPath(m_VSStudio.Solution.FullName);
        }

        private string SelectedItemFullPath
        {
            get
            {

                var solution = SelectedItemObj as Solution;
                if (solution != null)
                    return solution.FullName;

                if (IsSolutionFolder(SelectedItemObj))
                    return string.Empty;

                Project project = GetProject(SelectedItemObj);
                if (project != null)
                    return GetProjectFullName(project);

                var item = SelectedItemObj as ProjectItem;
                if (item != null)
                    return item.get_FileNames(1);

                return string.Empty;
            }
        }

        private string SelectedItemFileName
        {
            get
            {
                var solution = SelectedItemObj as Solution;
                if (solution != null)
                    return GetFileName(solution.FullName);

                if (IsSolutionFolder(SelectedItemObj))
                    return string.Empty;

                Project project = GetProject(SelectedItemObj);
                if (project != null)
                    return GetFileName(GetProjectFullName(project));

                var item = SelectedItemObj as ProjectItem;
                if (item != null)
                    return GetFileName(item.get_FileNames(1));

                return string.Empty;
            }
        }

        /// <summary>
        /// Returns af project. Because of problems with Projects that are subprojects of solution folders
        /// it is not as easy as casting. Instead we must do some more magic.
        /// </summary>
        /// <param name="selectedItemObject"></param>
        /// <returns></returns>
        private Project GetProject(object selectedItemObject)
        {
            var project = selectedItemObject as Project;
            if (project != null)
                return project;

            var item = selectedItemObject as ProjectItem;
            if (item == null)
                return null;

            return item.SubProject;
        }

        /// <summary>
        /// Evaluates if the item is a SolutionFolder. Does it by looking at the FileName property if it is empty it must be a solution folder.
        /// Because a normal Project always have a FileName. There is probably a more correct way of doing this, but will do
        /// for now.
        /// </summary>
        /// <param name="selectedItemObject"></param>
        /// <returns></returns>
        private bool IsSolutionFolder(object selectedItemObject)
        {
            var project = GetProject(selectedItemObject);
            if (project != null && string.IsNullOrEmpty(project.FileName))
                return true;
            return false;
        }

        /// <summary>
        /// Sets the visibility of all the Children of a mainMenu, and returns true if at least one of them is visible. Meaning main menu is
        /// visible. False if all are invisible. Meaning main menu is invisible.
        /// </summary>
        /// <param name="mainMenuCommandName"></param>
        /// <returns></returns>
        public bool SetVisibilityChildren(string mainMenuCommandName)
        {
            if (m_VSMainMenuToMenuTreeNode == null || m_VSMainMenuToMenuTreeNode.Count == 0)
                return false;

            MenuTreeNode node = m_VSMainMenuToMenuTreeNode[RemoveProgId(mainMenuCommandName)];
            return SetVisibilityChildren(node);
        }

        private string RemoveProgId(string mainMenuCommandName)
        {
            if(mainMenuCommandName.StartsWith(m_AddIn.ProgID + "."))
            {
                return mainMenuCommandName.Substring(m_AddIn.ProgID.Length + 1);
            }
            return mainMenuCommandName;
        }

        /// <summary>
        /// Sets the visibility of all the Children of a mainMenu, and returns true if at least one of them is visible. Meaning main menu is
        /// visible. False if all are invisible. Meaning main menu is invisible.
        /// </summary>
        /// <param name="node"></param>
        private bool SetVisibilityChildren(MenuTreeNode node)
        {
            if (node == null)
                return false;

            if (node.Children == null)
                return false;

            bool visible = false;
            foreach (MenuTreeNode childNode in node.Children.Values)
            {
                visible = visible | SetVisibility(childNode);
                visible = visible | SetVisibilityChildren(childNode);
            }
            return visible;
        }


        /// <summary>
        /// Set visibility of menuitem to true if the selected item is supossed to be Visible.
        /// </summary>
        /// <param name="node">The node for which to set the visibility.</param>
        private bool SetVisibility(MenuTreeNode node)
        {
            if (!node.MenuItem.Seperator)
            {
                bool visible = false;
                try
                {
                    visible = node.MenuItem.IsVisible(GetCurrentMenuContext(m_MenuItemToVSMenu[node.MenuItem.Id].Tag));
                }
                catch
                {
                    //Here I could have chosen to show a messagebox. But since this method is called quite often it would be unbearable.
                    //Instead I just do not show the menu if there is an exception
                    visible = false;
                }

                m_MenuItemToVSMenu[node.MenuItem.Id].Visible = visible;
                return visible;
            }
            return false;
        }

        /// <summary>
        /// Code borrowed from http://www.codeproject.com/KB/macros/zipstudio.aspx - Thank you...
        /// </summary>
        /// <param name="Project"></param>
        /// <returns></returns>
        private string GetProjectFullName(Project Project)
        {
            string filePath = Project.FullName;
            // Find the file extension of the project FullName.
            int extIndex = filePath.LastIndexOf('.');
            string filePathExt =
              (extIndex > 0) ? filePath.Substring(extIndex + 1) : "";
            // Find the file extension of the project UniqueName.
            extIndex = Project.UniqueName.LastIndexOf('.');
            string uniqueExt =
              (extIndex > 0) ? Project.UniqueName.Substring(extIndex + 1) : "";
            // If different use the UniqueName extension.
            if (filePathExt != uniqueExt)
            {
                // If the FullName does not have an extension,
                // add the one from UniqueName.
                if (filePathExt == "") filePath += "." + uniqueExt;
                // Else replace it.
                else filePath = filePath.Replace(filePathExt, uniqueExt);
            }
            return filePath;
        }

        /// <summary>
        /// Returns the Path - given a FileName
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string GetPath(string filename)
        {
            DirectoryInfo di = new DirectoryInfo(filename);
            if (string.IsNullOrEmpty(di.Extension))
                return PathAddSlash(filename);

            return PathAddSlash(di.FullName.Substring(0, di.FullName.Length - di.Name.Length));
        }

        /// <summary>
        /// Returns the FileName - given a Full path
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        private string GetFileName(string fullpath)
        {
            DirectoryInfo di = new DirectoryInfo(fullpath);
            if (string.IsNullOrEmpty(di.Extension))
                return string.Empty;

            return di.Name;
        }

        private string PathAddSlash(string path)
        {
            if (path.EndsWith(@"\"))
                return path;

            return path + @"\";
        }

    }
}
