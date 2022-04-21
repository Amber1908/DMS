


using Unity;
using Unity.AspNet.Mvc;
using X1APServer.Infrastructure.Common;
using System;
using System.Reflection;
using System.Web.Http;
using X1APServer.Repository;
using X1APServer.Repository.Utility.Interface;
using X1APServer.Repository.Utility;
using Unity.RegistrationByConvention;
using System.Web;
using System.Collections.Generic;
using X1APServer.Service.Interface;
using System.Diagnostics;
using WebApplication1.Infrastructure.Common;
using X1APServer.Service.Interface;
using WebApplication1.Infrastructure.Common.Interface;
using System.Configuration;
using NLog;
using System.Web.Configuration;

namespace X1APServer.WEB
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        private static readonly string _connStrTemplate = ConfigurationManager.AppSettings["ConnectionStringTemplate"];

        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              //RegisterComponents(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion
        //todo TINGYU
        public static Func<IUnityContainer, object> getConnectionString = c =>
        {
            var connectionString = "";
            var sessionkey = HttpContext.Current.Request.Headers["SessionKey"];
            var WebSN = HttpContext.Current.Request.Headers["WebSN"];
            if (sessionkey != null)
            {
                if (!GlobalVariable.Instance.ContainsKey(sessionkey))
                {
                    var svc = c.Resolve<IDMSShareService>();
                    var dmsSetting = svc.GetDMSSetting(sessionkey);
                    GlobalVariable.Instance.TryAdd(sessionkey, dmsSetting.Web_db);
                }
                connectionString = string.Format(_connStrTemplate, GlobalVariable.Instance.Get(sessionkey));
                //TingYU 拿到站台名稱
                System.Web.HttpContext.Current.Session["Web_DB"] = GlobalVariable.Instance.Get(sessionkey);
            }
            else if(WebSN != null)
            {
                var svc = c.Resolve<IDMSShareService>();
                var dmsSetting = svc.GetDMSSettingBySN(int.Parse(WebSN));
                connectionString = string.Format(_connStrTemplate, dmsSetting.Web_db);
            }

            return new X1APEntities(connectionString);
        };


        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            //var dbContext = new X1APEntities();


            // DbContext
            //container.RegisterType<X1APEntities, X1APEntities>(new PerRequestLifetimeManager());
            container.RegisterFactory<X1APEntities>(getConnectionString, new PerRequestLifetimeManager());
            container.RegisterType<DMSShareEntities, DMSShareEntities>(new PerRequestLifetimeManager());
            
            //Logger
            //container.RegisterType<ILogger, EnterpriseLogger>();
            //container.RegisterType<ILogger, NLogger>();

            // UnitOfWork
            //container.RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager());
            container.RegisterType<IX1UnitOfWork, X1UnitOfWork>(new PerRequestLifetimeManager());
            container.RegisterType<IDMSShareUnitOfWork, DMSShareUnitOfWork>(new PerRequestLifetimeManager());
            //container.RegisterType<IDbContextProxy, DbContextProxy>(new PerRequestLifetimeManager());
            container.RegisterType<IX1DbContextProxy, X1DbContextProxy>(new PerRequestLifetimeManager());
            container.RegisterType<IDMSShareDbContextProxy, DMSShareDbContextProxy>(new PerRequestLifetimeManager());

            container.RegisterType<IFrameRequest, FrameRequest>(new PerRequestLifetimeManager());

            // Repository
            //container.RegisterType(typeof(IBasicRepository<>), typeof(BasicRepository<>));

            container.RegisterTypes(
                AllClasses.FromAssemblies(true, Assembly.Load("X1APServer.Repository")),
                WithMappings.FromMatchingInterface,
                WithName.Default);

            // Service
            container.RegisterTypes(
                AllClasses.FromAssemblies(true, Assembly.Load("X1APServer.Service")),
                WithMappings.FromMatchingInterface,
                WithName.Default);
        }


        public static void RegisterComponents(IUnityContainer container)
        {
            //var dbContext = new X1APEntities();

            // DbContext
            //container.RegisterType<X1APEntities, X1APEntities>(new PerRequestLifetimeManager());
            container.RegisterFactory<X1APEntities>(getConnectionString, new PerRequestLifetimeManager());
            container.RegisterType<DMSShareEntities, DMSShareEntities>(new PerRequestLifetimeManager());

            //Logger
            //container.RegisterType<ILogger, EnterpriseLogger>();
            //container.RegisterType<ILogger, NLogger>();

            // UnitOfWork
            //container.RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager());
            container.RegisterType<IX1UnitOfWork, X1UnitOfWork>(new PerRequestLifetimeManager());
            container.RegisterType<IDMSShareUnitOfWork, DMSShareUnitOfWork>(new PerRequestLifetimeManager());
            //container.RegisterType<IDbContextProxy, DbContextProxy>(new PerRequestLifetimeManager());
            container.RegisterType<IX1DbContextProxy, X1DbContextProxy>(new PerRequestLifetimeManager());
            container.RegisterType<IDMSShareDbContextProxy, DMSShareDbContextProxy>(new PerRequestLifetimeManager());

            container.RegisterType<IFrameRequest, FrameRequest>(new PerRequestLifetimeManager());

            // Repository
            //container.RegisterType(typeof(IBasicRepository<>), typeof(BasicRepository<>));

            container.RegisterTypes(
                AllClasses.FromAssemblies(true, Assembly.Load("X1APServer.DAO")),
                WithMappings.FromMatchingInterface,
                WithName.Default);

            // Service
            container.RegisterTypes(
                AllClasses.FromAssemblies(true, Assembly.Load("X1APServer.Service")),
                WithMappings.FromMatchingInterface,
                WithName.Default);

            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
            //GlobalConfiguration.Configuration.DependencyResolver = new Unity.AspNet.WebApi.UnityHierarchicalDependencyResolver(container);
        }
    }
}