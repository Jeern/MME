using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMEContracts
{
    public interface IMenuContext
    {
        string ItemName { get; } //Name
        string FileName { get; } //FileName
        string FilePath { get; } //Path 
        string FullFileName { get; } //FullPath
        ContextLevels Level { get; } //Levels
        DetailedContextInformation Details { get; }
    }
}
