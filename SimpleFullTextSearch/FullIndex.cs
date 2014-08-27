using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using TextDocument;

namespace SimpleFullTextSearch
{
    class FullIndex
    {
        private Dictionary<string, HashSet<string>> m_keyValueList = new Dictionary<string, HashSet<string>>();
        private byte[] fastCheck = new byte[char.MaxValue];
        private byte[] fastLength = new byte[char.MaxValue];
        private BitArray charCheck = new BitArray(char.MaxValue);
        private BitArray endCheck = new BitArray(char.MaxValue);
        private int maxWordLength = 0;
        private int minWordLength = int.MaxValue;

        public FullIndex()
        {
            List<wordObject> m_wordObjectList = new List<wordObject>();//TODO:Initialize
            WordLibrary m_library = new WordLibrary(m_wordObjectList);
            Init(m_library.Words);
        }

        private void Init(Dictionary<string, HashSet<string>> words)
        {
            foreach (string word in words.Keys)
            {
                maxWordLength = Math.Max(maxWordLength, word.Length);
                minWordLength = Math.Min(minWordLength, word.Length);

                for (int i = 0; i < 7 && i < word.Length; i++)
                {
                    fastCheck[word[i]] |= (byte)(1 << i);
                }

                for (int i = 7; i < word.Length; i++)
                {
                    fastCheck[word[i]] |= 0x80;
                }

                if (word.Length == 1)
                {
                    charCheck[word[0]] = true;
                }
                else
                {
                    fastLength[word[0]] |= (byte)(1 << (Math.Min(7, word.Length - 1)));
                    endCheck[word[word.Length - 1]] = true;
                }
            }
            m_keyValueList = words;
        }

        public bool Commit()
        {
            //TODO::Save m_keyValueList
            return true;
        }

        public bool CreateIndex(SimpleDocument document)
        {
            int index = 0;

            while (index < document.Text.Length)
            {
                int count = 1;

                if ((fastCheck[document.Text[index]] & 1) == 0)
                {
                    while (index < document.Text.Length - 1 && (fastCheck[document.Text[++index]] & 1) == 0) ;
                }

                char begin = document.Text[index];

                if (minWordLength == 1 && charCheck[begin])
                {
                    m_keyValueList[begin.ToString()].Add(document.Id);
                }

                for (int j = 1; j <= Math.Min(maxWordLength, document.Text.Length - index - 1); j++)
                {
                    char current = document.Text[index + j];

                    if ((fastCheck[current] & 1) == 0 && count == j)
                    {
                        ++count;
                    }

                    if ((fastCheck[current] & (1 << Math.Min(j, 7))) == 0)
                    {
                        break;
                    }

                    if (j + 1 >= minWordLength)
                    {
                        if ((fastLength[begin] & (1 << Math.Min(j, 7))) > 0 && endCheck[current])
                        {
                            string sub = document.Text.Substring(index, j + 1);

                            if (m_keyValueList.Keys.Contains(sub))// && (fastLength[begin] >> Math.Min(j, 7)) == 0 避免最短匹配后直接返回的话加上
                            {
                                m_keyValueList[sub].Add(document.Id);
                            }
                        }
                    }
                }

                index += count;
            }
            return true;
        }
    }
}
