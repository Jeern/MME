using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMEContracts;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;
using EnvDTE;
using EnvDTE80;
using System.IO;

namespace Bewise.MMEWebDeploymentHelpers
{
    /// <summary>
    /// MME Extensions to allow copying to gac, recycling IIS app pools, and Attach debugger to IIS app pools, from context menu on a project item
    /// Authors: lionel.limozin@bewise.fr, sebastien.reynier@bewise.fr
    /// </summary>
    public class WebDeploymentHelpers : IMenuManager
    {
        #region IMenuManager Members

        /// <summary>
        /// Main Menu Title
        /// </summary>
        /// <returns></returns>
        public string MainMenu()
        {
            return Strings.MainTitle;
        }

        /// <summary>
        /// Sub Menus and Click Handlers
        /// Theses menus are only available on a project node
        /// </summary>
        /// <param name="menuForLevel"></param>
        /// <returns></returns>
        public IEnumerable<IMenuItem> GetMenus(ContextLevels menuForLevel)
        {
            if (menuForLevel == ContextLevels.Project)
            {
                //copy to gac menu
                var menu_copygac = new MMEContracts.MenuItem(Strings.CopyToGacTitle);
                menu_copygac.Click += Menu_CopyGac_Click;

                // recycle app polls menu
                var menu_recyclepools = new MMEContracts.MenuItem(Strings.RecyclePoolsTitle);
                menu_recyclepools.Click += Menu_RecyclePools_Click;

                //attach to process menu
                var menu_attachprocess = new MMEContracts.MenuItem(Strings.AttachToIssTitle);
                menu_attachprocess.Click += Menu_AttachProcess_Click;

                //return all menus
                return new List<IMenuItem>() { menu_copygac, menu_recyclepools, menu_attachprocess };

            }
            else return null;
        }

        #endregion

        /// <summary>
        /// Helper method to write to the dedicated output window
        /// </summary>
        /// <param name="e"></param>
        /// <param name="msg"></param>
        private static void WriteToOutput(EventArgs<IMenuContext> e, string msg)
        {
            //get output windows
            var win = e.Data.Details.Project.DTE.Windows.Item("{34E76E81-EE4A-11D0-AE2E-00A0C90FFFC3}");
            try
            {
                //search if output window contains output pane with our title
                var pane = win.Object.OutputWindowPanes.Item(Strings.OutputPaneTitle);
                pane.OutputString(msg);
                pane.OutputString(Environment.NewLine);
            }
            catch (ArgumentException)
            {
                //add the output pane if it is the first time we use it
                var pane = win.Object.OutputWindowPanes.Add(Strings.OutputPaneTitle);
                pane.OutputString(msg);
                pane.OutputString(Environment.NewLine);
            }

        }

        #region Business method

        /// <summary>
        /// Attach the current debugger to all App pools processes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Menu_AttachProcess_Click(object sender, EventArgs<IMenuContext> e)
        {
            try
            {
                WriteToOutput(e, Strings.StartAttachProcess);

                //get debugger object
                Debugger2 debugger = (Debugger2)e.Data.Details.VSStudio.Debugger;

                //get all processes and if the name match App pool process, attach debugger to it
                Processes processes = debugger.LocalProcesses;
                foreach (Process2 process in processes)
                {
                    if (process.Name.EndsWith("w3wp.exe"))
                    {
                        process.Attach();
                    }
                    if (process.Name.EndsWith("W3SVC.exe"))
                    {
                        process.Attach();
                    }
                }
                //write success
                WriteToOutput(e, Strings.SuccessMsgAttachIis);
            }
            catch (Exception ex)
            {
                //write error
                WriteToOutput(e, string.Concat(Strings.ErrorMsgAttachIis, " : ", ex.Message));
            }

        }

        /// <summary>
        /// Copy the current project output (with current configuration) to GAC using GacUtil
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Menu_CopyGac_Click(object sender, EventArgs<IMenuContext> e)
        {
            try
            {
                WriteToOutput(e, Strings.StartCopyGac);
                //get current config (debug, release, etc...)
                var currentConfig = e.Data.Details.Project.ConfigurationManager.ActiveConfiguration.ConfigurationName;
                //compose path to the output dll
                var path = string.Concat(e.Data.FilePath, "\\bin\\", currentConfig, "\\", e.Data.FileName.Replace(".csproj", ".dll"));
                //check if project produce a dll
                if (File.Exists(path) == false)
                {
                    WriteToOutput(e, Strings.OutputProjectNotADll);
                    return;
                }
                //start a build before copyin to GAC
                WriteToOutput(e, Strings.StartBuildMsg);
                e.Data.Details.VSStudio.ExecuteCommand("Build.RebuildSelection", "");

                //create process to gacutil.exe
                System.Diagnostics.ProcessStartInfo sti = new ProcessStartInfo(
                                                            Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFilesX86),
                                                                         @"Microsoft SDKs\Windows\v7.0A\bin\NETFX 4.0 Tools\GacUtil.exe"),
                                                            string.Concat("/i \"", path, "\" /f"));

                //setup process execution to hide and receive output
                sti.UseShellExecute = false;
                sti.RedirectStandardOutput = true;
                sti.WindowStyle = ProcessWindowStyle.Hidden;

                //start process
                System.Diagnostics.Process p = System.Diagnostics.Process.Start(sti);
                //wait
                p.WaitForExit();

                //write process output to the dedicated output pane (do not write another message, gacutil messages are sufficients)
                WriteToOutput(e, p.StandardOutput.ReadToEnd());



            }
            catch (Exception ex)
            {
                //write error
                WriteToOutput(e, string.Concat(Strings.ErrorMsgCopyGac, " : ", ex.Message));
            }
        }


        /// <summary>
        /// Recycle all app pools using WMI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Menu_RecyclePools_Click(object sender, EventArgs<IMenuContext> e)
        {
            try
            {
                WriteToOutput(e, Strings.StartRecycle);

                //find IIS managment object
                ManagementObject obj = new ManagementObject(@"\\.\root\microsoftiisv2");
                //query all pools
                ObjectQuery query = new ObjectQuery("Select * From IIsApplicationPool");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(obj.Scope, query);
                var results = searcher.Get();
                foreach (var ob in results)
                {
                    //recyle
                    ((ManagementObject)ob).InvokeMethod("Recycle", null);
                }
                //write success
                WriteToOutput(e, Strings.SuccessMsgRecycle);

            }
            catch (ManagementException)
            {
                WriteToOutput(e, string.Concat(Strings.ErrorMsgRecycle, " : IIS not found on this computer"));

            }
            catch (Exception ex)
            {
                //write error
                WriteToOutput(e, string.Concat(Strings.ErrorMsgRecycle, " : ", ex.Message));
            }
        }

        #endregion #region Business method
    }
}