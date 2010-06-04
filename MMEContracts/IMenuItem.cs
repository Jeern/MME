using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MMEContracts
{
    public interface IMenuItem
    {
        string Caption { get; }
        Guid Id { get; }
        bool Seperator { get; }
        Regex VisibleWhenCompliantName { get; }
    }
}
