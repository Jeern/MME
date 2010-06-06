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

    }
}
