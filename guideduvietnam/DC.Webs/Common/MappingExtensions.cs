using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;
using AutoMapper;
using DC.Entities.Domain;
using DC.Models.Authorize;
using DC.Models.Cms;
using DC.Models.Logging;
using DC.Models.Posts;
using DC.Models.Media;

namespace DC.Webs.Common
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return Mapper.Map(source, destination);
        }


        #region #Authorize - User
        
        public static UserModel ToModel(this User entity)
        {
            return entity.MapTo<User, UserModel>();
        }

        public static User ToEntity(this UserModel model)
        {
            return model.MapTo<UserModel, User>();
        }

        public static User ToEntity(this UserModel model, User destination)
        {
            return model.MapTo(destination);
        }
        #endregion        
        

        #region #Category
        public static CategoryModel ToModel(this Category entity)
        {
            return entity.MapTo<Category, CategoryModel>();
        }

        public static Category ToEntity(this CategoryModel model)
        {
            return model.MapTo<CategoryModel, Category>();
        }

        public static Category ToEntity(this CategoryModel model, Category destination)
        {
            return model.MapTo(destination);
        }
        #endregion                              

        #region #Menu
        // Menu
        public static MenuModel ToModel(this Menu entity)
        {
            return entity.MapTo<Menu, MenuModel>();
        }

        public static Menu ToEntity(this MenuModel model)
        {
            return model.MapTo<MenuModel, Menu>();
        }

        public static Menu ToEntity(this MenuModel model, Menu destination)
        {
            return model.MapTo(destination);
        }


        #endregion

        #region #Parameter
        // Parameter
        public static ParameterModel ToModel(this Parameter entity)
        {
            return entity.MapTo<Parameter, ParameterModel>();
        }

        public static Parameter ToEntity(this ParameterModel model)
        {
            return model.MapTo<ParameterModel, Parameter>();
        }

        public static Parameter ToEntity(this ParameterModel model, Parameter destination)
        {
            return model.MapTo(destination);
        }


        #endregion

        #region #Post
        public static PostModel ToModel(this Post entity)
        {
            return entity.MapTo<Post, PostModel>();
        }

        public static Post ToEntity(this PostModel model)
        {
            return model.MapTo<PostModel, Post>();
        }

        public static Post ToEntity(this PostModel model, Post destination)
        {
            return model.MapTo(destination);
        }

        //PostImage
        public static PostImageModel ToModel(this PostImage entity)
        {
            return entity.MapTo<PostImage, PostImageModel>();
        }

        public static PostImage ToEntity(this PostImageModel model)
        {
            return model.MapTo<PostImageModel, PostImage>();
        }

        public static PostImage ToEntity(this PostImageModel model, PostImage destination)
        {
            return model.MapTo(destination);
        }
        #endregion


        #region #Logging
        // ActivityLog
        public static ActivityLogModel ToModel(this ActivityLog entity)
        {
            return entity.MapTo<ActivityLog, ActivityLogModel>();
        }

        public static ActivityLog ToEntity(this ActivityLogModel model)
        {
            return model.MapTo<ActivityLogModel, ActivityLog>();
        }

        public static ActivityLog ToEntity(this ActivityLogModel model, ActivityLog destination)
        {
            return model.MapTo(destination);
        }


        // ActivityLogType
        public static ActivityLogTypeModel ToModel(this ActivityLogType entity)
        {
            return entity.MapTo<ActivityLogType, ActivityLogTypeModel>();
        }

        public static ActivityLogType ToEntity(this ActivityLogTypeModel model)
        {
            return model.MapTo<ActivityLogTypeModel, ActivityLogType>();
        }

        public static ActivityLogType ToEntity(this ActivityLogTypeModel model, ActivityLogType destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region #Slide

        // Parameter
        public static SlideModel ToModel(this Slide entity)
        {
            return entity.MapTo<Slide, SlideModel>();
        }

        public static Slide ToEntity(this SlideModel model)
        {
            return model.MapTo<SlideModel, Slide>();
        }

        public static Slide ToEntity(this SlideModel model, Slide destination)
        {
            return model.MapTo(destination);
        }


        #endregion

        #region #Tag

        // Tags
        public static TagModel ToModel(this Tag entity)
        {
            return entity.MapTo<Tag, TagModel>();
        }

        public static Tag ToEntity(this TagModel model)
        {
            return model.MapTo<TagModel, Tag>();
        }

        public static Tag ToEntity(this TagModel model, Tag destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region #Hotel

        // Hotel
        public static HotelModel ToModel(this Hotel entity)
        {
            return entity.MapTo<Hotel, HotelModel>();
        }

        public static Hotel ToEntity(this HotelModel model)
        {
            return model.MapTo<HotelModel, Hotel>();
        }

        public static Hotel ToEntity(this HotelModel model, Hotel destination)
        {
            return model.MapTo(destination);
        }

        #endregion
    }
}