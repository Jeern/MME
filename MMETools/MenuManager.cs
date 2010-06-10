using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMEContracts;

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
                menus.Add(seperator);
                menus.Add(buildAndGacMenu);
                return menus;
            }
            return menus;
        }

        private void BuildAndGacMenuClick(object sender, EventArgs<IMenuContext> e)
        {
            e.Data.Details.VSStudio.ExecuteCommand("Build.BuildSelection");
        }
    }
}
