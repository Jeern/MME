using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using System.Windows;

namespace MMEVS2010
{
    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2, IDTCommandTarget
    {
        private DTE2 m_VSStudio;
        private AddIn m_AddIn;
        private VSMenuUtil m_VSMenuUtil;
        private bool m_StartupCompleted = false;

        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public Connect()
        {
        }

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            try
            {
                m_VSStudio = application as DTE2;
                m_AddIn = addInInst as AddIn;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connect.OnConnection(): " + ex.ToString());
            }
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
            try
            {
                m_VSMenuUtil = new VSMenuUtil(m_VSStudio, m_AddIn);
                m_VSMenuUtil.BuildMenus();
                m_StartupCompleted = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Connect.OnStartupComplete(): " + ex.ToString());
            }
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }

        //The Exec and QueryStatus are fired when a MainMenu is clicked.

        /// <summary>
        /// Necessary part of the IDTCommandTarget interface. But not used here.
        /// </summary>
        /// <param name="CmdName"></param>
        /// <param name="ExecuteOption"></param>
        /// <param name="VariantIn"></param>
        /// <param name="VariantOut"></param>
        /// <param name="Handled"></param>
        public void Exec(string CmdName, vsCommandExecOption ExecuteOption, ref object VariantIn, ref object VariantOut, ref bool Handled)
        {
            //Do nothing. There does not need to be a special action when the Mainmenu is clicked.
        }

        /// <summary>
        /// Necessary part of the IDTCommandTarget interface. Is called by the VS Studio AddIn framework each time the status of the commands could change.
        /// Among other things when the user clicks one of the main menus.
        /// </summary>
        /// <param name="CmdName"></param>
        /// <param name="NeededText"></param>
        /// <param name="StatusOption"></param>
        /// <param name="CommandText"></param>
        public void QueryStatus(string CmdName, vsCommandStatusTextWanted NeededText, ref vsCommandStatus StatusOption, ref object CommandText)
        {
            if (m_StartupCompleted)
            {
                m_VSMenuUtil.SetVisibilityChildren(CmdName);
            }            
        }
    }
}
