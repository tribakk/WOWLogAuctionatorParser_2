using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace WOWLogAuctionatorParser
{
    public partial class Form1 : Form
    {
        Core.CAuctionAnalyzer m_Analyzer = new Core.CAuctionAnalyzer();
        private static readonly Microsoft.Office.Interop.Excel.Application m_pApplication = new Microsoft.Office.Interop.Excel.Application();
        private Workbook m_pBook;
        Core.CAllProductSpisok spisok = new Core.CAllProductSpisok();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.IO.FileInfo info = new System.IO.FileInfo(FileNameBox.Text);
            Text = info.LastWriteTime.ToString();
            string[] textFromFile = System.IO.File.ReadAllLines(FileNameBox.Text);
            m_Analyzer.Parse(textFromFile);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = "C:\\Users\\tribak\\Desktop\\wow алхимия аукцион.xlsx";
            m_pBook = m_pApplication.Workbooks.Open(path);
            m_pApplication.Visible = true;
        }

        private Worksheet GetWorksheet(string sheetName, bool bThrow = true)
        {
            Worksheet pSheet = null;
            try
            {
                pSheet = (Worksheet)m_pBook.Worksheets.Item[sheetName];
            }
            catch
            {
                if (bThrow)
                    throw new ArgumentNullException("нет листа " + sheetName);
            }

            return pSheet;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Worksheet pSheet = GetWorksheet("таблица цен");

            int emptyColumn = 2;//А1 - пустая, и ее надо пропустить
            for (;;emptyColumn++)
            {
                Range pRange = pSheet.Cells[1, emptyColumn];
                string text = pRange.Text;
                if (text == "")
                    break;
            }
            pSheet.Cells[1, emptyColumn].Value = DateTime.Now;

            List<Core.CAuctionItem> items = m_Analyzer.GetItems();
            int count = items.Count;
            for (int i = 0; i<count; i++)
            {
                Core.CAuctionItem item = items[i];
                string name = item.GetName();
                int row = 1;
                bool bFind = false;
                for (; row < 30; row++) //ToDo: 20 !!!!!!!!!!!!!!!
                {
                    Range pRange = pSheet.Cells[row, 1];

                    
                    if (name.Equals(pRange.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        bFind = true;
                        break;
                    }
                }
                if (!bFind)
                    System.Diagnostics.Debug.Assert(false, name);
                int needSize = 300;
                if (name == spisok.GetName(Core.ProductTag.ptYakorTrava))
                    needSize = 100;
                if (name.Contains("Настой") || name.Contains("Боевое"))
                    needSize = 1;
                double Gold = item.GetCost(needSize);
                //Gold /= needSize;
                Range pRange1 = pSheet.Cells[row, emptyColumn];
                pRange1.Value = Gold;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                m_pBook.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                //throw;
            }
        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
