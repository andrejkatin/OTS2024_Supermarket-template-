using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace OTS_Supermarket.Test
{
    public class CartTxtParser
    {
        public static IEnumerable<TestCaseData> GetTestCasesData(string fileName)
        {
            string path = $@"{AppDomain.CurrentDomain.BaseDirectory}{fileName}";
            string[] lines = File.ReadAllLines(path);

            List<TestCaseData> testCases = new List<TestCaseData>();
            bool isFirstLine = true;
            
            foreach(string line in lines)
            {
                if (isFirstLine) { isFirstLine = false; continue; }
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                string[] chars = line.Split('\t');
                int days = Convert.ToInt32(chars[0]);
                bool isNotWeekend = bool.Parse(chars[1]);
                int amount = Convert.ToInt32(chars[2]);
                int size = Convert.ToInt32(chars[3]);
                int computer = Convert.ToInt32(chars[4]);
                int laptop = Convert.ToInt32(chars[5]);
                int monitor = Convert.ToInt32(chars[6]);
                int chair = Convert.ToInt32(chars[7]);
                int keyboard = Convert.ToInt32(chars[8]);
                double discount = Math.Round(Convert.ToDouble(chars[9]), 2);

                DateTime targetDate = GetStableTestDate(days, isNotWeekend);
                string stringDate = targetDate.ToString("yyyy-MM-dd");

                testCases.Add( new TestCaseData(size, amount, laptop, monitor, chair, computer, keyboard, stringDate, discount));
            }
            return testCases;
        }

        private static DateTime GetStableTestDate(int expectedDaysRange, bool mustBeWeekday)
        {
            DateTime today = DateTime.Today;
            int startRange = (expectedDaysRange <= 3) ? -10 : 4;  
            int endRange = (expectedDaysRange <= 3) ? 3 : 7;      
            
            for (int i = startRange; i <= endRange; i++)
            {
                if (i == 0) continue; // Cart Calculation baca Exception za 0
                
                DateTime checkDate = today.AddDays(i);
                bool isWeekday = (checkDate.DayOfWeek != DayOfWeek.Saturday && checkDate.DayOfWeek != DayOfWeek.Sunday);
                
                if (isWeekday == mustBeWeekday)
                    return checkDate;
            }
            
            return today.AddDays(expectedDaysRange);
        }
    }
}
