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

        void ClearString(ref string inputString, string deleteString)
        {
            inputString = inputString.Replace(deleteString, "");
            inputString = inputString.Replace(",", "");
        }
        public int Parse(string[] s, int startPos)
        {
            string s1 = s[startPos];
            ClearString(ref s1, "				[\"buyoutPrice\"] = ");
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
            ClearString(ref s1, "				[\"stackSize\"] = ");
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
        public int Parse(string[] s, int startPos)
        {
            //"AnalyzeSortData - self - start",
            for (;;startPos++)
            {
                if (s[startPos].Contains("[\"itemName\"]"))
                {
                    string s1 = s[startPos];
                    s1 = s1.Replace("		[\"itemName\"] = \"", "");
                    s1 = s1.Replace("\",", "");
                    m_ItemName = s1;
                    spisok.GetTag(m_ItemName);
                    break;
                }
            }

            for (;;startPos++)
            {
                if (s[startPos].Contains("[\"scanData\"] = {"))
                    break;
            }
            startPos += 2;
            for (;;)
            {
                string str = s[startPos - 1];
                if (str.Contains("}"))
                    break;
                CAuctionLot lot = new CAuctionLot();
                startPos = lot.Parse(s, startPos);
                if (lot.GetBoyOutPrice() != 0.0) //без цены выкупа
                    m_Lots.Add(lot);
                startPos += 3;
            }
            return startPos;
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
