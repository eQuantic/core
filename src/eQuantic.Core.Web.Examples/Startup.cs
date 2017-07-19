using AutoMapper;
using eQuantic.Core.Extensions;
using eQuantic.Core.Web.Examples.Domain.Mappers;
using eQuantic.Core.Web.Examples.Domain.Services;
using eQuantic.Core.Web.Examples.Domain.Services.Contracts;
using eQuantic.Core.Web.Examples.Infrastructure;
using eQuantic.Core.Web.Examples.Infrastructure.Repositories;
using eQuantic.Core.Web.Examples.Infrastructure.Repositories.Contracts;
using eQuantic.Core.Web.Examples.Ioc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StructureMap;
using StructureMap.Pipeline;
using System;
using System.Linq;
using System.Reflection;

namespace eQuantic.Core.Web.Examples
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var container = new ExampleContainer();

            services.AddDbContext<ExampleDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            container.Configure(config =>
            {
                config.For<ExampleUnitOfWork>().Transient();

                config.Scan(s =>
                {
                    s.AssemblyContainingType<UserMapperProfile>();

                    s.Include(
                        t =>
                            !t.GetTypeInfo().IsAbstract && !t.GetTypeInfo().IsGenericTypeDefinition &&
                            typeof(Profile).IsAssignableFrom(t));
                    s.Convention<BaseTypeConvention>();
                });

                config.For<MapperConfiguration>().Use(o => new MapperConfiguration(c => SetMapperConfiguration(o, c))).Singleton();
                config.For<AutoMapper.IConfigurationProvider>().Use(o => o.GetInstance<MapperConfiguration>()).Singleton();
                config.For<IMapper>().Use(o => o.GetInstance<MapperConfiguration>().CreateMapper()).Singleton();

                config.For<IPersonRepository>().Use<PersonRepository>();
                config.For<IPersonService>().Use<PersonService>();

                config.For<eQuantic.Core.Ioc.IContainer>()
                    .UseInstance(new ObjectInstance(container));

                config.Populate(services);
            });
                // Add framework services.
            services.AddMvc();

            return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void SetMapperConfiguration(IContext o, IMapperConfigurationExpression c)
        {
            c.ConstructServicesUsing(o.GetInstance);
            c.CreateMap<Guid, ShortGuid>().ConvertUsing(g => ShortGuid.Encode(g));
            c.CreateMap<ShortGuid, Guid>().ConvertUsing(sg => sg.ToGuid());
            o.GetAllInstances<Profile>().ForEach(c.AddProfile);
        }
    }
}
