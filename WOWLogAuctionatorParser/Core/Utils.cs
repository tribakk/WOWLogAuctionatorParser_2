using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace WOWLogAuctionatorParser.Core
{
    public class Utils
    {
        public static int FindNextRow(string[] string_array, string text, int start_row)
        {
            int count = string_array.Length;
            for (;start_row < count; start_row ++)
            {
                string str = string_array[start_row];
                if (str.Contains(text))
                    return start_row;
            }
            return 0;
        }

        public static Range GetRange(Worksheet pSheet, int startRow, int startColumn, int endRow, int endColumn)
        {
            Range pRange1 = pSheet.Cells[startRow, startColumn];
            Range pRange2 = pSheet.Cells[endRow, endColumn];
            return pSheet.Range[pRange1, pRange2];
        }
        public static void UpdateCell(Worksheet pSheet, int Row, int Column, string value, int weight = 5)
        {
            Range pRange = pSheet.Cells[Row, Column];
            pRange.Value = value;
            pRange.ColumnWidth = weight;
        }

        public static void WriteHeader(Worksheet pSheet, int row, int column)
        {
            UpdateCell(pSheet, row, column++, "Название", 20);
            UpdateCell(pSheet, row, column++, "Надо");
            UpdateCell(pSheet, row, column++, "Процесс");
            UpdateCell(pSheet, row, column++, "Готов");
            UpdateCell(pSheet, row, column++, "Осталось");
        }

        public static void WriteTagString(Worksheet pSheet, int row, int column, string name, int value)
        {
            UpdateCell(pSheet, row, column, name, 20);
            UpdateCell(pSheet, row, column + 1, value.ToString());
            UpdateCell(pSheet, row, column + 2, "");
            UpdateCell(pSheet, row, column + 3, "");
            UpdateCell(pSheet, row, column + 4, "");
            pSheet.Cells[row, column + 4].FormulaR1C1 = "=RC[-3]-RC[-2]-RC[-1]";
            //string formula = "=IF(RC[-1]=0,\"можно удалить\",\"\")";
            string formula = "=IF(AND(RC[-1]=0,RC[-3]=0),\"можно удалить\",\"\")";
            //string formula = "=SQRT(R1C1)";
            pSheet.Cells[row, column + 5].FormulaR1C1 = formula;

            Utils.DrawConditions(pSheet.Cells[row, column + 1], Color.CornflowerBlue);
            Utils.DrawConditions(pSheet.Cells[row, column + 2], Color.Khaki);
            Utils.DrawConditions(pSheet.Cells[row, column + 4], Color.DarkSeaGreen);
        }

        public static void DrawConditions(Range pRange, Color color)
        {
            FormatConditions pConditions = pRange.FormatConditions;
            Databar pCondition = pConditions.AddDatabar();
            pCondition.MaxPoint.Modify(XlConditionValueTypes.xlConditionValueNumber, 0.5);
            pCondition.MinPoint.Modify(XlConditionValueTypes.xlConditionValueNumber, 0);
            pCondition.BarColor.Color = color;
            pCondition.BarFillType = XlDataBarFillType.xlDataBarFillSolid;
        }
    }

    class CAllSourceWrite
    {
        CAllProductSpisok m_spisok = new CAllProductSpisok();

        public CAllSourceWrite(Worksheet pSheet, CSmartArray array)
        {
            Dictionary<ProductTag, int> map = array.GetMap();
            Dictionary<ProductTag, int>.KeyCollection.Enumerator e = map.Keys.GetEnumerator();
            const int startRow = 3;
            const int startColumn = 10;
            Utils.WriteHeader(pSheet, startRow, startColumn);

            int count = map.Keys.Count;
            for (int i = 0; i < count; i++)
            {
                e.MoveNext();
                ProductTag tag = e.Current;
                string name = m_spisok.GetName(tag);
                int value = map[tag];
                Utils.WriteTagString(pSheet, startRow + i, startColumn, name, value);
            }
        }
    }

    class CAllFactoryOneTable
    {
        CAllProductSpisok m_spisok = new CAllProductSpisok();
        public CAllFactoryOneTable(List<CFactory> factoryList, Worksheet pSheet)
        {
            int currentRow = 1;
            const int currentColumn = 1;
            for (int i = 0; i < factoryList.Count; i++)
            {
                currentRow += WriteFactory(pSheet, factoryList[i], currentRow, currentColumn);
            }
        }

        private int WriteFactory(Worksheet pSheet, CFactory pFactory, int row, int column)
        {
            List<ProductTag> input = pFactory.GetInputArray();
            int realCount = 0;
            string factoryName = pFactory.GetFactName();
            pSheet.Cells[row, column] = factoryName;
            Range pTitleRange = Utils.GetRange(pSheet, row, column, row, column + 4);
            pTitleRange.Merge();
            pTitleRange.Interior.Color = Color.Bisque;
            realCount++;
            for (int i = 0; i < input.Count; i++)
            {
                ProductTag tag = input[i];
                int value = pFactory.GetTagCount(tag);
                int currRow = row + realCount;
                string tagName = m_spisok.GetName(tag);
                
                if (value > 0)
                {
                    realCount++;
                    pSheet.Cells[currRow, column].ColumnWidth = 20;
                    Utils.WriteTagString(pSheet, currRow, column, tagName, value);
                }
            }

            return realCount;
        }
    }
    class CExcelInputHelper
    {
        Worksheet m_pSheet;
        Application m_pApp;
        CAllProductSpisok m_spisok = new CAllProductSpisok();
        public CExcelInputHelper(CSmartArray array)
        {
            //Application pApp = new Application();
            m_pApp = new Application();
            Workbook pBook = m_pApp.Workbooks.Add();
            m_pSheet = pBook.Sheets[1];

            m_pSheet.Cells[1, 1] = "наименование";
            m_pSheet.Cells[1, 2] = "количество";

            //int count = array.GetCount();
            int count = m_spisok.GetCount();
            for (int i = 0; i < count; i++) 
            {
                ProductTag tag = (ProductTag)(i + 1);
                m_pSheet.Cells[i + 2, 1] = m_spisok.GetName(tag);
                m_pSheet.Cells[i + 2, 2] = array.GetTagCount(tag);
            }
            Range pRange = m_pSheet.Range[m_pSheet.Cells[1, 1], m_pSheet.Cells[count + 1, 2]];
            //Range pColumn = m_pSheet.Columns["A:FF"];
            //pColumn.AutoFit();
            for (int i = (int)XlBordersIndex.xlEdgeLeft; i <= (int)XlBordersIndex.xlInsideHorizontal; i++) 
            {
                pRange.Borders[(XlBordersIndex)i].LineStyle = XlLineStyle.xlContinuous;
            }
            m_pApp.Visible = true;

        }
        public CSmartArray GetArray()
        {
            CSmartArray array = new CSmartArray();
            int count = m_spisok.GetCount();
            for (int i = 1; i < count;i++)
            {
                string name = m_pSheet.Cells[i + 1, 1].Text;
                string value = m_pSheet.Cells[i + 1, 2].Text;
                array.Add(name, Convert.ToInt32(value));
            }
            return array;
        }
        ~CExcelInputHelper()
        {
            m_pApp.Workbooks.Close();
        }
    }
}
