using Autofac;
using Courier_lockers.Data;
using Courier_lockers.Helper;
using Courier_lockers.Models;
using Courier_lockers.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ServiceStack.Redis;
using System.Reflection;
using System.Text;
using WMSService.Helper;

namespace Courier_lockers
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule(new AutofacModuleRegister());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                //需要从加载配置文件appsettings.json
                services.AddOptions();
                //获取第三方服务地址  ..
                services.Configure<OtherServerModel>(Configuration.GetSection("OtherServerConfig"));

                services.AddControllers();
                services.AddControllers().AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.PropertyNamingPolicy = new JsonPolicy.UpperCaseNamingPolicy();
                    opt.JsonSerializerOptions.IgnoreNullValues = true;
                });
                // 添加 CORS 服务
                //services.AddCors(options =>
                //{
                //    options.AddPolicy("AllowSpecificOrigin",
                //        builder =>
                //        {
                //            builder.WithOrigins("")
                //                   .AllowAnyHeader()
                //                   .AllowAnyMethod();
                //        });
                //});
                //services.AddMvc(options => options.Filters.Add(typeof(ActionAttribute)));
                #region Swagger
                services.AddSwaggerGen(options =>
                {
                    options.CustomSchemaIds(type => type.FullName);

                    const string ApiVersion = "v1";
                    const string ApiName = "API";

                    options.SwaggerDoc(ApiVersion, new OpenApiInfo()
                    {
                        Version = ApiVersion,
                        Title = $"{ApiName} 接口文档"
                    });

                    // 使用反射获取xml文件。并构造出文件的路径
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    // 启用xml注释. 该方法第二个参数启用控制器的注释，默认为false.
                    //options.IncludeXmlComments(xmlPath, true);

                });
                #endregion
                #region 注册服务接口

                //services.AddScoped<ITest, TestRepository>();
                #endregion
                //初始化redis
                //RedisClient.redisClient.InitConnect(Configuration);
                //数据库读写连接  
                services.AddDbContext<ServiceDbContext>(option =>
                {
                    option.UseMySql("server=127.0.0.1;port=3306;database=courier-lockers;user=root;password=Hr123456",
                        //看你版本设置
                new MySqlServerVersion(new Version(8, 2, 0)));
                });

                ////数据库读连接
                //services.AddDbContext<ServiceDbContext>(option =>
                //{
                //    option.UseMySql(@"Server=127.0.0.1;port=3306;database=courier-lockers;user=;password=Hr123456;ApplicationIntent=ReadOnly",
                //new MySqlServerVersion(new Version(8, 2, 0)));
                //});


                #region JWT鉴权认证
                //var jwtConfig = Configuration.GetSection("Jwt");
                //生成密钥
                //var symmetricKeyAsBase64 = jwtConfig.GetValue<string>("Secret");
                //var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);

                #endregion


                #region 注册第三方接口服务
                services.AddHttpClient();
                services.AddHttpClient<WPFHttpClient>();
                #endregion  
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }



            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });


            app.UseHttpsRedirection();

            //启用客户端IP限制速率
            //app.UseIpRateLimiting();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.UseQuartz(); //这里注入Quartz
        }
    }
}
