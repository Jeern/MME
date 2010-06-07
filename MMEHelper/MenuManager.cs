using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMEContracts;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using EnvDTE80;

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
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    ShowFromCodeplex(menuContext.Details.VSStudio);
                }
                else
                {
                    ShowLocal(menuContext.Details.VSStudio);
                }
            }
        }

        private void ShowFromCodeplex(DTE2 vsStudio)
        {
            try
            {
                vsStudio.ExecuteCommand("View.URL", "http://www.google.com");
            }
            catch
            {
                ShowLocal(vsStudio);
            }
        }

        private void ShowLocal(DTE2 vsStudio)
        {
            vsStudio.ExecuteCommand("View.URL", @"C:\tmp.mht");
        }

    }
}
