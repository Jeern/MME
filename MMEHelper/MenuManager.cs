using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMEContracts;

namespace MMEHelper
{
    public class MenuManager : IMenuManager
    {
        public string MainMenu()
        {
            return "Managed Menu Extensions";
        }

        private const string MenuShowHowTo = "Show how To...";

        public IEnumerable<IMenuItem> GetMenus(ContextLevels menuForLevel)
        {
            List<IMenuItem> menuItems = new List<IMenuItem>(1);
            var m1 = new MenuItem(MenuShowHowTo);
            menuItems.Add(m1);
            return menuItems;
        }

        public void MenuClicked(IMenuItem clickedMenu, IMenuContext menuContext)
        {
            if (clickedMenu.Caption == MenuShowHowTo)
            {
                menuContext.Details.VSStudio.ExecuteCommand("View.Url", "http://www.google.com");
            }
        }
    }
}
