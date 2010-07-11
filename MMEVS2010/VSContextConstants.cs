using System;
using System.Collections.Generic;
using System.Text;

namespace MMEVS2010
{
    /// <summary>
    /// Constants used to get the different CommandBars in the visual studio explorer
    /// </summary>
    public static class VSContextConstants
    {
        /// <summary> The VS Constant Name for a VS Solution </summary>
        public const string Solution = "Solution";
        /// <summary> The VS Constant Name for a VS Project </summary>
        public const string Project = "Project";
        /// <summary> The VS Constant Name for a VS Solution folder </summary>
        public const string SolutionFolder = "Solution Folder";
        /// <summary> The VS Constant Name for a VS Project Folder</summary>
        public const string Folder = "Folder";
        /// <summary> The VS Constant Name for a Reference root folder </summary>
        public const string References = "Reference Root";
        /// <summary> The VS Constant Name for a Web Reference folder </summary>
        public const string WebReferences = "Web Reference Folder"; //"Web Folder"??
        /// <summary> The VS Constant Name for an AssemblyInfo.cs file</summary>
        public const string AssemblyInfo = "Item";
        /// <summary> The VS Constant Name for a VS ProjectItem </summary>
        public const string Item = "Item";
        /// <summary> The VS Constant Name for all other items </summary>
        public const string OtherItem = "";

        /// <summary> Not tested yet </summary>
        public const string MenuBar = "MenuBar";
        /// <summary> Not tested yet </summary>
        public const string Standard = "Standard";
        /// <summary> Not tested yet </summary>
        public const string Build = "Build";
        /// <summary> Not tested yet </summary>
        public const string ClassViewMultiselectProjectreferencesItems = "Class View Multi-select Project references Items";
        /// <summary> Not tested yet </summary>
        public const string ClassViewMultiselectProjectreferencesmembers = "Class View Multi-select Project references members";
        /// <summary> Not tested yet </summary>
        public const string ContextMenus = "Context Menus";
        /// <summary> Not tested yet </summary>
        public const string SortBy = "Sort By";
        /// <summary> Not tested yet </summary>
        public const string ShowColumns = "Show Columns";
        /// <summary> Not tested yet </summary>
        public const string ErrorList = "Error List";
        /// <summary> Not tested yet </summary>
        public const string DockedWindow = "Docked Window";
        /// <summary> Not tested yet </summary>
        public const string MenuDesigner = "Menu Designer";
        /// <summary> Not tested yet </summary>
        public const string PropertiesWindow = "Properties Window";
        /// <summary> Not tested yet </summary>
        public const string Toolbox = "Toolbox";
        /// <summary> Not tested yet </summary>
        public const string CodeWindow = "Code Window";
        /// <summary> Not tested yet </summary>
        public const string TaskList = "Task List";
        /// <summary> Not tested yet </summary>
        public const string ResultsList = "Results List";
        /// <summary> Not tested yet </summary>
        public const string StubProject = "Stub Project";
        /// <summary> Not tested yet </summary>
        public const string CrossProjectSolutionProject = "Cross Project Solution Project";
        /// <summary> Not tested yet </summary>
        public const string CrossProjectSolutionItem = "Cross Project Solution Item";
        /// <summary> Not tested yet </summary>
        public const string CrossProjectProjectItem = "Cross Project Project Item";
        /// <summary> Not tested yet </summary>
        public const string CrossProjectMultiProject = "Cross Project Multi Project";
        /// <summary> Not tested yet </summary>
        public const string CrossProjectMultiItem = "Cross Project Multi Item";
        /// <summary> Not tested yet </summary>
        public const string MiscFilesProject = "Misc Files Project";
        /// <summary> Not tested yet </summary>
        public const string CrossProjectMultiSolutionFolder = "Cross Project Multi Solution Folder";
        /// <summary> Not tested yet </summary>
        public const string CrossProjectMultiProjectFolder = "Cross Project Multi Project/Folder";
        /// <summary> Not tested yet </summary>
        public const string CommandWindow = "Command Window";
        /// <summary> Not tested yet </summary>
        public const string AutoHiddenWindows = "AutoHidden Windows";
        /// <summary> Not tested yet </summary>
        public const string ExpansionManager = "Expansion Manager";
        /// <summary> Not tested yet </summary>
        public const string FindRegularExpressionBuilder = "Find Regular Expression Builder";
        /// <summary> Not tested yet </summary>
        public const string ReplaceRegularExpressionBuilder = "Replace Regular Expression Builder";
        /// <summary> Not tested yet </summary>
        public const string WildCardExpressionBuilder = "Wild Card Expression Builder";
        /// <summary> Not tested yet </summary>
        public const string ExternalToolsArguments = "External Tools Arguments";
        /// <summary> Not tested yet </summary>
        public const string ExternalToolsDirectories = "External Tools Directories";
        /// <summary> Not tested yet </summary>
        public const string EasyMDIToolWindow = "Easy MDI Tool Window";
        /// <summary> Not tested yet </summary>
        public const string EasyMDIDocumentWindow = "Easy MDI Document Window";
        /// <summary> Not tested yet </summary>
        public const string EasyMDIDragging = "Easy MDI Dragging";
        /// <summary> Not tested yet </summary>
        public const string OpenDropDown = "Open Drop Down";
        /// <summary> Not tested yet </summary>
        public const string ClassViewProject = "Class View Project";
        /// <summary> Not tested yet </summary>
        public const string ClassViewItem = "Class View Item";
        /// <summary> Not tested yet </summary>
        public const string ClassViewFolder = "Class View Folder";
        /// <summary> Not tested yet </summary>
        public const string ClassViewGroupingFolder = "Class View Grouping Folder";
        /// <summary> Not tested yet </summary>
        public const string ClassViewMultiselect = "Class View Multi-select";
        /// <summary> Not tested yet </summary>
        public const string ClassViewMultiselectmembers = "Class View Multi-select members";
        /// <summary> Not tested yet </summary>
        public const string ClassViewMember = "Class View Member";
        /// <summary> Not tested yet </summary>
        public const string ClassViewGroupingMembers = "Class View Grouping Members";
        /// <summary> Not tested yet </summary>
        public const string ClassViewProjectReferencesFolder = "Class View Project References Folder";
        /// <summary> Not tested yet </summary>
        public const string ClassViewProjectReference = "Class View Project Reference";
        /// <summary> Not tested yet </summary>
        public const string ClassViewProjectReferenceItem = "Class View Project Reference Item";
        /// <summary> Not tested yet </summary>
        public const string ClassViewProjectReferenceMember = "Class View Project Reference Member";
        /// <summary> Not tested yet </summary>
        public const string ObjectBrowserObjectsPane = "Object Browser Objects Pane";
        /// <summary> Not tested yet </summary>
        public const string ObjectBrowserMembersPane = "Object Browser Members Pane";
        /// <summary> Not tested yet </summary>
        public const string ObjectBrowserDescriptionPane = "Object Browser Description Pane";
        /// <summary> Not tested yet </summary>
        public const string FindSymbol = "Find Symbol";
        /// <summary> Not tested yet </summary>
        public const string ReferenceItem = "Reference Item";
    }
}
