using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DC.Common.Utility;
using DC.Webs.Models;

namespace DC.Webs.Areas.Admin.Controllers
{
    public class AttachmentsController : Controller
    {
        public static string ProductImageUrl = ConfigurationManager.AppSettings["ProductImageUrl"].ToString();



        // GET: Admin/Attachments
        public ActionResult Index()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        public ActionResult AddFileLocation()
        {
            List<JsonItem> output = new List<JsonItem>();
            string url = string.Empty;
            string filename = string.Empty;
            string homeDirectory = Server.MapPath(ProductImageUrl);

            // Create folder
            string path = string.Empty;
            string pathUrl = string.Empty;
            path = homeDirectory + string.Format(@"\{0}\{1}\{2}",
                DateTime.Now.Year,
                DateTime.Now.ToString("MM"),
                DateTime.Now.ToString("dd")
            );
            pathUrl = ProductImageUrl + string.Format(@"/{0}/{1}/{2}",
                DateTime.Now.Year,
                DateTime.Now.ToString("MM"),
                DateTime.Now.ToString("dd"));
            this.CreateFolder(path);

            // Save file to folder
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase fileData = Request.Files[file] as HttpPostedFileBase;
                if (fileData.ContentLength == 0 || !ValidateInput.IsImageFile(Path.GetExtension(fileData.FileName)))
                    continue;
                //check xem ảnh có lớn hơn 1MB ko
                if (fileData.ContentLength > 1048576)
                    continue;


                // Get FileName
                string fileName = StringTools.ClearSpecials(Path.GetFileNameWithoutExtension(fileData.FileName));
                string fileExt = Path.GetExtension(fileData.FileName);

                // Check file existed on this location
                if (System.IO.File.Exists(path + "\\" + string.Format(@"{0}{1}", fileName, fileExt)))
                {
                    fileName = string.Format("{0}-{1}", fileName, Guid.NewGuid());
                }
                fileName = string.Format("{0}{1}", fileName, fileExt);


                // Save to Large
                path = path + "\\" + fileName;
                fileData.SaveAs(path);
                url = string.Format("{0}/{1}", pathUrl, fileName);
                filename = fileData.FileName;

                output.Add(new JsonItem()
                {
                    Name = filename,
                    Url = url
                });

            }
            // Trả lại file vừa upload lên trên máy chủ

            return Json(output);
        }


        #region #Folder|Files
        /// <summary>
        /// Tạo thư mục nếu chưa tồn tại, thư mục chứa ảnh, tài liệu đính kèm được tải lên
        /// </summary>
        /// <param name="path">/Upload/Thumbs/Thumb/100/2016/08/20/[images/*]</param>
        private void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }


        /// <summary>
        /// Sao chép ảnh từ đường dẫn cũ đã được lưu trên hệ thống, sang đường dẫn mới
        /// </summary>
        /// <param name="originalFilePath">Đường dẫn cũ: /Upload/Thumbs/Large/100/2016/08/20/abc.gif</param>
        /// <param name="outputFilePath">Đường dẫn mới: /Upload/Thumbs/Thumb/100/2016/08/20/abc.gif</param>
        /// <param name="width">Độ rộng của ảnh</param>
        /// <param name="height">Chiều cao của ảnh</param>
        private void CreateThumbnail(string originalFilePath, string outputFilePath, int width, int height)
        {
            // Do nothing if the origin is smaller than the designated
            // thumbnail dimension
            var image = new Bitmap(originalFilePath);
            var brush = new SolidBrush(System.Drawing.Color.White);
            float scale = Math.Min((float)width / (float)image.Width, (float)height / (float)image.Height);

            try
            {
                var bmp = new Bitmap((int)width, (int)height);
                var graph = Graphics.FromImage(bmp);

                // uncomment for higher quality output
                graph.InterpolationMode = InterpolationMode.High;
                graph.CompositingQuality = CompositingQuality.HighQuality;
                graph.SmoothingMode = SmoothingMode.AntiAlias;

                var scaleWidth = (int)(image.Width * scale);
                var scaleHeight = (int)(image.Height * scale);

                graph.FillRectangle(brush, new RectangleF(0, 0, width, height));
                int x = ((int)width - scaleWidth) / 2;
                int y = ((int)height - scaleHeight) / 2;

                graph.DrawImage(image, new Rectangle(x, y, scaleWidth, scaleHeight));

                // Save to new folder
                bmp.Save(outputFilePath);
            }
            catch
            {
            }
        }
        #endregion
    }
}