using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOWLogAuctionatorParser.Core
{
    class CAuctionLot
    {
        private double m_BuyOutPrice;
        private double m_Price;
        private int m_StackSize;

        
        public int Parse(string[] s, int startPos)
        {
            startPos = Utils.FindNextRow(s, "buyoutPrice", startPos);
            string s1 = s[startPos];
            Utils.ClearStringText(ref s1,"buyoutPrice");
            m_BuyOutPrice = Convert.ToDouble(s1);
            m_BuyOutPrice /= (100 * 100);

            for (int i = 0; ; i++)
            {
                startPos++;
                s1 = s[startPos];
                if (s1.Contains("stackSize"))
                    break;
                if (i == 10)
                    System.Diagnostics.Debug.Assert(false);
            }
            s1 = s[startPos];
            Utils.ClearStringText(ref s1, "stackSize");
            m_StackSize = Convert.ToInt32(s1);

            m_Price = m_BuyOutPrice / m_StackSize;
            return startPos;
        }

        public int GetStactSize()
        {
            return m_StackSize;
        }
        public double GetPrice()
        {
            return m_Price;
        }
        public double GetBoyOutPrice()
        {
            return m_BuyOutPrice;
        }
    }

    class CAuctionItem
    {
        CAllProductSpisok spisok = new CAllProductSpisok();
        string m_ItemName;
        ProductTag m_Tag;
        List<CAuctionLot> m_Lots = new List<CAuctionLot>();

        public string GetName()
        {
            return m_ItemName;
        }
        public void Sort()
        {
            for (int i = 0; i<m_Lots.Count - 1; i++)
            {
                for (int j = 0;j < m_Lots.Count - 1;j++)
                {
                    if (m_Lots[j].GetPrice() > m_Lots[j+1].GetPrice())
                    {
                        CAuctionLot temp = m_Lots[j];
                        m_Lots[j] = m_Lots[j+1];
                        m_Lots[j+1] = temp;
                    }
                }
            }
        }
        public int Parse(string[] s, int pos)
        {
            //"AnalyzeSortData - self - start",

            pos = Utils.FindNextRow(s, "itemName", pos);
            string s1 = s[pos];
            Utils.ClearStringText(ref s1, "itemName");
            m_ItemName = s1;
            m_Tag = spisok.GetTag(m_ItemName);

            pos = Utils.FindNextRow(s, "scanData", pos);

            for (;;)
            {
                int pos_end = Utils.FindNextRow(s, "}", pos);
                int pos_next = Utils.FindNextRow(s, "buyoutPrice", pos);
                if (pos_next > pos_end)
                    break;
                CAuctionLot lot = new CAuctionLot();
                pos = lot.Parse(s, pos);
                pos = Utils.FindNextRow(s, "}", pos) + 1;
                if (lot.GetBoyOutPrice() != 0.0) //без цены выкупа
                    m_Lots.Add(lot);
            }
            return pos;
        }
        public double GetCost(int size)
        {
            double sumGold = 0.0;
            int sumItems = 0;
            for (int i = 0; i<m_Lots.Count; i++)
            {
                sumItems += m_Lots[i].GetStactSize();
                if (size == 1)
                    sumGold += m_Lots[i].GetPrice();
                else
                    sumGold += m_Lots[i].GetBoyOutPrice();
                if (sumItems >= size)
                    break;
            }
            if (size != 1)
                sumGold /= sumItems;
            return sumGold;
        }
    }


    class CAuctionAnalyzer
    {
        List<CAuctionItem> m_Items = new List<CAuctionItem>();
        public void Clear()
        {
            m_Items.Clear();
        }
        public void Parse(string [] s)
        {
            int Count = s.Length;
            for (int startPos = 0; startPos < Count; )
            {
                string str = s[startPos];
                if (str.Contains("AnalyzeSortData - self - start"))
                {
                    CAuctionItem item = new CAuctionItem();
                    startPos = item.Parse(s, startPos);
                    item.Sort();
                    m_Items.Add(item);
                }
                else
                    startPos++;
            }
        }

        public List<CAuctionItem> GetItems()
        {
            return m_Items;
        }
    }
}
