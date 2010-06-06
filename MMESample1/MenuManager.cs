using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMEContracts;
using System.Text.RegularExpressions;

namespace MMESample1
{
    public class MenuManager : IMenuManager
    {
        public string MainMenu()
        {
            return "Managed Menu Extensions";
        }

        public IEnumerable<IMenuItem> GetMenus(ContextLevels menuForLevel)
        {
            List<IMenuItem> menuItems = new List<IMenuItem>(2);
            MenuItem m1 = new MenuItem("Hardy 1");
            m1.IsVisible = context => context.ItemName.StartsWith("M");
            MenuItem m2 = new MenuItem("Hardy 2");

            menuItems.Add(m1);
            menuItems.Add(m2);
            return menuItems;
        }

        public void MenuClicked(IMenuItem clickedMenu, IMenuContext menuContext)
        {
            System.Windows.Forms.MessageBox.Show("Hardy Rocks");
        }

    }
}
