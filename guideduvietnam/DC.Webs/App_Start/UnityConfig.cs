using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using DC.Entities.Base;
using DC.Entities.Domain;
using DC.Services.Authorize;
using DC.Services.Cms;
using DC.Services.Logging;
using DC.Services.Media;
using DC.Services.Posts;

namespace DC.Webs
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            container.RegisterType<DataDbContext, DataDbContext>();
            //Authorize
            container.RegisterType<IUserService, UserService>();

            //Cms
            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<IMenuService, MenuService>();
            container.RegisterType<IParameterService, ParameterService>();
            container.RegisterType<IHotelService, HotelService>();

            // Product
            container.RegisterType<IPostService, PostService>();
            container.RegisterType<ITagsService, TagsService>();
            container.RegisterType<IPostImageService, PostImageService>();
            //Media
            container.RegisterType<ISliderService, SliderService>();

            //Logging
            container.RegisterType<IActivityLogService, ActivityLogService>();
            container.RegisterType<IActivityLogTypeService, ActivityLogTypeService>();
            
        }
    }
}