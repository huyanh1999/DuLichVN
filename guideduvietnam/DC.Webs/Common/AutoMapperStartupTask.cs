using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;
using AutoMapper;
using DC.Common.Utility;
using DC.Entities.Domain;
using DC.Models.Authorize;
using DC.Models.Cms;
using DC.Models.Logging;
using DC.Models.Media;
using DC.Models.Posts;

namespace DC.Webs.Common
{
    public class AutoMapperStartupTask
    {
        public static void Configure()
        {
            MapAuthorize();
            MapCms();
            MapLogging();
            MapMedia();
            MapPost();

        }

        #region Authorize


        private static void MapAuthorize()
        {
            Mapper.CreateMap<User, UserModel>().ForMember(dest => dest.CreateDate,
                opt => opt.MapFrom(src => StringTools.ConvertToString(src.CreateDate)));
            Mapper.CreateMap<UserModel, User>().ForMember(dest => dest.CreateDate, mo => mo.Ignore());            
        }

        #endregion

        #region #Cms
        private static void MapCms()
        {
            
            //Category
            Mapper.CreateMap<Category, CategoryModel>();
            Mapper.CreateMap<CategoryModel, Category>();            

            //Menu
            Mapper.CreateMap<Menu, MenuModel>();
            Mapper.CreateMap<MenuModel, Menu>();

            //Parameter
            Mapper.CreateMap<Parameter, ParametersModel>();
            Mapper.CreateMap<ParametersModel, Parameter>();

            //Hotel
            Mapper.CreateMap<Hotel, HotelModel>();
            Mapper.CreateMap<HotelModel, Hotel>();
        }

        #endregion


        #region #MapMedia
        private static void MapMedia()
        {

            //Slider
            Mapper.CreateMap<Slide, SlideModel>();
            Mapper.CreateMap<SlideModel, Slide>();            
        }
        #endregion

        #region #MapProduct
        private static void MapPost()
        {
            //Post
            Mapper.CreateMap<Post, PostModel>().ForMember(dest => dest.CreateDate,
               opt => opt.MapFrom(src => StringTools.ConvertToString(src.CreateDate)))
               .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => StringTools.ConvertToString(src.ModifiedDate)));
            Mapper.CreateMap<PostModel, Post>()
                .ForMember(dest => dest.CreateDate, mo => mo.Ignore())
                .ForMember(dest => dest.ModifiedDate, mo => mo.Ignore());            

            // productImages
            Mapper.CreateMap<PostImage, PostImageModel>();
            Mapper.CreateMap<PostImageModel, PostImage>();
            
            //Tag
            Mapper.CreateMap<Tag, TagModel>();
            Mapper.CreateMap<TagModel, Tag>();            
        }
        #endregion

        #region #MapLog

        private static void MapLogging()
        {
            // ActivityLog
            Mapper.CreateMap<ActivityLog, ActivityLogModel>()
                .ForMember(dest => dest.CreateDate,
                    opt => opt.MapFrom(src => StringTools.ConvertToFullString(src.CreateDate)));
            Mapper.CreateMap<ActivityLogModel, ActivityLog>()
                .ForMember(dest => dest.CreateDate, mo => mo.Ignore());


            // ActivityLogType
            Mapper.CreateMap<ActivityLogType, ActivityLogTypeModel>();
            Mapper.CreateMap<ActivityLogTypeModel, ActivityLogType>();
        }
        #endregion

        
    }
}