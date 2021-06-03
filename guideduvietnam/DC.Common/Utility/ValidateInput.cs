using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DC.Common.Utility
{
    public static class ValidateInput
    {
        /// <summary>
        /// Kiểm tra file dạng ảnh
        /// </summary>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        public static bool IsExecuteFiles(string fileExt)
        {
            if (!string.IsNullOrEmpty(fileExt) && (fileExt.Equals(".exe", StringComparison.OrdinalIgnoreCase)
                                                    || fileExt.Equals(".dll", StringComparison.OrdinalIgnoreCase)
                                                    || fileExt.Equals(".bat", StringComparison.OrdinalIgnoreCase) 
                                                    || fileExt.Equals(".msi", StringComparison.OrdinalIgnoreCase))
            )
                return true;

            return false;
        }


        /// <summary>
        /// Kiểm tra file dạng ảnh
        /// </summary>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        public static bool IsImageFile(string fileExt)
        {
            if (!string.IsNullOrEmpty(fileExt) && (fileExt.Equals(".gif", StringComparison.OrdinalIgnoreCase) || fileExt.Equals(".jpg", StringComparison.OrdinalIgnoreCase) || fileExt.Equals(".png", StringComparison.OrdinalIgnoreCase)))
                return true;

            return false;
        }

        public static bool IsMp3File(string fileExt)
        {
            if (!string.IsNullOrEmpty(fileExt) && (fileExt.Equals(".mp3", StringComparison.OrdinalIgnoreCase)))
                return true;

            return false;
        }

        /// <summary>
        /// Kiểm tra file dạng tài liệu được hệ thống hỗ trợ
        /// </summary>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        public static bool IsDocumentFile(string fileExt)
        {
            if (!string.IsNullOrEmpty(fileExt) && (fileExt.Equals(".doc", StringComparison.OrdinalIgnoreCase)
                                                    || fileExt.Equals(".docx", StringComparison.OrdinalIgnoreCase)
                                                    || fileExt.Equals(".xls", StringComparison.OrdinalIgnoreCase)
                                                    || fileExt.Equals(".xlsx", StringComparison.OrdinalIgnoreCase)
                                                    || fileExt.Equals(".pdf", StringComparison.OrdinalIgnoreCase)
                                                    || fileExt.Equals(".ppt", StringComparison.OrdinalIgnoreCase)
                                                    || fileExt.Equals(".zip", StringComparison.OrdinalIgnoreCase)
                                                    || fileExt.Equals(".txt", StringComparison.OrdinalIgnoreCase)))
                return true;

            return false;
        }


        
        /// <summary>
        /// Kiểm tra loại email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;

            return false;
        }
    }
}
