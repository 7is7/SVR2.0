using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using AutoMapper;
using GlobalExceptionHandler.WebApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using VueExample.Color;
using VueExample.Contexts;
using VueExample.Exceptions;
using VueExample.Helpers;
using VueExample.Hubs;
using VueExample.Providers;
using VueExample.Providers.Abstract;
using VueExample.Providers.ChipVerification;
using VueExample.Providers.ChipVerification.Abstract;
using VueExample.Providers.Srv6;
using VueExample.Providers.Srv6.Interfaces;
using VueExample.Services;
using VueExample.StatisticsCore.Abstract;
using VueExample.StatisticsCore.Services;

namespace VueExample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression(options=>options.EnableForHttps = true);
     
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            services.AddMvc(options => { options.CacheProfiles.Add("Default60",
                            new CacheProfile()
                            {
                                Duration = 3600
                            });
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAutoMapper();
            services.AddLazyCache();
            services.AddSignalR();
            services.AddOptions();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "0.0.0.0:1799";
                options.InstanceName = "srvredis";
            });

            services.AddCors(o => o.AddPolicy("DefaultPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
                
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v0.2.2", new Info
                {
                    Version = "v0.2.2",
                    Title = "SVR_API",
                    Description = "SVR_MES_19_API_0.2.2"
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        

            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ApplicationContext")), ServiceLifetime.Transient);
            services.AddDbContext<Srv6Context>(options => options.UseSqlServer(Configuration.GetConnectionString("SRV6Context")), ServiceLifetime.Transient);

            
            
           // services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, BackgroundTasks.OnlineTestingService>();
           
            services.AddScoped<IUserProvider, UserProvider>();
            services.AddTransient<IWaferMapProvider, WaferMapProvider>();
            services.AddTransient<IDeviceTypeProvider, DeviceTypeProvider>();
            services.AddTransient<IDieProvider, DieProvider>();
            services.AddTransient<IDividerService, DividerService>();
            services.AddTransient<IMeasurementProvider, SimpleMeasurementProvider>();
            services.AddTransient<IGraphicProvider, BasicGraphicProvider>();
            services.AddTransient<ISRV6GraphicService, GraphicService>();
            services.AddTransient<IStandartWaferService, StandartWaferService>();
            services.AddTransient<IUploaderService, UploaderService>();
            services.AddTransient<IDefectProvider, DefectProvider>();
            services.AddTransient<IMeasurementRecordingService, MeasurementRecordingService>();
            services.AddTransient<IPhotoProvider, PhotoProvider>();
            services.AddTransient<IFileGraphicUploaderService, FileGraphicUploaderService>();
            services.AddTransient<IChartJSProvider, ChartJSProvider>();
            services.AddTransient<IDieValueService, DieValueService>();
            services.AddTransient<IColorService, ColorService>();
            services.AddTransient<IGradientService, GradientService>();
            services.AddTransient<IElementService, ElementService>();
            services.AddTransient<IStatParameterService, StatParameterService>();
            services.AddTransient<IElementTypeProvider, ElementTypeProvider>();
            services.AddTransient<IElementTypeService, ElementTypeService>();
            services.AddTransient<ISpecificElementTypeProvider, SpecificElementTypeProvider>();
            services.AddTransient<ISpecificElementTypeService, SpecificElementTypeService>();
            services.AddTransient<IFolderService, FolderService>();
            services.AddTransient<IDieTypeProvider, DieTypeProvider>();
            services.AddTransient<IWaferProvider, WaferProvider>();
            services.AddTransient<IStandartWaferProvider, StandartWaferProvider>();
            services.AddTransient<ICodeProductProvider, CodeProductProvider>();
            services.AddTransient<IProcessProvider, ProcessProvider>();
            services.AddTransient<IDeviceProvider, DeviceProvider>();
            services.AddTransient<IStageProvider, StageProvider>();
            services.AddTransient<IMeasurementSetProvider, MeasurementSetProvider>();
            services.AddTransient<IAtomicMeasurementProvider, AtomicMeasurementProvider>();
            services.AddTransient<IMaterialProvider, MaterialProvider>();
            services.AddTransient<IFacilityProvider, FacilityProvider>();
            services.AddTransient<IMeasuredDeviceProvider, MeasuredDeviceProvider>();
            services.AddTransient<IPointProvider, PointProvider>();
            services.AddTransient<IExportProvider, ExportService>();
            services.AddTransient<IShortLinkProvider, ShortLinkProvider>();
            services.AddTransient<IStandartParameterProvider, StandartParameterProvider>();
            services.AddTransient<IStandartParameterService, StandartParameterService>();
            services.AddTransient<IStandartPatternProvider, StandartPatternProvider>();
            services.AddTransient<IStandartPatternService, StandartPatternService>();
            services.AddTransient<IStandartMeasurementPatternProvider, StandartMeasurementPatternProvider>();
            services.AddTransient<IStandartMeasurementPatternService, StandartMeasurementPatternService>();
            services.AddTransient<IKurbatovParameterBordersProvider, KurbatovParameterBordersProvider>();
            services.AddTransient<IKurbatovParameterBordersService, KurbatovParameterBordersService>();
            services.AddTransient<IKurbatovParameterProvider, KurbatovParameterProvider>();
            services.AddTransient<IKurbatovParameterService, KurbatovParameterService>();
            
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
          
            if (env.IsDevelopment())
            {              
                app.UseBrowserLink();
                app.UseStatusCodePages();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }

            app.UseGlobalExceptionHandler(x => {
                x.ContentType = "application/json";
                x.ResponseBody(s => JsonConvert.SerializeObject(new
                {
                    Message = "An error occurred while processing your request"
                }));
                x.Map<RecordNotFoundException>().ToStatusCode(StatusCodes.Status404NotFound);
                x.Map<CollectionIsEmptyException>().ToStatusCode(StatusCodes.Status404NotFound);
                x.Map<ValidationErrorException>().ToStatusCode(StatusCodes.Status403Forbidden);
            });           

            app.UseSignalR(options =>
            {
                options.MapHub<LivePointHub>("/livepoint");                
            });

            app.UseCors("DefaultPolicy");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v0.2.2/swagger.json", "SVR_MES_19_API_0.2.2");
            });


            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseResponseCompression();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");

            });

            app.MapWhen(x => !x.Request.Path.Value.StartsWith("/api"), builder =>
            {
                builder.UseMvc(routes =>
                {
                    routes.MapSpaFallbackRoute(
                        name: "spa-fallback",
                        defaults: new { controller = "Home", action = "Index" });
                });
            });
        }
    }
}
