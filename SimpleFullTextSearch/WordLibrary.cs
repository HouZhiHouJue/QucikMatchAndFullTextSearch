using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleFullTextSearch
{
    class WordLibrary
    {
        public WordLibrary(List<wordObject> m_wordObjectList)
        {
            foreach (wordObject word in m_wordObjectList)
                m_words.Add(word.Word, word.DocumentIdList);
        }
        private Dictionary<string, HashSet<string>> m_words;

        public Dictionary<string, HashSet<string>> Words
        {
            get { return m_words; }
            set { m_words = value; }
        }
    }

    class wordObject
    {
        string m_word;

        public string Word
        {
            get { return m_word; }
            set { m_word = value; }
        }

        HashSet<string> m_documentIdList;

        public HashSet<string> DocumentIdList
        {
            get { return m_documentIdList; }
            set { m_documentIdList = value; }
        }
    }
}
