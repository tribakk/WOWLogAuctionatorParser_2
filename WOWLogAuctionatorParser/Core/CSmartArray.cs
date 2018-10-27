using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOWLogAuctionatorParser.Core
{
    public class CSmartArray
    {
        CAllProductSpisok spisok =  new CAllProductSpisok();
        Dictionary<ProductTag, int> m_TagMap = new Dictionary<ProductTag, int>();
	    public void Add(ProductTag tag, int count)
        {
            if (tag == ProductTag.ptNotFound)
                return;
            if (count == 0)
                return;
            if (m_TagMap.ContainsKey(tag))
                m_TagMap[tag] += count;
            else
                m_TagMap[tag] = count;
        }
        public Dictionary<ProductTag, int> GetMap()
        {
            return m_TagMap;
        }
        public CSmartArray Copy()
        {
            CSmartArray array = new CSmartArray();
            var e = m_TagMap.Keys.GetEnumerator();
            for (int i = 0; i<m_TagMap.Keys.Count; i++)
            {
                e.MoveNext();
                ProductTag tag = e.Current;
                array.Add(tag, m_TagMap[tag]);
            }
            return array;
        }

        public void Add(String name, int count)
        {
            ProductTag tag = spisok.GetTag(name);
            Add(tag, count);
        }
        public void Merge(CSmartArray Arr)
        {
            var e = Arr.m_TagMap.Keys.GetEnumerator();
            e.MoveNext();
            for (int i = 0; i< Arr.m_TagMap.Keys.Count; i++)
            {
                ProductTag pTag = e.Current;
                Add(pTag, Arr.m_TagMap[pTag]);
                e.MoveNext();
            }
            e.Dispose();
        }

        public void ClearTag(ProductTag tag)
        {
            m_TagMap.Remove(tag);
        }

        public int GetTagCount(ProductTag tag)
        {
            int count = 0;
            if (m_TagMap.ContainsKey(tag))
                count = m_TagMap[tag];
            return count;
        }

        public int GetTagCount(string name)
        {
            ProductTag tag = spisok.GetTag(name);
            int count = 0;
            if (m_TagMap.ContainsKey(tag))
                count = m_TagMap[tag];
            return count;
        }
        public void RemoveAll()
        {
            m_TagMap.Clear();
        }
        public String Print()
        {
            string result = "";
            var e = m_TagMap.Keys.GetEnumerator();
            e.MoveNext();
            for (int i = 0; i < m_TagMap.Keys.Count; i++)
            {
                ProductTag pTag = e.Current;
                int value = m_TagMap[pTag];
                result += spisok.GetName(pTag) + " : " + value.ToString() + Environment.NewLine;
                e.MoveNext();
            }
            return result;
        }
        public bool IsEmpty()
        {
            return m_TagMap.Count == 0;
        }
        public int GetCount()
        {
            return m_TagMap.Count;
        }
        public void FillArray(List<ProductTag> PTlist, List<int> iList)
        {
            var e = m_TagMap.Keys.GetEnumerator();
            e.MoveNext();
            for (int i = 0; i < m_TagMap.Keys.Count; i++)
            {
                ProductTag tag = e.Current;
                int value = m_TagMap[tag];
                PTlist.Add(tag);
                iList.Add(value);
                e.MoveNext();
            }
        }

        public void ExcludeWhatHave(CSmartArray Arr)
        {
            var e = m_TagMap.Keys.GetEnumerator();
            List<ProductTag> sameTag = new List<ProductTag>();
            for (int i = 0; i < m_TagMap.Keys.Count; i++)
            {
                e.MoveNext();
                ProductTag tag = e.Current;
                int count = Arr.GetTagCount(tag);
                if (count != 0)
                    sameTag.Add(tag);
            }
            for (int i = 0; i<sameTag.Count;i++)
            {
                ProductTag tag = sameTag[i];
                int count = Arr.GetTagCount(tag);
                int value = m_TagMap[tag];

                if (value > count)
                {
                    Add(tag, -count);
                    Arr.ClearTag(tag);
                }
                else if (value < count)
                {
                    ClearTag(tag);
                    Arr.Add(tag, -value);
                }
                else //одинаковое значение
                {
                    ClearTag(tag);
                    Arr.ClearTag(tag);
                }
            }
                //e.MoveNext();
        }
        public bool IsEqual(CSmartArray array)
        {
            Dictionary<ProductTag, int> map = array.GetMap();
            int count = m_TagMap.Count;
            if (count != map.Count)
                return false;
            
            var en = m_TagMap.Keys.GetEnumerator();
            en.MoveNext();
            for (int i = 0; i < count; i++)
            {
                ProductTag pTag = en.Current;
                int TagCount = array.GetTagCount(pTag);
                if (TagCount == 0)
                    return false;
                if (TagCount != GetTagCount(pTag))
                    return false;
            }
            return true;
        }
    };
}
