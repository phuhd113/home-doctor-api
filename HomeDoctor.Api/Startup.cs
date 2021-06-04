using System;
using System.IO;
using System.Reflection;
using System.Text;
using HomeDoctor.Business.IService;
using HomeDoctor.Business.Quartz;
using HomeDoctor.Business.Quartz.Jobs;
using HomeDoctor.Business.Service;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Data.DBContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace HomeDoctor.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var sqlConnectionString = Configuration.GetConnectionString("MySqlConnection");
            services.AddDbContext<HomeDoctorContext>(
                options =>
              options.UseSqlServer(sqlConnectionString)
          );
            services.AddControllers();
            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin()
                                             .AllowAnyHeader()
                                             .AllowAnyMethod();
                                  });
            });
            services.AddSignalR();
            services.AddHostedService<QuartzHostedService>();
            services.AddSwaggerGen(swagger =>
           {
               //This is to generate the Default UI of Swagger Documentation  
               swagger.SwaggerDoc("v1", new OpenApiInfo
               {
                   Version = "v1",
                   Title = "Home Doctor API",
                   Description = "ASP.NET Core 3.1 Web API"
               });
               // Set the comments path for the Swagger JSON and UI.
               var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
               var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
               swagger.IncludeXmlComments(xmlPath);
               // To Enable authorization using Swagger (JWT)  
               swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
               {
                   Name = "Authorization",
                   Type = SecuritySchemeType.ApiKey,
                   Scheme = "Bearer",
                   BearerFormat = "JWT",
                   In = ParameterLocation.Header,
                   Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
               });
               swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
           });
            // register authentication jst
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])) //Configuration["JwtToken:SecretKey"]  
                };
            });
            

            // register dependecy injection
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<ILicenseService, LicenseService>();
            services.AddScoped<IDiseaseService, DiseaseService>();
            services.AddScoped<IMedicalInstructionService, MedicalInstructionService>();
            services.AddScoped<IMedicalInstructionTypeService, MedicalInstructionTypeService>();
            services.AddScoped<IHealthRecordService, HealthRecordService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IFirebaseFCMService, FirebaseFCMService>();           
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IPersonalHealthRecordService, PersonalHealthRecordService>();
            services.AddScoped<IActionService, ActionService>();
            services.AddScoped<IVitalSignService, VitalSignService>();
            services.AddScoped<ISMSMessageService, SMSMessageService>();
            services.AddScoped<ITimeService, TimeService>();
            services.AddScoped<IPaymentService, PaymentService>();           
            // add Quartz serivce
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            /*
            //add Job
            services.AddSingleton<HelloWordJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(HelloWordJob),
                cronExpression: "0 18 1 ? * *")); // run every 5 seconds                                 
            services.AddSingleton<Action8AMJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(Action8AMJob),
                cronExpression: "0 58 02 ? * *"));
            */
            services.AddSingleton<Action00AMJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(Action00AMJob),
                cronExpression: "3 0 0 ? * *"));

            services.AddSingleton<Action06AMJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(Action06AMJob),
                cronExpression: "0 0 6 ? * *"));

            //IronOcr.Installation.LicenseKey = "3C2C3B9A-DC89-41DD-985E-33999472D889";
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });                   
            //configure swagger 
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

            });
        }
    }
}
