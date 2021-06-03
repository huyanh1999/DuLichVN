using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace DC.Common.Helpers
{
    public class StringHelpers
    {
        #region
        public static byte[] EncryptData(string data)
        {
            var md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();

            var encoder = new System.Text.UTF8Encoding();

            return md5Hasher.ComputeHash(encoder.GetBytes(data));
        }
        public static string MD5Hash(string data)
        {
            return BitConverter.ToString(EncryptData(data)).Replace("-", "").ToLower();
        }
        #endregion
        #region GenerateKey

        public static string UniqueKey(int maxSize)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            var data = new byte[1];
            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            var result = new StringBuilder(maxSize);
            foreach (var b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            return result.ToString();
        }

        public static string GetExtensionFiles(string filename)
        {

            return filename.Substring(filename.LastIndexOf('.') + 1);
        }

        

        public static string GenerateOrderCode()
        {
            var st = new DateTime(1970, 1, 1);
            var t = (DateTime.Now.ToUniversalTime() - st);
            var timestamp = (Int64)(t.TotalMinutes);
            var key = UniqueKey(6);
            return string.Format("{0}_{1}", key, timestamp);
        }


       
        #endregion
        #region Random
        //Function to get random number
        private static readonly Random Random = new Random();
        private static readonly object SyncLock = new object();
        public static int RandomNumber(int min, int max)
        {
            lock (SyncLock)
            { // synchronize
                return Random.Next(min, max);
            }
        }

        #endregion
        #region Base 64
        public static string Base64Encode(string data)
        {
            try
            {
                var encDataByte = Encoding.UTF8.GetBytes(data);
                var encodedData = Convert.ToBase64String(encDataByte);
                return encodedData.Replace("+", "-").Replace("/", "_");
            }
            catch
            {
                return "";
            }
        }
        public static string Base64Decode(string data)
        {
            try
            {
                var encoder = new UTF8Encoding();
                var utf8Decode = encoder.GetDecoder();
                data = data.Replace("-", "+").Replace("_", "/");
                var todecodeByte = Convert.FromBase64String(data);
                var charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
                var decodedChar = new char[charCount];
                utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);
                var result = new String(decodedChar);
                return result;
            }
            catch
            {
                return "";
            }
        }
        #endregion


        #region Filter


        
        public static string RemoveSpecialCharacters(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            var r = new Regex("(?:[^a-z0-9 -]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            var output = r.Replace(input, String.Empty);
            return output;
        }


        public static string ParseUrlText(string intput)
        {
            intput = UnicodeUnSign(intput);
            intput = RemoveSpecialCharacters(intput);
            intput = intput.Replace(" ", "-");
            intput = intput.ToLower();
            return intput;
        }

        #endregion

        #region  Other

        public static string TimeAgo(DateTime? timeInput)
        {
            double min;
            return TimeAgo(timeInput, out min);
        }
        public static string TimeAgo(DateTime? timeInput, out double min)
        {
            var result = "";
            min = 0;
            if (timeInput.HasValue)
            {
                var startDate = DateTime.Now;
                var deltaMinutes = startDate.Subtract(timeInput.Value);

                var minutes = deltaMinutes.TotalMinutes;
                min = minutes;
                //int mi = (minutes);
                if (minutes < 1)
                {
                    result = "vài giây trước";
                }
                else if (minutes < 50)
                {
                    result = Math.Round(new decimal(minutes)) + " phút trước";
                }
                //else if (minutes < 90)
                //{
                //    result = "một giờ trước";
                //}
                else if (minutes < 1080)
                {
                    result = Math.Round(new decimal((minutes / 60))) + " giờ trước";
                }
                //else if (minutes < 1440)
                //{
                //    result = "một ngày trước";
                //}
                //else if (minutes < 2880)
                //{
                //    result = "about one day";
                //}
                else
                {
                    result = Math.Round(new decimal((minutes / 1440))) + " ngày trước";
                }
            }
            return result;

        }
        
        public static string FormatNumber(string input)
        {
            const string commaDelimiter = ".";
            //if(isNaN(input)) return ;
            const string pattern = "(-?[0-9]+)([0-9]{3})";
            var regex = new Regex(pattern);

            while (Regex.IsMatch(input, pattern))
            {
                input = regex.Replace(input, "$1" + commaDelimiter + "$2");
            }
            return input;
        }

        public static string UnicodeUnSign(string s)
        {
            const string uniChars = "àáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯ";
            const string koDauChars = "aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEDIIIOOOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYAADOOU";

            if (string.IsNullOrEmpty(s))
            {
                return s;
            }
            var retVal = String.Empty;
            for (var i = 0; i < s.Length; i++)
            {
                var pos = uniChars.IndexOf(s[i].ToString());
                if (pos >= 0)
                    retVal += koDauChars[pos];
                else
                    retVal += s[i];
            }
            return retVal;
        }


        public static string ExtractString(string source, int length)
        {
            if (string.IsNullOrEmpty(source))
                return "";
            source = source.Trim();
            var dest = String.Empty;
            if (length == 0 || source.Length == 0)
                return dest;
            if (source.Length < length)
                dest = source;
            else
            {
                var tmp = source.Substring(0, length);
                var nSub = tmp.Length - 1;
                while (tmp[nSub] != ' ')
                {
                    nSub--;
                    if (nSub == 0) break;
                }
                dest = tmp.Substring(0, nSub);
            }
            return dest;
        }

   

        public static string SearchKeyword(string input)
        {
            var output = UnicodeUnSign(input);
            var r = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            output = r.Replace(output, String.Empty);
            //output = RemoveSpecialCharacters(output);
            return Regex.Replace(output, @"\s+", " ");
        }

        public static string ParseTimeByDuration(int duration)
        {
            var strDuration = ((decimal)((duration / 60))).ToString().PadLeft(2, '0') + ":" + (duration % 60).ToString().PadLeft(2, '0');
            return strDuration;
        }

        public static string ParseFileSize(int filesize)
        {
            var strFileSize = (Math.Round(filesize / (1024.0 * 1024), 2)).ToString() + " MB";
            return strFileSize;
        }
        #endregion

        public static string ReplaceCharacter(string word)
        {
            word = word.Replace(" ", "-");
            word = word.Replace("*", "");
            word = word.Replace("|", "");
            word = word.Replace("!", "");
            word = word.Replace("$", "");
            word = word.Replace("%", "");
            word = word.Replace("&", "");
            word = word.Replace("#", "");
            word = word.Replace("?", "");
            word = word.Replace(":", "");
            word = word.Replace("：", "");
            word = word.Replace(";", "");
            word = word.Replace("/", "");
            word = word.Replace(".", "");
            word = word.Replace(",", "");
            word = word.Replace("~", "");
            word = word.Replace("`", "");
            word = word.Replace("(", "");
            word = word.Replace(")", "");
            word = word.Replace("{", "");
            word = word.Replace("}", "");
            word = word.Replace("[", "");
            word = word.Replace("]", "");
            word = word.Replace("'", "");
            word = word.Replace("\"", "");
            word = word.Replace("  ", "");
            word = Regex.Replace(word, "([^A-Za-z0-9-]+)", String.Empty);
            //word = HttpUtility.HtmlEncode(word);
            return word;
        }

        public static string ConvertToKD(string data)
        {
            if (string.IsNullOrEmpty(data)) return "";

            string xmau;
            string xketqua;
            xmau = "áàảãạâấấầẩẫậăắằẳẵặéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵđÁÀẢÃẠÂẤẤẦẨẪẬĂẮẰẲẴẶÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴĐ";
            xketqua = "aaaaaaaaaaaaaaaaaaeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyydAAAAAAAAAAAAAAAAAAEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYD";

            if (data == "") return "";
            var kq = "";
            for (var i = 0; i < data.Length; i++)
            {
                var j = xmau.IndexOf(data[i]);
                if (j == -1) kq += data[i];
                else kq += xketqua[j];
            }

            kq = ReplaceCharacter(kq);
            return kq;

        }

        public static string GetStringBetween(string Str, string Seq, string SeqEnd)
        {
            var Orgi = Str;
            try
            {
                Str = Str.ToLower();
                Seq = Seq.ToLower();
                SeqEnd = SeqEnd.ToLower();

                var i = Str.IndexOf(Seq);

                if (i < 0)
                    return "";

                i = i + Seq.Length;

                var j = Str.IndexOf(SeqEnd, i);
                int end;

                if (j > 0) end = j - i;
                else end = Str.Length - i;

                return Orgi.Substring(i, end);
            }
            catch (Exception)
            {
                return "";
            }
        }

        


    }
}
