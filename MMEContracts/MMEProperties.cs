using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMEContracts
{
    public class MMEProperties
    {
#if DEBUG
        public static readonly string PluginMainDirectory = @"C:\MMEPlugins";
#else
        public static readonly string PluginMainDirectory = @"C:\MMEPlugins";
#endif
    }
}
