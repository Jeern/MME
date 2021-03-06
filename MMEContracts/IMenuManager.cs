﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace MMEContracts
{
    [InheritedExport]
    public interface IMenuManager
    {
        string MainMenu();
        IEnumerable<IMenuItem> GetMenus(ContextLevels menuForLevel);
    }
}
