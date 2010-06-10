using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMEContracts;
using EnvDTE;

namespace MMETools
{
    public class MenuManager : IMenuManager
    {
        public string MainMenu()
        {
            return "Managed Menu Extensions";
        }

        public IEnumerable<IMenuItem> GetMenus(ContextLevels menuForLevel)
        {
            var menus = new List<MenuItem>();
            if (menuForLevel == ContextLevels.Project)
            {
                var seperator = new MenuItem("", true);
                var buildAndGacMenu = new MenuItem("Build and add to GAC");
                buildAndGacMenu.Click += BuildAndGacMenuClick;
                seperator.IsVisible = BuildAndGacMenuVisible;
                buildAndGacMenu.IsVisible = BuildAndGacMenuVisible;
                menus.Add(seperator);
                menus.Add(buildAndGacMenu);
                return menus;
            }
            return menus;
        }

        private void BuildAndGacMenuClick(object sender, EventArgs<IMenuContext> e)
        {
            e.Data.Details.VSStudio.ExecuteCommand("Build.RebuildSelection");
        }

        private bool BuildAndGacMenuVisible(IMenuContext context)
        {
            if (context.Details.Project == null)
                return false;

            ProjectItems items = context.Details.Project.ProjectItems;
            if (items == null)
                return false;

            if (items.Count <= 0)
                return false;

            for (int i = 1; i <= items.Count; i++)
            {
                if (items.Item(i).Name.EndsWith(".snk"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
