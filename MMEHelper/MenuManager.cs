﻿using System;
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

        private const string MenuHowToImplement = "How to add new menus ...";
        private const string MenuHowToDeploy = "How to deploy new menus ...";

        public IEnumerable<IMenuItem> GetMenus(ContextLevels menuForLevel)
        {
            List<IMenuItem> menuItems = new List<IMenuItem>(2);
            var menuImplement = new MenuItem(MenuHowToImplement);
            menuImplement.Click += menuClick;
            menuItems.Add(menuImplement);
            var menuDeploy = new MenuItem(MenuHowToDeploy);
            menuDeploy.Click += menuClick;
            menuItems.Add(menuDeploy);
            return menuItems;
        }

        private void menuClick(object sender, EventArgs<IMenuContext> e)
        {
            IMenuItem menu = sender as IMenuItem;
            HelpInfo helpInfo = GetHelpInfo(menu.Caption);
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                ShowFromCodeplex(e.Data.Details.VSStudio, helpInfo);
            }
            else
            {
                ShowLocal(e.Data.Details.VSStudio, helpInfo);
            }
        }

        private HelpInfo GetHelpInfo(string caption)
        {
            var helpInfo = new HelpInfo();
            switch (caption)
            {
                case MenuHowToImplement:
                    helpInfo.Url = "http://www.google.com";
                    helpInfo.FileName = @"C:\tmp.mht";
                    break;
                case MenuHowToDeploy:
                    helpInfo.Url = "http://www.google.com";
                    helpInfo.FileName = @"C:\tmp.mht";
                    break;
                default:
                    helpInfo.Url = string.Empty;
                    helpInfo.FileName = string.Empty;
                    break;
            }
            return helpInfo;
        }

        private void ShowFromCodeplex(DTE2 vsStudio, HelpInfo helpInfo)
        {
            try
            {
                vsStudio.ExecuteCommand("View.URL", helpInfo.Url);
            }
            catch
            {
                ShowLocal(vsStudio, helpInfo);
            }
        }

        private void ShowLocal(DTE2 vsStudio, HelpInfo helpInfo)
        {
            vsStudio.ExecuteCommand("View.URL", helpInfo.FileName);
        }

    }
}
