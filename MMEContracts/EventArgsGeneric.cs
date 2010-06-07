using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMEContracts
{
    public class EventArgs<T> : EventArgs
    {
        private T m_Data;
        
        public T Data
        {
            get { return m_Data; }
        }

        public EventArgs(T data)
        {
            m_Data = data;
        }
    }
}
