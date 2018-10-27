using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOWLogAuctionatorParser.Core
{
    //public enum FactoryType
    //{
    //    ftField = 0,
    //    ftMilk = 1,
    //    ftBakery = 2,
    //    ftWeaving = 3,
    //    ftSugar = 4,
    //    ftRubber = 5,
    //    ftPlastic = 6,
    //    ftPaper = 7,
    //    ftSewing = 8,
    //    ftSnack = 9,
    //    ftFastFood = 10,
    //    ftIceCream = 11,
    //    ftConfectionery = 12,
    //    ftAnimal = 13,
    //    ftKorm = 14,
    //    ftJam = 15,
    //    ftCandy = 16,
    //    ftIsland = 17,
    //    ftFuture = 18,
    //    ftMexican = 19
    //}
    public class CFactory
    {
        protected CAllProductSpisok spisok = new CAllProductSpisok();
        protected List<ProductTag> m_TagInputArray = new List<ProductTag>();
        protected List<int> m_InputCount;
        protected CSmartArray m_SmartInput;
        protected CSmartArray m_Result;
        protected PriorityTag m_Priority;

        public CFactory()
        {
        }

        public void Refresh()
        {
            m_InputCount = new List<int>();
            m_SmartInput = new CSmartArray();
            m_Result = new CSmartArray();

            int count = m_TagInputArray.Count();
            for (int i = 0; i < count; i++)
            {
                m_InputCount.Add(0);
            }
        }

        protected void Init()
        {
            Refresh();
        }

        virtual public void Calc()
        {
        }
        virtual public string GetFactName()
        {
            return "";
        }
    	virtual public void SetInput(CSmartArray Array)
        {
            int count = m_TagInputArray.Count;
            for (int i = 0; i < count; i++)
            {
                ProductTag tag = m_TagInputArray[i];
                int ProductCount = Array.GetTagCount(tag);
                m_InputCount[i] = ProductCount;
                m_SmartInput.Add(tag, ProductCount);
                Array.ClearTag(tag);
            }
        }

        public void ExcludeWhatHave(CFactory pFactory)
        {
            m_SmartInput.ExcludeWhatHave(pFactory.GetSmartInput().Copy());
        }
        public PriorityTag GetPriorety()
        {
            return m_Priority;
        }

        public void UpdateResult(CSmartArray arr)
        {
            if (arr == null)
                return;
            arr.Merge(m_Result);
            m_Result.RemoveAll();
        }
        public List<ProductTag> GetInputArray()
        {
            return m_TagInputArray;
        }

        public int GetTagCount(ProductTag tag)
        {
            return m_SmartInput.GetTagCount(tag);
        }

        public CSmartArray GetSmartInput()
        {
            return m_SmartInput;
        }

        public bool IsEmpty()
        {
            return m_SmartInput.IsEmpty();
        }


        void AddResult(String name, int count)
        {
            m_Result.Add(spisok.GetTag(name), count);
        }
        public CSmartArray GetResult()
        {
            return m_Result;
        }
        
        public String Print()
        {
            if (m_SmartInput.IsEmpty())
                return "";
            //String pr;
            String result = Environment.NewLine + GetFactName() + Environment.NewLine;
            result += m_SmartInput.Print();
            return result;
        }
    };

    public class CAlhimiyaFactory : CFactory
    {
        public CAlhimiyaFactory()
        {
            m_TagInputArray.Add(spisok.GetTag(("Боевое зелье выносливости")));
            m_TagInputArray.Add(spisok.GetTag(("Боевое зелье интеллекта")));
            m_TagInputArray.Add(spisok.GetTag(("Боевое зелье ловкости")));
            m_TagInputArray.Add(spisok.GetTag(("Боевое зелье силы")));

            m_TagInputArray.Add(spisok.GetTag(("Настой бездонных глубин")));
            m_TagInputArray.Add(spisok.GetTag(("Настой бескрайнего горизонта")));
            m_TagInputArray.Add(spisok.GetTag(("Настой силы прибоя")));
            m_TagInputArray.Add(spisok.GetTag(("Настой стремительных течений")));
            Init();
            m_Priority = PriorityTag.one;
        }
        public override String GetFactName() 
        {
            return ("Алхимия");
        }
        public override void Calc()
        {

            int count = 0;
            int counter = 0;
            
            {
                //Боевое зелье выносливости
                count = m_InputCount[counter];
                m_Result.Add(spisok.GetTag(("Морской стебель")), 10 * count);
                m_Result.Add(spisok.GetTag(("Звездный мох")), 8 * count);
                counter++;
            }

            {
                //Боевое зелье интелекта
                count = m_InputCount[counter];
                m_Result.Add(spisok.GetTag(("Пыльца сирены")), 10 * count);
                m_Result.Add(spisok.GetTag(("Речной горох")), 8 * count);
                counter++;
            }

            {
                //Боевое зелье ловкости
                count = m_InputCount[counter];
                m_Result.Add(spisok.GetTag(("Пыльца сирена")), 8 * count);
                m_Result.Add(spisok.GetTag(("Речной горох")), 10 * count);
                counter++;
            }

            {
                //Боевое зелье силы
                count = m_InputCount[counter];
                m_Result.Add(spisok.GetTag(("Морской стебель")), 10 * count);
                m_Result.Add(spisok.GetTag(("Звездный мох")), 8 * count);
                counter++;
            }

            {
                //Настой бездонных глубин
                count = m_InputCount[counter];
                m_Result.Add(spisok.GetTag(("Якорь-трава")), 5 * count);
                m_Result.Add(spisok.GetTag(("Поцелуй зимы")), 10 * count);
                m_Result.Add(spisok.GetTag(("Речной горох")), 15 * count);
                counter++;
            }

            {
                //Настой бескрайнего горизонта
                count = m_InputCount[counter];
                m_Result.Add(spisok.GetTag(("Якорь-трава")), 5 * count);
                m_Result.Add(spisok.GetTag(("Поцелуй зимы")), 10 * count);
                m_Result.Add(spisok.GetTag(("Звездный мох")), 15 * count);
                counter++;
            }

            {
                //Настой силы прибоя
                count = m_InputCount[counter];
                m_Result.Add(spisok.GetTag(("Якорь-трава")), 5 * count);
                m_Result.Add(spisok.GetTag(("Укус Акунды")), 10 * count);
                m_Result.Add(spisok.GetTag(("Пыльца сирены")), 15 * count);
                counter++;
            }

            {
                //Настой бездонных глубин
                count = m_InputCount[counter];
                m_Result.Add(spisok.GetTag(("Якорь-трава")), 5 * count);
                m_Result.Add(spisok.GetTag(("Укус Акунды")), 10 * count);
                m_Result.Add(spisok.GetTag(("Морской стебель")), 15 * count);
                counter++;
            }
        }

    };

    
}
