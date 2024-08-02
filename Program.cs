using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using ShopBackgroundSystem.Helpers;
using ShopBackgroundSystem.Models;
using ShopBackgroundSystem.Services.Implementations;
using ShopBackgroundSystem.Services.Interfaces;
using ShopServerSystem.Helpers;
using ShopServerSystem.Services.Implementation;

namespace ShopBackgroundSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<CustomerDbContext>(
                options => options.UseMySql
                (
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 25))
                )
            );
            builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection(nameof(AuthSettings)));
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPictureService, PictureService>();
            builder.Services.AddSingleton(typeof(RSAHelper));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                //API������ʽ
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "ʹ��Bearer������jwt��Ȩ��ͷ",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
                //APIȫ�ְ�ȫҪ��
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });
            builder.Services.AddCors(
                option => option.AddDefaultPolicy(
                    p => p.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("https://localhost:7145/", "http://localhost:8080/")
                    )
                );
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors();
            app.UseMiddleware<JwtMiddleWare>();
            app.UseAuthorization();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "Uploads")),
                RequestPath = "/uploads"  //��������·��
            });

            app.MapControllers();

            app.Run();
        }
    }
}
