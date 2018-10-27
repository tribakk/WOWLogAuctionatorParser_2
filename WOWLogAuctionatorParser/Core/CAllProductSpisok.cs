using System;
using System.Collections.Generic;
using System.Linq;

namespace WOWLogAuctionatorParser.Core
{
    public class CAllProductSpisok
    {
        List<ProductTagString> m_Array = new List<ProductTagString>();
        public String GetName(ProductTag tag)
        {
            String name = "";
            int count = m_Array.Count;
            for (int i = 0; i < count; i++)
            {
                if (m_Array[i].m_Tag == tag)
                {
                    name = m_Array[i].m_Name;
                    break;
                }
            }
            return name;
        }
        public ProductTag GetTag(String Name)
        {
            ProductTag tag = ProductTag.ptNotFound;
            if (Name == null || Name.Length == 0)
                return tag;
            int count = m_Array.Count();
            for (int i = 0; i < count; i++)
            {
                //if (i<10 && false) 
                if (m_Array[i].m_Name == Name)
                {
                    tag = m_Array[i].m_Tag;
                    break;
                }
            }
            if (Name != "" && tag == ProductTag.ptNotFound)
            {
                System.Diagnostics.Debug.Assert(false, Name);
                //OutputDebugString(("BIG ERROR: ") + Name + ("\n\r\r"));
            }

            return tag;
        }

        public int GetCount()
        {
            return m_Array.Count;
        }
        public ProductTagString GetProductTagString(int i)
        {
            return m_Array[i];
        }

        public CAllProductSpisok()
        {
            m_Array.Add(new ProductTagString(ProductTag.ptZvezdniyMoch, "Звездный мох"));
            m_Array.Add(new ProductTagString(ProductTag.ptMorskoyStebel, "Морской стебель"));
            m_Array.Add(new ProductTagString(ProductTag.ptPozeluyZimi, "Поцелуй зимы"));
            m_Array.Add(new ProductTagString(ProductTag.ptPilzaSireni, "Пыльца сирены"));
            m_Array.Add(new ProductTagString(ProductTag.ptRechnoyGoroh, "Речной горох"));
            m_Array.Add(new ProductTagString(ProductTag.ptUkusAkundi, "Укус Акунды"));
            m_Array.Add(new ProductTagString(ProductTag.ptYakorTrava, "Якорь-трава"));

            m_Array.Add(new ProductTagString(ProductTag.ptZvezdniyMoch, "Боевое зелье выносливости"));
            m_Array.Add(new ProductTagString(ProductTag.ptZvezdniyMoch, "Боевое зелье интеллекта"));
            m_Array.Add(new ProductTagString(ProductTag.ptZvezdniyMoch, "Боевое зелье ловкости"));
            m_Array.Add(new ProductTagString(ProductTag.ptZvezdniyMoch, "Боевое зелье силы"));

            m_Array.Add(new ProductTagString(ProductTag.ptZvezdniyMoch, "Настой бездонных глубин"));
            m_Array.Add(new ProductTagString(ProductTag.ptZvezdniyMoch, "Настой бескрайнего горизонта"));
            m_Array.Add(new ProductTagString(ProductTag.ptZvezdniyMoch, "Настой силы прибоя"));
            m_Array.Add(new ProductTagString(ProductTag.ptZvezdniyMoch, "Настой стремительных течений"));
        }
    };
}