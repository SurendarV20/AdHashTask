using EBay.API.Configuration;
using EBay.Infrastructure;
using OfficeOpenXml;
namespace EBay.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Add services to the container.
            builder.Services.AddStackExchangeRedisCache(o =>
            {
                o.InstanceName = "EBayRedis";
                o.Configuration = "localhost:6379";
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("DefaultConnection"));
            builder.Services.AddServices();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin());
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
