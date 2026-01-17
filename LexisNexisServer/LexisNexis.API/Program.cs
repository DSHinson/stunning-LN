using LexisNexis.API.Helpers;
using LexisNexis.API.Middleware;
using LexisNexis.API.Products;
using LexisNexis.BLL.SearchEngine;
using LexisNexis.Common.Cache;
using LexisNexis.Common.CQRS;
using LexisNexis.DAL;
using LexisNexis.DAL.Models;
using LexisNexis.DAL.Seed;
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
            builder.Services.AddMemoryCache();

            builder.Services.AddSingleton<ICacheService, CacheService>();
            builder.Services.AddCqrs(typeof(LexisNexis.BLL.AssemblyMarkerForBll).Assembly);

            builder.Services.AddScoped(typeof(ISearchEngine<,>), typeof(SearchEngine<,>));


            bool useEF = true;
            if (useEF)
            {
                builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("LexisNexisDb"));
                builder.Services.AddScoped(typeof(IReadRepository<,>), typeof(EfReadRepository<,>));
                builder.Services.AddScoped(typeof(IWriteRepository<,>), typeof(EfWriteRepository<,>));
            }
            else
            {
                //Id generator for in memory repos
                builder.Services.AddTransient<IIdGenerator<int>, IntIdGenerator>();

                //Product 
                builder.Services.AddSingleton<ConcurrentDictionary<int, Product>>();
                builder.Services.AddSingleton<InMemoryRepository<Product, int>>(); 
                builder.Services.AddSingleton<IReadRepository<Product, int>>(sp => sp.GetRequiredService<InMemoryRepository<Product, int>>());
                builder.Services.AddSingleton<IWriteRepository<Product, int>>(sp => sp.GetRequiredService<InMemoryRepository<Product, int>>());

                //Category
                builder.Services.AddSingleton<ConcurrentDictionary<int, Category>>();
                builder.Services.AddSingleton<InMemoryRepository<Category, int>>();
                builder.Services.AddSingleton<IReadRepository<Category, int>>(sp => sp.GetRequiredService<InMemoryRepository<Category, int>>());
                builder.Services.AddSingleton<IWriteRepository<Category, int>>(sp => sp.GetRequiredService<InMemoryRepository<Category, int>>());
            }

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var categoryRepo = scope.ServiceProvider.GetRequiredService<IWriteRepository<Category, int>>();
                var productRepo = scope.ServiceProvider.GetRequiredService<IWriteRepository<Product, int>>();

                var categories = CategorySeeder.SeedCategories();
                foreach (var category in categories)
                {
                    categoryRepo.AddAsync(category).ConfigureAwait(false);
                }

                var products = ProductSeeder.SeedProducts(categories);
                foreach (var product in products)
                {
                    productRepo.AddAsync(product).ConfigureAwait(false);
                }
            }


            app.UseMiddleware<RequestContextMiddleware>();
            app.MapProductsApi();
            app.MapCategoryApi();

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
