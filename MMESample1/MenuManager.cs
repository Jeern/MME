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
            m1.Click += MenuClick;
            m1.IsVisible = context => context.ItemName.StartsWith("M");
            MenuItem m2 = new MenuItem("Hardy 2");
            m2.Click += MenuClick;
            m2.IsVisible = context => context.Level == ContextLevels.Project;

            menuItems.Add(m1);
            menuItems.Add(m2);
            return menuItems;
        }

        private void MenuClick(object sender, EventArgs<IMenuContext> e)
        {
            System.Windows.Forms.MessageBox.Show("Hardy Rocks");
        }
    }
}
