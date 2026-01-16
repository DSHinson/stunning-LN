using LexisNexis.API.Helpers;
using LexisNexis.API.Middleware;
using LexisNexis.API.Products;
using LexisNexis.Common.CQRS;
using LexisNexis.DAL;
using LexisNexis.DAL.Models;
using LexisNexis.DAL.Storage;
using LexisNexis.DAL.Storage.EF;
using LexisNexis.DAL.Storage.InMemory;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace LexisNexis.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCqrs(typeof(LexisNexis.BLL.AssemblyMarkerForBll).Assembly);

            bool useEF = true;
            if (useEF)
            {
                builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("LexisNexisDb"));
                builder.Services.AddScoped(typeof(IReadRepository<,>), typeof(EfReadRepository<,>));
                builder.Services.AddScoped(typeof(IWriteRepository<,>), typeof(EfWriteRepository<,>));
            }
            else
            {
                builder.Services.AddTransient<IIdGenerator<int>, IntIdGenerator>();
                builder.Services.AddSingleton<ConcurrentDictionary<int, Product>>();
                builder.Services.AddSingleton<InMemoryRepository<Product, int>>();

                builder.Services.AddSingleton<IReadRepository<Product, int>>(sp => sp.GetRequiredService<InMemoryRepository<Product, int>>());
                builder.Services.AddSingleton<IWriteRepository<Product, int>>(sp => sp.GetRequiredService<InMemoryRepository<Product, int>>());
            }

            var app = builder.Build();
            app.UseMiddleware<RequestContextMiddleware>();
            app.MapProductsApi();

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
