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
        public static void ClearString(ref string inputString, string deleteString)
        {
            inputString = inputString.Replace(deleteString, "");
            inputString = inputString.Replace(",", "");
        }

        public static void ClearStringText(ref string inputString, string deleteString)
        {
            int start_pos = inputString.IndexOf(deleteString);
            int count = deleteString.Length;
            if (inputString[start_pos+count] == '\"')
                count++;
            if (inputString[start_pos+count] == ']')
                count++;
            if (inputString[start_pos + count] == ' ')
                count++;
            if (inputString[start_pos + count] == '=')
                count++;
            if (inputString[start_pos + count] == ' ')
                count++;
            inputString = inputString.Remove(0, start_pos + count);
            inputString = inputString.Replace(",", "");
            inputString = inputString.Replace("\"", "");
        }

        public bool WhatNext(string[] text, int start_pos, string s1, string s2)
        {
            int p1 = FindNextRow(text, s1, start_pos);
            int p2 = FindNextRow(text, s2, start_pos);
            return p1 < p2;//когда true, значит 1ый текст ближе
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
    }

    
}
