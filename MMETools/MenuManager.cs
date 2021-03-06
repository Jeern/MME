﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMEContracts;
using EnvDTE;
using EnvDTE80;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

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
            
            if (menuForLevel == ContextLevels.Project || menuForLevel == ContextLevels.Solution)
            {
                var seperator = new MenuItem("", true);
                if (menuForLevel == ContextLevels.Project)
                {
                    seperator.IsVisible = BuildAndGacMenuVisible;
                }
                menus.Add(seperator);
            }

            if (menuForLevel == ContextLevels.Project)
            {
                var buildAndGacMenu = new MenuItem("Copy GAC Command to Clipboard");
                buildAndGacMenu.Click += GacMenuClick;
                buildAndGacMenu.IsVisible = BuildAndGacMenuVisible;
                menus.Add(buildAndGacMenu);
            }

            if (menuForLevel == ContextLevels.Project || menuForLevel == ContextLevels.Solution)
            {
                var openInNotepadMenu = new MenuItem("Open in Notepad");
                openInNotepadMenu.Click += OpenInNotepadClick;
                menus.Add(openInNotepadMenu);
            }

            var openFileLocation = new MenuItem("Open file location");
            openFileLocation.Click += OpenFileLocationClick;
            openFileLocation.IsVisible = context => Directory.Exists(context.FilePath);
            menus.Add(openFileLocation);

            if (menuForLevel == ContextLevels.CodeWindow)
            {
                var google = new MenuItem("Search Google");
                google.IsVisible = context => !string.IsNullOrEmpty(context.Details.SelectedText);
                google.Click += GoogleClick;
                menus.Add(google);
                var bing = new MenuItem("Search Bing");
                bing.IsVisible = context => !string.IsNullOrEmpty(context.Details.SelectedText);
                bing.Click += BingClick;
                menus.Add(bing);
            }

            return menus;
        }

        private void GoogleClick(object sender, EventArgs<IMenuContext> e)
        {
            ShowUrl(e.Data.Details.VSStudio, BuildUrl("google", e.Data.Details.SelectedText));
        }

        private void BingClick(object sender, EventArgs<IMenuContext> e)
        {
            ShowUrl(e.Data.Details.VSStudio, BuildUrl("bing", e.Data.Details.SelectedText));
        }

        private string GetQueryStrings(string selectedText)
        {
            return selectedText.Replace("=", "").Replace(")", "").Replace("(", "").Replace("&", "").
                Replace("+", "").Replace("?", "").Replace(" ", "+");
        }

        private string BuildUrl(string searchEngine, string selectedText)
        {
            return string.Format("http://www.{0}.com/search?q={1}", searchEngine, GetQueryStrings(selectedText));
        }

        private void ShowUrl(DTE2 vsStudio, string url)
        {
            try
            {
                vsStudio.ExecuteCommand("View.URL", url);
            }
            catch {}
        }


        private void OpenInNotepadClick(object sender, EventArgs<IMenuContext> e)
        {
            OpenInNotepad(e.Data);
        }

        private void OpenInNotepad(IMenuContext context)
        {
            System.Diagnostics.Process.Start("Notepad.exe", context.FullFileName); 
        }

        private void OpenFileLocationClick(object sender, EventArgs<IMenuContext> e)
        {
            OpenFileLocation(e.Data);
        }

        private void OpenFileLocation(IMenuContext context)
        {
            System.Diagnostics.Process.Start(context.FilePath);
        }

        private void GacMenuClick(object sender, EventArgs<IMenuContext> e)
        {
            GacIt(e.Data.Details.Project);
        }

        private static void BuildIt(DTE2 dte)
        {
            dte.ExecuteCommand("Build.RebuildSelection");
        }

        private void GacIt(Project p)
        {
            string pathToGacUtil = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Microsoft SDKs\Windows\v7.0A\bin\NETFX 4.0 Tools\GacUtil.exe");
            if (!File.Exists(pathToGacUtil))
            {
                string message = "GacUtil.exe does not exist at " + Path.GetDirectoryName(pathToGacUtil);
                throw new ArgumentException(message);
            }

            if (p == null)
            {
                throw new ArgumentNullException("p", "The Project argument cannot be null");
            }

            //Thank you to: http://community.devexpress.com/forums/p/64756/219780.aspx
            string fullPath = p.Properties.Item("FullPath").Value.ToString();
            string outputPath = p.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value.ToString();
            string outputFileName = p.Properties.Item("OutputFileName").Value.ToString();
            string assemblyFile = Path.Combine(Path.Combine(fullPath, outputPath), outputFileName);

            if (!File.Exists(assemblyFile))
            {
                throw new ArgumentException(assemblyFile + " does not exist.");
            }
            
            pathToGacUtil = FixPath(pathToGacUtil);
            assemblyFile = FixPath(assemblyFile);

            string gacCommand = pathToGacUtil + " /i " + assemblyFile;
            Clipboard.SetText(gacCommand);
            MessageBox.Show(gacCommand, "Added to Clipboard");
            //Because you need administrative rights the line below can typically not be executed
            //System.Diagnostics.Process.Start(gacCommand);            
        }

        private string FixPath(string pathToGacUtil)
        {
            return "\"" + pathToGacUtil + "\"";
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
