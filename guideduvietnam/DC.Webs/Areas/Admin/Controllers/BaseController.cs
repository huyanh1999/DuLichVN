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
using DC.Models.Authorize;
using DC.Models.Logging;
using DC.Models.Posts;
using DC.Services;
using DC.Services.Authorize;
using DC.Services.Logging;
using DC.Webs.Common;
using DC.Webs.Models;
using DC.Services.Cms;
using DC.Services.Posts;

namespace DC.Webs.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        // Images
        public static string PostImageUrl = ConfigurationManager.AppSettings["PostImageUrl"].ToString();
        public static string SliderImageUrl = ConfigurationManager.AppSettings["SliderImageUrl"].ToString();
        public static string AdsImageUrl = ConfigurationManager.AppSettings["AdsImageUrl"].ToString();
        public static string ProductImageUrl = ConfigurationManager.AppSettings["ProductImageUrl"].ToString();
        public static string VideoImageUrl = ConfigurationManager.AppSettings["VideoImageUrl"].ToString();
        public static string MapImageUrl = ConfigurationManager.AppSettings["MapImageUrl"].ToString();

        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;
        private readonly ITagsService _tagsService;
        private readonly IActivityLogTypeService _activityLogTypeService;
        private readonly IActivityLogService _activityLogService;

        public BaseController()
        {
            this._userService = DependencySolve.GetServiceName<IUserService>();
            this._categoryService = DependencySolve.GetServiceName<ICategoryService>();
            this._tagsService = DependencySolve.GetServiceName<ITagsService>();
            this._activityLogService = DependencySolve.GetServiceName<IActivityLogService>();
            this._activityLogTypeService = DependencySolve.GetServiceName<IActivityLogTypeService>();
        }


        #region #CurrentUserLoggedIn
        /// <summary>
        /// Lấy thông tin người dùng đăng nhập hệ thống theo tên người dùng
        /// Tên người dùng có thể là tên tài khoản hoặc địa chỉ email cá nhân
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserModel GetUserByName(string username)
        {
            var user = _userService.GetByUserName(username);
            return user?.ToModel();
        }


        /// <summary>
        /// Kiểm tra thông tin người dùng đăng nhập hợp lệ
        /// </summary>
        /// <param name="username">Tên người dùng - địa chỉ email dùng để đăng nhập hệ thống</param>
        /// <param name="password">Mật khẩu người dùng</param>
        /// <returns></returns>
        public bool ValidateUser(string username, string password, ref string roleName)
        {
            var user = GetUserByName(username);

            if (user == null || user.IsLockedOut == true || user.Password != password)
                return false;

            // Lưu trữ thông tin người dùng truy cập theo biến ứng dụng
            // Với khóa là duy nhất cho từng tài khoản truy cập      
            roleName = user.RoleName;

            return true;
        }


        /// <summary>
        /// Lấy thông tin người dùng truy cập hệ thống
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserModel GetCurrentUserLoggedIn(string username)
        {
            var user = _userService.GetByUserName(username);

            return user?.ToModel();
        }


        /// <summary>
        /// Lấy mã người dùng đăng nhập hệ thống
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GetCurrentUserId(string username)
        {
            var user = _userService.GetByUserName(username);
            if (user != null)
                return user.Id;

            return 0;
        }
        #endregion

        #region #Common
        /// <summary>
        /// Thiết lập giá trị cho model dùng chung của toàn hệ thống backend
        /// Gán người dùng, thiết lập sở hữu website, ngôn ngữ lựa chọn
        /// </summary>
        /// <param name="model"></param>
        public void SetupBaseModel(BaseViewModel model)
        {
            // Check Authorize
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("/Admin/Login");
            
            // Set user to view
            var user = new UserLoggedIn();
            var userObj = this._userService.GetByUserName(User.Identity.Name);
            if (userObj != null)
            {
                var userModel = userObj.ToModel();
                user.UserId = userModel.Id;
                user.Username = userModel.UserName;
                user.Email = userModel.Email;
                user.RoleName = userModel.RoleName;
                user.LastName = userModel.LastName;
                model.CurrentUser = user;
                
            }
            else
                Response.Redirect("/Admin/Login");

        }
        #endregion


        #region #CategoryItems

        public List<SelectItemModel> GetCategoryOptions(string type,bool all)
        {
            List<SelectItemModel> list = new List<SelectItemModel>();
            if (all)
                list.Add(new SelectItemModel() {Text = "--Chọn danh mục--",Value = "0"});
            var cateObj = this._categoryService.GetAll(type);
            int n = cateObj.Count;
            for (int i = 0; i < n; i++)
            {
                if (cateObj[i].ParentId == 0)
                {
                    list.Add(new SelectItemModel(cateObj[i].Name, cateObj[i].Id.ToString()));
                    for (int j = 0; j < n; j++)
                    {
                        if (cateObj[j].ParentId == cateObj[i].Id)
                        {
                            list.Add(new SelectItemModel("-- " + cateObj[j].Name, cateObj[j].Id.ToString()));
                            for (int k = 0; k < n; k++)
                            {
                                if (cateObj[k].ParentId == cateObj[j].Id)
                                {
                                    list.Add(new SelectItemModel("---- " + cateObj[k].Name, cateObj[k].Id.ToString()));
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }


        #endregion


        #region #TagsItem
        public List<SelectItemModel> GetTagsOptions(string type)
        {
            var tagsObj = this._tagsService.GetAll(type);
            return tagsObj.Select(item => new SelectItemModel(item.Name, item.Id.ToString())).ToList();
        }

        public List<TagModel> GetTagItems(string type)
        {
            var tagsObj = this._tagsService.GetAll(type);
            List<TagModel> list = new List<TagModel>();
            if (tagsObj.Any())
                list = tagsObj.Select(c => c.ToModel()).ToList();
            return list;
        }
        #endregion


        #region #Loggings
        /// <summary>
        /// Lưu nhật ký hoạt động người dùng trên hệ thống
        /// </summary>
        /// <param name="activityLogTypeName">Tên loại hành động: Insert, Update, Delete,...</param>
        /// <param name="comment">Câu ghi chú chi tiết hành động, đối tượng</param>
        public void AddActivityLog(string activityLogTypeName, string comment)
        {
            try
            {
                var userObj = this._userService.GetByUserName(User.Identity.Name);
                ActivityLogModel activityLog = new ActivityLogModel();
                var activityLogTypeObj = this._activityLogTypeService.GetSingle(activityLogTypeName);
                if (activityLogTypeObj != null)
                {
                    activityLog.ActivityLogTypeId = activityLogTypeObj.Id;
                    activityLog.SessionId = Session[SessionConst.SESSION_START].ToString();
                    activityLog.Comment = comment;
                    activityLog.ResourceType = userObj.RoleName;
                    activityLog.UserId = userObj.Id;
                    this._activityLogService.Insert(activityLog.ToEntity());
                }
            }
            catch
            {

            }

        }
        #endregion


        #region #Options


        /// <summary>
        /// Trạng thái
        /// </summary>
        /// <param name="allOptions"></param>
        /// <returns></returns>
        public List<SelectItemModel> GetStatusOptions(bool allOptions)
        {
            List<SelectItemModel> list = new List<SelectItemModel>();
            if (allOptions)
                list.Add(new SelectItemModel() { Text = "Chọn trạng thái...",Value = " "});
            list.Add(new SelectItemModel("Kích hoạt", StatusConst.PUBLISHNAME));
            list.Add(new SelectItemModel("Nháp", StatusConst.DRAFTNAME));
            return list;
        }


        public List<SelectItemModel> GetCateTypeOptions(bool all)
        {
            List<SelectItemModel> list = new List<SelectItemModel>();
            if (all)
                list.Add(new SelectItemModel() { Text = "Chọn loại danh mục...", Value = " " });
            list.Add(new SelectItemModel("Bài viết", CategoryConst.CATEGORYPOST));
            list.Add(new SelectItemModel("Tour", CategoryConst.CATEGORYPRODUCT));
            return list;
        }

        #endregion



        #region #Upload|File
        public string AddFiles(string folder, ref bool maxsize)
        {
            string url = string.Empty;
            string homeDirectory = Server.MapPath(folder);

            // Create thumbs/large folder
            string largePath = string.Empty;
            string largeUrl = string.Empty;
            

            foreach (string file in Request.Files)
            {
                HttpPostedFileBase fileData = Request.Files[file] as HttpPostedFileBase;
                if (fileData.ContentLength == 0 || !ValidateInput.IsImageFile(Path.GetExtension(fileData.FileName)))
                    continue;

                largePath = homeDirectory + string.Format(@"\{0}\{1}\{2}",
                DateTime.Now.Year,
                DateTime.Now.ToString("MM"),
                DateTime.Now.ToString("dd")
                 );
                largeUrl = string.Format(@"{0}/{1}/{2}/{3}",
                    folder,
                    DateTime.Now.Year,
                    DateTime.Now.ToString("MM"),
                    DateTime.Now.ToString("dd")
                );
                CreateFolder(largePath);

                // Get FileName
                string fileName = StringTools.ClearSpecials(Path.GetFileNameWithoutExtension(fileData.FileName));
                string fileExt = Path.GetExtension(fileData.FileName);

                // Check file existed on this location
                if (System.IO.File.Exists(largePath + "\\" + string.Format(@"{0}{1}", fileName, fileExt)))
                {
                    fileName = string.Format("{0}-{1}", fileName, Guid.NewGuid());
                }
                fileName = string.Format("{0}{1}", fileName, fileExt);


                // Save to Large
                largePath = largePath + "\\" + fileName;
                fileData.SaveAs(largePath);
                url = string.Format("{0}/{1}", largeUrl, fileName);

            }


            // Trả lại ảnh vừa upload lên trên máy chủ
            return url;
        }
        #endregion

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
        
        #endregion


        #region #Upload|File to auto Resize
        /// <summary>
        /// Upload image and resize images
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public string AddFilesResize(string folder,int width, int height)
        {
            string url = string.Empty;
            string homeDirectory = Server.MapPath(folder);

            // Create thumbs/large folder
            string largePath = string.Empty;
            string largeUrl = string.Empty;
            string imgPath = string.Empty;
            string fileNameAfterUpload = string.Empty;
            

            foreach (string file in Request.Files)
            {
                HttpPostedFileBase fileData = Request.Files[file] as HttpPostedFileBase;
                if (fileData.ContentLength == 0 || !ValidateInput.IsImageFile(Path.GetExtension(fileData.FileName)))
                    continue;

                largePath = homeDirectory + string.Format(@"\{0}\{1}\{2}",
                DateTime.Now.Year,
                DateTime.Now.ToString("MM"),
                DateTime.Now.ToString("dd")
            );
                largeUrl = string.Format(@"{0}/{1}/{2}/{3}",
                    folder,
                    DateTime.Now.Year,
                    DateTime.Now.ToString("MM"),
                    DateTime.Now.ToString("dd")
                );
                imgPath = largePath + "\\";

                CreateFolder(largePath);
                // Get FileName
                string fileName = StringTools.ClearSpecials(Path.GetFileNameWithoutExtension(fileData.FileName));
                string fileExt = Path.GetExtension(fileData.FileName);

                // Check file existed on this location
                if (System.IO.File.Exists(largePath + "\\" + string.Format(@"{0}{1}", fileName, fileExt)))
                {
                    fileName = string.Format("{0}-{1}", fileName, Guid.NewGuid());
                }
                fileNameAfterUpload = fileName + "1.jpg";
                fileName = string.Format("{0}{1}", fileName, fileExt);


                // Save to Large
                largePath = largePath + "\\" + fileName;
                fileData.SaveAs(largePath);
                
                ResizeImageTools.PerformImageResizeAndPutOnCanvas(imgPath, fileName, width, height, fileNameAfterUpload);

                fileData.InputStream.Dispose();
             
                if (System.IO.File.Exists(largePath))
                {
                    System.IO.File.Delete(largePath);
                }
                url = string.Format("{0}/{1}", largeUrl, fileNameAfterUpload);

            }


            // Trả lại ảnh vừa upload lên trên máy chủ
            return url;
        }
        #endregion
    }
}