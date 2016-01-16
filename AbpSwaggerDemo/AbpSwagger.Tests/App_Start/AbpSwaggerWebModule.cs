﻿using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Application.Services;
using Abp.Modules;
using Abp.WebApi.Swagger;
using Abp.WebApi.Swagger.Application;
using Abp.WebApi.Swagger.Builders;
using Abp.WebApi.Swagger.Configuration;
using AbpSwagger.Application;
using AbpSwagger.Application.Classes;
using AbpSwagger.Application.WebApi.WebApi;
using OtherApp.Application;

namespace AbpSwagger.Tests
{
    [DependsOn(
       typeof(AbpSwaggerModule),
       typeof(AbpSwaggerAppModule),
       typeof(OtherAppModule),
       typeof(SwaggerWebApiModule))]
    public class AbpSwaggerWebModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            var configure = IocManager.Resolve<IAbpSwaggerModuleConfiguration>();
            configure.AbpSwaggerUiConfigure = (x) =>
            {
                x.DocExpansion(DocExpansion.List);
                x.BooleanValues(new string[] { "0", "1" });
                x.EnableTranslator("zh-cn");
            };

            AbpSwaggerBuilder.ForAll<IApplicationService>(typeof (AbpSwaggerAppModule).Assembly)
                .Where(x => x != typeof (IClassAppService))
                .Build("AbpSwagger");

            AbpSwaggerBuilder.ForAll<IApplicationService>(typeof(OtherAppModule).Assembly)
               .Build("OtherModule").EnableAbpSwaggerUi();


            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}