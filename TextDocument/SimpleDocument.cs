using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextDocument
{
    public class SimpleDocument
    {
        private string m_text;

        public string Text
        {
            get { return m_text; }
            set { m_text = value; }
        }

        private string m_id;

        public string Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        private string m_test;

        public string Test
        {
            get { return m_test; }
            set { m_test = value; }
        }
    }
}
