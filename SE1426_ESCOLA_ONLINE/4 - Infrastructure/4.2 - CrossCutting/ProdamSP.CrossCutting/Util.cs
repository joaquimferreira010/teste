using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ProdamSP.CrossCutting
{
    public static class Util
    {
        public static string GetOnlyNumbers(string number)
        {
            Regex matchOnlyDigits = new Regex(@"\W");
            string onlyNumbers = matchOnlyDigits.Replace(number, "");
            return onlyNumbers;
        }

        public static long GetOnlyNumbersToLng(string number)
        {
            long number1 = 0;
            long.TryParse(GetOnlyNumbers(number), out number1);

            return number1;
        }

        public static long GetOnlyNumbersToInt(string number)
        {
            int number1 = 0;
            int.TryParse(GetOnlyNumbers(number), out number1);

            return number1;
        }

    }
}
