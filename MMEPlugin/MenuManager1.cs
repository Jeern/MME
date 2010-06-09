using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using EnvDTE80;
using MMEContracts;

namespace MMEPlugin
{
    public class MenuManager1 : IMenuManager
    {
        public IEnumerable<IMenuItem> GetMenus(ContextLevels menuForLevel)
        {
            var menuItems = new List<IMenuItem>();
            //TODO: Change name of menuitem here
            var menuItem1 = new MenuItem("MenuItem1");
            menuItem1.Click += MenuClick;
            menuItems.Add(menuItem1);
            //TODO: Add more menuitems here
            return menuItems;
        }

        private void MenuClick(object sender, EventArgs<IMenuContext> e)
        {
            //TODO: Add functionality for clicking the menu here. You can access the MenuContext
            //by using e.Data, and the MenuItem by casting sender to MenuItem like this:
            var menuItem = sender as MenuItem;
            var menuContext = e.Data;
        }

        public string MainMenu()
        {
            //TODO: Change name of the MainMenu.
            return "MMEPlugin1";
        }
    }
}
