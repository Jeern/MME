using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace MMEContracts
{
    public class MMEProperties
    {
        public const string AddInRegKey = @"Software\Jern\MME";
        public const string AddInRegValueKey = "AddInMainDirectory";

        public static string PluginMainDirectory
        {
            get
            {
#if DEBUG
                return Environment.CurrentDirectory + @"C:\MMEPlugins";
#else
                RegistryKey key = Registry.LocalMachine.OpenSubKey(AddInRegKey);
                return Convert.ToString(key.GetValue(AddInRegValueKey));
#endif
            }
        }
    }
}
