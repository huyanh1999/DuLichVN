using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;

namespace DC.Common.Utility
{
    public static class StringTools
    {
        #region #Specials
        /// <summary>
        /// Chuyển giá sản phẩm
        /// </summary>
        /// <param name="listPrice"></param>
        /// <param name="regularPrice"></param>
        /// <returns></returns>
        public static string ToStrings(decimal? listPrice, decimal? regularPrice)
        {
            if (listPrice.HasValue || regularPrice.HasValue)
            {
                if (regularPrice != null && regularPrice.HasValue && regularPrice.Value > 0)
                    return ConvertToString(regularPrice);
                else
                    return ConvertToString(listPrice);
            }

            return string.Empty;
        }

        /// <summary>
        /// Chuyển giá sản phẩm thành tỉ lệ phần trăm giảm giá
        /// </summary>
        /// <param name="listPrice"></param>
        /// <param name="regularPrice"></param>
        /// <returns></returns>
        public static string SaleoffToStrings(decimal? listPrice, decimal? regularPrice)
        {
            if (listPrice.HasValue && regularPrice.HasValue && listPrice.Value > regularPrice.Value)
            {
                decimal? saleOff = listPrice - regularPrice;
                return string.Format("-{0} OFF", (saleOff / listPrice).Value.ToString("0%"));
            }

            return string.Empty;
        }
        #endregion

        public static string ConvertToString(DateTime? fromDate, DateTime? toDate)
        {
            string value = string.Empty;

            if (toDate == null)
                toDate = DateTime.Now;

            // Get year
            if (toDate > fromDate)
            {
                int months = NumberTools.GetMonthsBetween(fromDate.Value, toDate.Value);
                int years = months / 12;

                if (years > 0)
                {
                    value = string.Format("{0} năm", years);
                    months = months - (years * 12);

                    if (months > 0)
                        value += string.Format(", {0} tháng", months);
                }
                else if (months > 0)
                    value = string.Format("{0} tháng", months);
            }
            

            return value;
        }


        public static string ConvertToString(DateTime? dt)
        {
            if (dt == null)
                return string.Empty;
            else
            {
                return Convert.ToDateTime(dt.Value).ToString("dd/MM/yyyy");
            }
        }


        public static string ConvertToFullString(DateTime? dt)
        {
            if (dt == null)
                return string.Empty;
            else
            {
                string value = Convert.ToDateTime(dt.Value).ToString("dd/MM/yyyy HH:mm");
                return value.Replace("AM", "SA").Replace("PM", "CH");
            }
        }


        public static string ConvertToString(Guid? guid)
        {
            if (!guid.HasValue)
                return string.Empty;
            else
                return guid.Value.ToString();
        }
        

        public static string ConvertToString(decimal? value)
        {
            if (value == null || value.Value <= 0)
                return string.Empty;

            string temp = string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:c}", value);
            temp = temp.Replace(",00", string.Empty);
            temp = temp.Replace("₫", "");
            return temp;
        }
        

        public static string IntToString(int? value)
        {
            if (value == null || value.Value < 0)
                return string.Empty;

            string temp = string.Format(new System.Globalization.CultureInfo("vi-VN"), "{0:c}", value);
            temp = temp.Replace(",00", string.Empty);
            temp = temp.Replace("₫", "");
            return temp;
        }


        public static string MergeStrings(string param1, string param2)
        {
            string value = string.Empty;

            if (!string.IsNullOrEmpty(param1) && !string.IsNullOrEmpty(param2))
                value = string.Format("{0}; {1}", param1, param2);
            else if (!string.IsNullOrEmpty(param1))
                value = string.Format("{0}", param1);
            else if (!string.IsNullOrEmpty(param2))
                value = string.Format("{0}", param2);

            return value;
        }


        public static string GenerateSeparator(int num)
        {
            if (num <= 1)
                return string.Empty;

            string value = string.Empty;
            for (int i = 1; i < num; i++)
            {
                value += "--";
            }

            return value;
        }


        public static string MakeSeparator(int num)
        {
            if (num <= 1)
                return string.Empty;

            string value = string.Empty;
            for (int i = 1; i < num; i++)
            {
                value += "--";
            }

            return value;
        }

        public static string GetBetweenStrings(string param1, string param2)
        {
            string value = string.Empty;

            if (!string.IsNullOrEmpty(param1))
                value = string.Format("{0}", param1);
            else if (!string.IsNullOrEmpty(param2))
                value = string.Format("{0}", param2);

            return value;
        }





        /// <summary>
        /// Remove all html tags exit on string value
        /// </summary>
        /// <param name="stringSummary"></param>
        /// <returns></returns>
        public static string RemoveHtml(string stringSummary)
        {
            string Summary = "";
            string[] arraySummary = stringSummary.Split(new Char[] { '<' });
            foreach (string s in arraySummary)
            {
                string htmlString = s.Substring(0, s.IndexOf(">") + CountChars(">"));
                if (string.IsNullOrEmpty(htmlString))
                {
                    Summary += " " + s.Trim();
                }
                else
                {
                    Summary += " " + s.Replace(htmlString, string.Empty).Trim();
                }
            }
            return Summary.Replace("\n", string.Empty);
        }

        /// <summary>
        /// Remove all html tag not allow on string value
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveHtmlTags(string html)
        {
            if (html == null || html == string.Empty)
                return string.Empty;

            html = Regex.Replace(html, "<title.*?</title>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "<object.*?</object>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "<script.*?</script>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "<style.*?</style>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "<w.*?</w.*?>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("<[^>]*>");
            html = rx.Replace(html, string.Empty);

            return html;
        }



        /// <summary>
        /// Remove all script and html tag on string value
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveScriptHtmlTags(string html)
        {
            if (html == null || html == string.Empty)
                return string.Empty;
            html = Regex.Replace(html, "<title.*?</title>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "<object.*?</object>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "<script.*?</script>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "<style.*?</style>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "<w.*?</w.*?>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            return html;
        }



        /// <summary>
        /// Remove all tag can be used to apply injection technology
        /// </summary>
        /// <param name="stringinput"></param>
        /// <returns></returns>
        public static string RemoveScriptTags(object stringinput)
        {
            string stringoutput;
            stringoutput = stringinput.ToString().Replace("'", "&#146;");
            stringoutput = stringinput.ToString().Replace("script", "&#115;cript");
            stringoutput = stringinput.ToString().Replace("SCRIPT", "&#083;CRIPT");
            stringoutput = stringinput.ToString().Replace("Script", "&#083;cript");
            stringoutput = stringinput.ToString().Replace("script", "&#083;cript");
            stringoutput = stringinput.ToString().Replace("object", "&#111;bject");
            stringoutput = stringinput.ToString().Replace("OBJECT", "&#079;BJECT");
            stringoutput = stringinput.ToString().Replace("Object", "&#079;bject");
            stringoutput = stringinput.ToString().Replace("object", "&#079;bject");
            stringoutput = stringinput.ToString().Replace("applet", "&#097;pplet");
            stringoutput = stringinput.ToString().Replace("APPLET", "&#065;PPLET");
            stringoutput = stringinput.ToString().Replace("Applet", "&#065;pplet");
            stringoutput = stringinput.ToString().Replace("applet", "&#065;pplet");
            stringoutput = stringinput.ToString().Replace("event", "&#101;vent");
            stringoutput = stringinput.ToString().Replace("EVENT", "&#069;VENT");
            stringoutput = stringinput.ToString().Replace("Event", "&#069;vent");
            stringoutput = stringinput.ToString().Replace("event", "&#069;vent");
            stringoutput = stringinput.ToString().Replace("document", "&#100;ocument");
            stringoutput = stringinput.ToString().Replace("DOCUMENT", "&#068;OCUMENT");
            stringoutput = stringinput.ToString().Replace("Document", "&#068;ocument");
            stringoutput = stringinput.ToString().Replace("document", "&#068;ocument");
            stringoutput = stringinput.ToString().Replace("cookie", "&#099;ookie");
            stringoutput = stringinput.ToString().Replace("COOKIE", "&#067;OOKIE");
            stringoutput = stringinput.ToString().Replace("Cookie", "&#067;ookie");
            stringoutput = stringinput.ToString().Replace("cookie", "&#067;ookie");
            stringoutput = stringinput.ToString().Replace("form", "&#102;orm");
            stringoutput = stringinput.ToString().Replace("FORM", "&#070;ORM");
            stringoutput = stringinput.ToString().Replace("Form", "&#070;orm");
            stringoutput = stringinput.ToString().Replace("form", "&#070;orm");
            stringoutput = stringinput.ToString().Replace("iframe", "i&#102;rame");
            stringoutput = stringinput.ToString().Replace("IFRAME", "I&#070;RAME");
            stringoutput = stringinput.ToString().Replace("Iframe", "I&#102;rame");
            stringoutput = stringinput.ToString().Replace("iframe", "i&#102;rame");
            stringoutput = stringinput.ToString().Replace("textarea", "&#116;extarea");
            stringoutput = stringinput.ToString().Replace("TEXTAREA", "&#84;EXTAREA");
            stringoutput = stringinput.ToString().Replace("Textarea", "&#84;extarea");
            stringoutput = stringinput.ToString().Replace("textarea", "&#84;extarea");
            stringoutput = stringinput.ToString().Replace("input", "&#073;nput");
            stringoutput = stringinput.ToString().Replace("Input", "&#073;nput");
            stringoutput = stringinput.ToString().Replace("INPUT", "&#073;nput");
            stringoutput = stringinput.ToString().Replace("input", "&#073;nput");
            stringoutput = stringinput.ToString().Replace("<", "&lt;");
            stringoutput = stringinput.ToString().Replace(">", "&gt;");
            stringoutput = stringinput.ToString().Replace("'", "''");
            stringoutput = stringinput.ToString().Replace("=", "&#061;");
            stringoutput = stringinput.ToString().Replace("select", "sel&#101;ct");
            stringoutput = stringinput.ToString().Replace("join", "jo&#105;n");
            stringoutput = stringinput.ToString().Replace("union", "un&#105;on");
            stringoutput = stringinput.ToString().Replace("where", "wh&#101;re");
            stringoutput = stringinput.ToString().Replace("insert", "ins&#101;rt");
            stringoutput = stringinput.ToString().Replace("delete", "del&#101;te");
            stringoutput = stringinput.ToString().Replace("update", "up&#100;ate");
            stringoutput = stringinput.ToString().Replace("like", "lik&#101;");
            stringoutput = stringinput.ToString().Replace("drop", "dro&#112;");
            stringoutput = stringinput.ToString().Replace("create", "cr&#101;ate");
            stringoutput = stringinput.ToString().Replace("modify", "mod&#105;fy");
            stringoutput = stringinput.ToString().Replace("rename", "ren&#097;me");
            stringoutput = stringinput.ToString().Replace("alter", "alt&#101;r");
            stringoutput = stringinput.ToString().Replace("cast", "ca&#115;t");

            return stringoutput;
        }




        /// <summary>
        /// Get number words of string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static int CountChars(string value)
        {
            int result = 0;
            bool lastWasSpace = false;

            foreach (char c in value)
            {
                if (char.IsWhiteSpace(c))
                {
                    if (lastWasSpace == false)
                    {
                        result++;
                    }
                    lastWasSpace = true;
                }
                else
                {
                    result++;
                    lastWasSpace = false;
                }
            }

            return result;
        }


        /// <summary>
        /// Remove HTML from string with Regex.
        /// </summary>
        public static string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        /// <summary>
        /// Compiled regular expression for performance.
        /// </summary>
        static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

        /// <summary>
        /// Remove HTML from string with compiled Regex.
        /// </summary>
        public static string StripTagsRegexCompiled(string source)
        {
            return _htmlRegex.Replace(source, string.Empty);
        }

        /// <summary>
        /// Remove HTML tags from string using char array.
        /// </summary>
        public static string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }



        /// <summary>
        /// Clear unicode support with language=vi-VN to ASCII
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static string ClearSpecials(string Name)
        {
            string strValue = string.Empty;

            // Convert a string to utf-8 bytes.
            if (!string.IsNullOrEmpty(Name))
            {
                string str = HttpUtility.HtmlDecode(Name);
                strValue = str.ToLower().Trim();
                // - Special with char 'A'
                strValue = strValue.Replace("à", "a").Replace("á", "a").Replace("ạ", "a").Replace("ã", "a").Replace("ả", "a");
                strValue = strValue.Replace("ắ", "a").Replace("ằ", "a").Replace("ặ", "a").Replace("ẵ", "a").Replace("ẳ", "a").Replace("ă", "a");
                strValue = strValue.Replace("ấ", "a").Replace("ầ", "a").Replace("ậ", "a").Replace("ẫ", "a").Replace("ẩ", "a").Replace("â", "a");

                // - Special with char 'D'
                strValue = strValue.Replace("đ", "d");

                // - Special with char 'E'
                strValue = strValue.Replace("è", "e").Replace("é", "e").Replace("ẹ", "e").Replace("ẽ", "e").Replace("ẻ", "e");
                strValue = strValue.Replace("ề", "e").Replace("ế", "e").Replace("ệ", "e").Replace("ễ", "e").Replace("ể", "e").Replace("ê", "e");

                // - Special with char 'O'
                strValue = strValue.Replace("ò", "o").Replace("ó", "o").Replace("ọ", "o").Replace("õ", "o").Replace("ỏ", "o");
                strValue = strValue.Replace("ồ", "o").Replace("ố", "o").Replace("ộ", "o").Replace("ỗ", "o").Replace("ổ", "o").Replace("ô", "o");
                strValue = strValue.Replace("ờ", "o").Replace("ớ", "o").Replace("ợ", "o").Replace("ỡ", "o").Replace("ở", "o").Replace("ơ", "o");

                // - Special with char 'U'
                strValue = strValue.Replace("ù", "u").Replace("ú", "u").Replace("ụ", "u").Replace("ũ", "u").Replace("ủ", "u");
                strValue = strValue.Replace("ừ", "u").Replace("ứ", "u").Replace("ự", "u").Replace("ữ", "u").Replace("ử", "u").Replace("ư", "u");

                // - Special with char 'i'
                strValue = strValue.Replace("í", "i").Replace("ì", "i").Replace("ị", "i").Replace("ĩ", "i").Replace("ỉ", "i");

                // - Special with char 'y');
                strValue = strValue.Replace("ỳ", "y").Replace("ý", "y").Replace("ỵ", "y").Replace("ỹ", "y").Replace("ỷ", "i");

                strValue = System.Text.RegularExpressions.Regex.Replace(strValue, "[^0-9a-zA-Z]+?", "-").Replace("--", "-");

            }
            // - Return values
            strValue = strValue.Replace("--", " ").Trim();
            return strValue.Replace(" ", "-");
        }


        public static string Cleanup(string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && value.IndexOf('(') > -1 && value.IndexOf(')') > -1)
            {
                value = value.Substring(value.IndexOf('(') + 1, value.IndexOf(')') - value.IndexOf('(') - 1);
                return value;
            }

            return string.Empty;
        }



        // - Encrypt password.
        /// <summary>
        /// Encrypt password wih SHA512 method
        /// </summary>
        /// <param name="pstrPwd">Password string to ecrypt</param>
        /// <returns>Encrypted password</returns>
        public static string Encryption(string pstrPwd)
        {
            string strHex = string.Empty;
            // A hash function works on a byte array, so we will create two arrays, 
            // one for our resulting hash and one for the given text.
            byte[] HashValue, MessageBytes = UnicodeEncoding.Default.GetBytes(pstrPwd);

            // Now we create an object that will hash our text:
            SHA512 sha512 = new SHA512Managed();

            // And finally we calculate the hash and convert it to a hexadecimal string. 
            // Which we can store in a database for example
            HashValue = sha512.ComputeHash(MessageBytes);
            foreach (byte b in HashValue)
                strHex += string.Format("{0:x2}", b);
            return strHex;
        }

        public static string RandomCode(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(36);
                if (temp != -1 && temp == t)
                {
                    return RandomCode(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }
    }
}
