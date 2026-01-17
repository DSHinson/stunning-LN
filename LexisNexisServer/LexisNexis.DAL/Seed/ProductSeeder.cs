using LexisNexis.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.DAL.Seed
{
    public static class ProductSeeder
    {
        public static List<Product> SeedProducts(List<Category> categories)
        {
            // Helper to get a category id by name
            int GetCategoryId(string name) => categories.First(c => c.Name == name).Id;

            return new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Gaming Laptop",
                    Description = "High performance laptop for gaming",
                    SKU = "LPT-001",
                    Price = 1999.99m,
                    Quantity = 10,
                    CategoryId = GetCategoryId("Laptops"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 2,
                    Name = "Office Laptop",
                    Description = "Reliable laptop for office use",
                    SKU = "LPT-002",
                    Price = 999.99m,
                    Quantity = 25,
                    CategoryId = GetCategoryId("Laptops"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 3,
                    Name = "Smartphone X",
                    Description = "Latest smartphone model",
                    SKU = "PHN-001",
                    Price = 799.99m,
                    Quantity = 50,
                    CategoryId = GetCategoryId("Phones"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 4,
                    Name = "Ergonomic Chair",
                    Description = "Comfortable office chair",
                    SKU = "CHR-001",
                    Price = 199.99m,
                    Quantity = 15,
                    CategoryId = GetCategoryId("Chairs"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 5,
                    Name = "Wooden Desk",
                    Description = "Spacious office desk",
                    SKU = "DSK-001",
                    Price = 299.99m,
                    Quantity = 12,
                    CategoryId = GetCategoryId("Desks"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 6,
                    Name = "Microwave Oven",
                    Description = "Compact microwave for home use",
                    SKU = "HAP-001",
                    Price = 99.99m,
                    Quantity = 20,
                    CategoryId = GetCategoryId("Home Appliances"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                         new Product
                {
                    Id = 7,
                    Name = "Smartphone Y",
                    Description = "Mid-range smartphone",
                    SKU = "PHN-002",
                    Price = 499.99m,
                    Quantity = 35,
                    CategoryId = GetCategoryId("Phones"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 8,
                    Name = "Ultrabook Laptop",
                    Description = "Lightweight business laptop",
                    SKU = "LPT-003",
                    Price = 1299.99m,
                    Quantity = 20,
                    CategoryId = GetCategoryId("Laptops"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 9,
                    Name = "Bluetooth Headphones",
                    Description = "Wireless over-ear headphones",
                    SKU = "HDP-001",
                    Price = 149.99m,
                    Quantity = 40,
                    CategoryId = GetCategoryId("Electronics"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 10,
                    Name = "LED Desk Lamp",
                    Description = "Adjustable desk lamp with LED light",
                    SKU = "LMP-001",
                    Price = 49.99m,
                    Quantity = 50,
                    CategoryId = GetCategoryId("Desks"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 11,
                    Name = "Standing Desk",
                    Description = "Height adjustable standing desk",
                    SKU = "DSK-002",
                    Price = 399.99m,
                    Quantity = 10,
                    CategoryId = GetCategoryId("Desks"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 12,
                    Name = "Office Chair Deluxe",
                    Description = "Ergonomic office chair with lumbar support",
                    SKU = "CHR-002",
                    Price = 249.99m,
                    Quantity = 15,
                    CategoryId = GetCategoryId("Chairs"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 13,
                    Name = "Microwave Oven XL",
                    Description = "Large microwave for family kitchens",
                    SKU = "HAP-002",
                    Price = 179.99m,
                    Quantity = 25,
                    CategoryId = GetCategoryId("Home Appliances"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 14,
                    Name = "Refrigerator",
                    Description = "Double-door refrigerator",
                    SKU = "HAP-003",
                    Price = 899.99m,
                    Quantity = 8,
                    CategoryId = GetCategoryId("Home Appliances"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 15,
                    Name = "Wooden Bookshelf",
                    Description = "Classic wooden bookshelf",
                    SKU = "BKS-001",
                    Price = 149.99m,
                    Quantity = 12,
                    CategoryId = GetCategoryId("Books"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 16,
                    Name = "Notebook Set",
                    Description = "Pack of 5 ruled notebooks",
                    SKU = "BKS-002",
                    Price = 24.99m,
                    Quantity = 100,
                    CategoryId = GetCategoryId("Books"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
        }
    }

}
