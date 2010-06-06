﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MMEContracts
{
    public class MenuItem : IMenuItem
    {
        private string m_Caption;
        private Guid m_Id;
        private bool m_Seperator;
        private Regex m_VisibleWhenCompliantName;

        public MenuItem(string caption, bool seperator = false)
        {
            m_Caption = caption;
            m_Id = Guid.NewGuid();
            m_Seperator = seperator;
        }

        public string Caption
        {
            get { return m_Caption; }
        }

        public Guid Id
        {
            get { return m_Id; }
        }

        public bool Seperator
        {
            get { return m_Seperator; }
        }

        public Regex VisibleWhenCompliantName
        {
            get { return m_VisibleWhenCompliantName; }
        }

        Func<IMenuContext, bool> m_IsVisible = context => true;
        public Func<IMenuContext, bool> IsVisible
        {
            get
            {
                return m_IsVisible;
            }
            set
            {
                m_IsVisible = value;
            }
        }
    }
}
