using Autofac;
using Courier_lockers.Data;
using Courier_lockers.Helper;
using Courier_lockers.Models;
using Courier_lockers.Services;
using Courier_lockers.Services.UserToken;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
                services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
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
                var jwtConfig = Configuration.GetSection("Jwt");
                //生成密钥
                var symmetricKeyAsBase64 = jwtConfig.GetValue<string>("Secret");
                var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
                var signingKey = new SymmetricSecurityKey(keyByteArray);
                //认证参数
                services.AddAuthentication("Bearer")
                    .AddJwtBearer(o =>
                    {
                        o.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,//是否验证签名,不验证的画可以篡改数据，不安全
                            IssuerSigningKey = signingKey,//解密的密钥
                            ValidateIssuer = true,//是否验证发行人，就是验证载荷中的Iss是否对应ValidIssuer参数
                            ValidIssuer = jwtConfig.GetValue<string>("Iss"),//发行人
                            ValidateAudience = true,//是否验证订阅人，就是验证载荷中的Aud是否对应ValidAudience参数
                            ValidAudience = jwtConfig.GetValue<string>("Aud"),//订阅人
                            ValidateLifetime = true,//是否验证过期时间，过期了就拒绝访问
                            ClockSkew = TimeSpan.Zero,//这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，你可以直接设置为0
                            RequireExpirationTime = true,
                        };
                        o.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = context =>
                            {   //令牌过期
                                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                                {
                                    context.Response.Headers.Add("act", "expired");
                                }
                                return Task.CompletedTask;
                            }
                        };
                    });
                #endregion
                //services.AddSingleton(new JwtHelper());
                services.AddCors();
                services.AddControllers();

                // configure strongly typed settings object


                // configure DI for application services
                services.AddScoped<IUserService, UserService>();
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
            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();
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
