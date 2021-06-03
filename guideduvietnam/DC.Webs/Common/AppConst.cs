using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DC.Webs.Common
{
    public static class RoleConst
    {
        public const string ADMINISTRATOR = "Administrator"; // Quản trị hệ thống
        public const string ADMIN = "Admin"; // Quản lý
        public const string USER = "User"; // Người dùng
    }
    public static class CookieConst
    {
        public const string COOKIE_USER_KEY = ".AUTHENSERVICEBACKOFFICE";
        public const string COOKIE_CULTURE_KEY = "CookieCultureValue";
        public const string COOKIE_SHOPPING_KEY = "ShoppingCart";
        public const string COOKIE_SHIPPING_KEY = "ShippingTax";
    }

    public static class CategoryConst
    {
        public const string CATEGORYPOST = "CATEGORYPOST";
        public const string CATEGORYPRODUCT = "CATEGORYPRODUCT";
    }
    public static class TagConst
    {
        public const string TAGPOST = "TAGPOST";
        public const string TAGTOUR = "TAGTOUR";
    }
    public static class PostTypeConst
    {
        public const string POST = "POST";
        public const string HOT = "HOT";
        public const string PROMOTION = "PROMOTION";
        public const string BLOG = "BLOG";
        public const string TOUR = "TOUR";
    }

    /// <summary>
    /// Trạng thái của sản phẩm, bài đăng, danh mục...
    /// </summary>
    public static class StatusConst
    {
        public const string DRAFTNAME = "DRAFTNAME";
        public const string PUBLISHNAME = "PUBLISHNAME";
        public const string PRIVATENAME = "PRIVATENAME";
    }
    /// <summary>
    /// TaxonomyConst
    /// </summary>
    public static class TaxonomyConst
    {
        public const string PRODUCTTYPE = "PRODUCTTYPE";
    }


    public static class SessionConst
    {
        public const string ISAUTHORIZED = "IsAuthorized";
        public const string MESSAGE = "Messages";
        public const string SESSION_START = "SESSION_START";
    }

    /// <summary>
    /// Nguồn lưu log
    /// </summary>
    public static class ResourceTypeConst
    {
        public const string ADMIN = "Admin";
        public const string USER = "User";
    }

    /// <summary>
    /// Thực đơn (menu) hệ thống hỗ trợ
    /// </summary>
    public static class MenuConst
    {
        public const string MAINMENU = "MAINMENU";
        public const string FOOTERMENU = "FOOTERMENU";
        public const string DETAILPRODUCT = "DETAILPRODUCT";
    }


    /// <summary>
    /// Thông tin config
    /// </summary>
    public static class ParameterConst
    {
        public const string METATITLE = "METATITLE";
        public const string METAKEYWORD = "METAKEYWORD";
        public const string METADESCRIPTION = "METADESCRIPTION";
        public const string COMPANYNAME = "COMPANYNAME";
        public const string ADDRESS = "ADDRESS";
        public const string PHONE = "PHONE";
        public const string EMAIL = "EMAIL";
        public const string TWITTER = "TWITTER";
        public const string FACEBOOK = "FACEBOOK";
        public const string GOOGLEPLUS = "GOOGLEPLUS";
        public const string YOUTUBE = "YOUTUBE";
        public const string BUSINESSLICENSE = "BUSINESSLICENSE";
        public const string GMAP = "GMAP";
        public const string ABOUT = "ABOUT";
        public const string INSTAGRAM = "INSTAGRAM";
        public const string FOOTER = "FOOTER";
        public const string CUSTOMIZETOUR = "CUSTOMIZETOUR";
        public const string FRMCONTACT = "FRMCONTACT";
        public const string ABOUTHOME = "ABOUTHOME";
        public const string IMGMAP = "IMGMAP";
    }


    /// <summary>
    /// Vị trí hiển thị ở các page
    /// </summary>
    public static class PositionConst
    {
        public const string PAGEHOME = "PAGEHOME";
        public const string PAGESERVICE = "PAGESERVICE";
        public const string PAGENEW = "PAGENEW";
        public const string PAGENEWDETAIL = "PAGENEWDETAIL";
        public const string PAGESERVICEDETAIL = "PAGESERVICEDETAIL";
    }


    public static class NotifyStatusConst
    {
        public const string READ = "READ"; // đã xem
        public const string UNREAD = "UNREAD"; // chưa xem
    }


    public static class ActionTypeConst
    {
        public const string INSERT = "INSERT";
        public const string UPDATE = "UPDATE";
        public const string DELETE = "DELETE";
    }


    /// <summary>
    /// Trạng thái đơn hàng
    /// </summary>
    public static class OrderStatusConst
    {
        public const string PENDING = "PENDING"; // -Đang giao dịch
        public const string COMPLETE = "COMPLETE"; // - hoàn thành
        public const string CANCELED = "CANCELED"; // - Đã hủy
    }


    /// <summary>
    /// Nguồn đơn hàng
    /// </summary>
    public static class OrderSourceConst
    {
        public const string WEBSITE = "WEBSITE"; // - web
        public const string FACEBOOK = "FACEBOOK"; // - fb
        public const string ZALO = "ZALO"; // - zalo
        public const string POS = "POS"; // - của hàng
        public const string OTHER = "OTHER"; // - khác

    }


    /// <summary>
    /// Loại video
    /// </summary>
    public static class VideoTypeConst
    {
        public const string YOUTUBE = "YOUTUBE";

    }


    /// <summary>
    /// Order list product
    /// </summary>
    public static class OrderByConst
    {
        public const int AZ = 1;
        public const int ZA = 2;
        public const int PRICEASC = 3;
        public const int PRICEDESC = 4;
        public const int DATENEW = 5;
        public const int DATEOLD = 6;
    }


    /// <summary>
    /// Hình thức thanh toán
    /// </summary>
    public static class PaymentTypeConst
    {
        public const string CASH = "CASH";
        public const string TRANSFER = "TRANSFER";
    }


    /// <summary>
    /// Hình thức giao hàng
    /// </summary>
    public static class DeliveryConst
    {
        public const string POSTOFFICE = "POSTOFFICE"; // Giao qua bưu điện
        public const string HOME = "HOME"; // Giao hàng tận nơi
        public const string STORE = "STORE"; // Khách tới lấy hàng
        public const string OTHER = "OTHER"; // hình thức khác
    }


    public static class LogTypeConst
    {
        public const string INSERT = "INSERT";
        public const string UPDATE = "UPDATE";
        public const string DELETE = "DELETE";
    }


    public static class LableConst
    {
        public const string SIDEBAR_POSTNEW = "SIDEBAR_POSTNEW";
        public const string SIDEBAR_TOURNEW = "SIDEBAR_TOURNEW";
        public const string TITLE_TOUR_HOME = "TITLE_TOUR_HOME";
        public const string TITLE_DESC_TOUR_HOME = "TITLE_DESC_TOUR_HOME";
        public const string NEWS_LETTER = "NEWS_LETTER";
        public const string READ_MORE = "READ_MORE";//
        public const string HOME = "HOME";//
    }
}