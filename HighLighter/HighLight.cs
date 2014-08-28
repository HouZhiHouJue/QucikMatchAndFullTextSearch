using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace HighLighter
{
    public class HighLight
    {
        private HashSet<string> hash = new HashSet<string>();
        private byte[] fastCheck = new byte[char.MaxValue];
        private byte[] fastLength = new byte[char.MaxValue];
        private BitArray charCheck = new BitArray(char.MaxValue);
        private BitArray endCheck = new BitArray(char.MaxValue);
        private int maxWordLength = 0;
        private int minWordLength = int.MaxValue;

        private void Init(List<string> badwords)
        {
            foreach (string word in badwords)
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

                    hash.Add(word);
                }
            }
        }

        public string GetHightLightWords(List<string> words, string content, HtmlFormatter formatter)
        {
            Init(words);
            FormattterWord(formatter, ref content);
            return content;
        }

        //最长匹配法
        public bool FormattterWord(HtmlFormatter formatter, ref string text)
        {
            List<identifyRecord> m_list = new List<identifyRecord>();
            int index = 0;

            while (index < text.Length)
            {
                int count = 1;

                if ((fastCheck[text[index]] & 1) == 0)
                {
                    while (index < text.Length - 1 && (fastCheck[text[++index]] & 1) == 0) ;
                }

                char begin = text[index];

                for (int j = 1; j <= Math.Min(maxWordLength, text.Length - index - 1); j++)
                {
                    char current = text[index + j];

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
                            string sub = text.Substring(index, j + 1);

                            if (hash.Contains(sub) && (fastLength[begin] >> Math.Min(j, 7)) == 0) // 避免最短匹配后直接返回的话加上
                            {
                                identifyRecord m_recourd = new identifyRecord() { Begin = index, End = index + count };
                                if (m_list.Count == 0)
                                {
                                    m_list.Add(m_recourd);
                                    continue;
                                }
                                bool flag = false;
                                for (int i = 0; i < m_list.Count; i++)
                                {
                                    if (m_recourd.Begin <= m_list[i].End)
                                    {
                                        m_list[i].End = Math.Max(m_list[i].End, m_recourd.End);
                                        flag = true;
                                        break;
                                    }
                                }
                                if (!flag)
                                {
                                    m_list.Add(m_recourd);
                                    continue;
                                }
                                //return true;
                            }
                        }
                    }
                }

                index += count;
            }
            int length = 0;
            foreach (identifyRecord record in m_list)
            {
                text.Insert(record.Begin + length, formatter.StartTag);
                length += formatter.StartTag.Length;
                text.Insert(record.End + length, formatter.EndTag);
                length += formatter.EndTag.Length;
            }
            return true;
        }

    }

    public class identifyRecord
    {
        private int m_begin;

        public int Begin
        {
            get { return m_begin; }
            set { m_begin = value; }
        }

        private int m_end;

        public int End
        {
            get { return m_end; }
            set { m_end = value; }
        }
    }
}
