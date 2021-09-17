using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPortal.Helpers
{
    public class Validation
    {
        private static long l;
        private static char[] SpecialChars = new char[] { '-', ' ', '(', ')' };

        public static bool ValidateRecipient(string Number, out string validNum)
        {
            Number = Number.Trim();
            validNum = Number;
            try
            {
                if (Number.Length > 14) return false;

                if (Number.IndexOfAny(SpecialChars) > -1)
                    Number = Number.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");

                string str = Number.Substring(Number.IndexOf('3'));

                if (str.Length != 10 || !long.TryParse(str, out l)) return false;

                Number = "923" + Number.Substring(Number.Length - 9);
                validNum = Number;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}