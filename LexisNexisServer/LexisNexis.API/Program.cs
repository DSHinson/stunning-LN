using LexisNexis.API.Products;
using LexisNexis.Common.CQRS;
using LexisNexis.DAL.Models;
using LexisNexis.DAL.Storage;
using LexisNexis.DAL.Storage.InMemory;
using System.Collections.Concurrent;

namespace LexisNexis.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //TODO: change this to use reflection -> assembly scanning? or perhaps a marker interface on BLL project?
            builder.Services.AddCqrs(typeof(LexisNexis.BLL.Weather.GetWeatherForecastEvent).Assembly);
            builder.Services.AddTransient<IIdGenerator<int>, IntIdGenerator>();
            builder.Services.AddSingleton<ConcurrentDictionary<int,Product>>();
            builder.Services.AddSingleton<InMemoryRepository<Product, int>>();

            builder.Services.AddSingleton<IReadRepository<Product, int>>(sp => sp.GetRequiredService<InMemoryRepository<Product, int>>());
            builder.Services.AddSingleton<IWriteRepository<Product, int>>(sp => sp.GetRequiredService<InMemoryRepository<Product, int>>());

            var app = builder.Build();

            app.MapGetProducts();
            app.MapPostProduct();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
