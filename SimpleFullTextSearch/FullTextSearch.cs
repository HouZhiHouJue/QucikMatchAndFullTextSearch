using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleFullTextSearch
{
    class FullTextSearch
    {
        //All Relation with And
        //Simple Return All DocumentId
        public HashSet<string> SearchText(List<string> basicWord)
        {
            List<wordObject> m_wordObjectList = new List<wordObject>();//TODO:Initialize
            WordLibrary m_library = new WordLibrary(m_wordObjectList);
            HashSet<string> m_result = new HashSet<string>();
            foreach (string word in basicWord)
            {
                if (m_library.Words.Keys.Contains(word))
                    m_result.UnionWith(m_library.Words[word]);
            }
            return m_result;
        }
    }
}
