using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPosting.Code
{
    public static class ConvertStringToList
    {
        public static List<int> ConvertToInt(string Array)
        {
            int myint;
            List<int> numbers = Array.Split(',').Where(n => int.TryParse(n.ToString(), out myint)).Select(n => int.Parse(n.ToString())).ToList();
            return numbers;
        }


    }
}