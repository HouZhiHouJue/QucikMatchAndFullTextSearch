using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HighLighter
{
    public class HtmlFormatter
    {
        private string m_startTag;

        public string StartTag
        {
            get { return m_startTag; }
            set { m_startTag = value; }
        }

        private string m_endTag;

        public string EndTag
        {
            get { return m_endTag; }
            set { m_endTag = value; }
        }
    }
}
