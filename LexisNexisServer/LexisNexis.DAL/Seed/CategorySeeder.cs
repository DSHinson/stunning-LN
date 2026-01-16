using LexisNexis.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.DAL.Seed
{
    public static class CategorySeeder
    {
        public static List<Category> SeedCategories()
        {
            return new List<Category>
        {
            new Category
            {
                Id = 1,
                Name = "Electronics",
                Description = "All electronic items",
                ParentCategoryId = null
            },
            new Category
            {
                Id = 2,
                Name = "Laptops",
                Description = "Portable computers",
                ParentCategoryId = 1
            },
            new Category
            {
                Id = 3,
                Name = "Phones",
                Description = "Smartphones and accessories",
                ParentCategoryId = 1
            },
            new Category
            {
                Id = 4,
                Name = "Home Appliances",
                Description = "Appliances for home use",
                ParentCategoryId = 1
            },
            new Category
            {
                Id = 5,
                Name = "Furniture",
                Description = "Home and office furniture",
                ParentCategoryId = null
            },
            new Category
            {
                Id = 6,
                Name = "Chairs",
                Description = "Seating furniture",
                ParentCategoryId = 5
            },
            new Category
            {
                Id = 7,
                Name = "Desks",
                Description = "Office desks",
                ParentCategoryId = 5
            },
            new Category
            {
                Id = 8,
                Name = "Books",
                Description = "Books and stationery",
                ParentCategoryId = null
            }
        };
        }
    }

}
