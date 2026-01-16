using LexisNexis.Common.DTO;
using LexisNexis.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.BLL.Categories
{
    public static class CategoryTreeBuilder
    {
        /// <summary>
        /// Builds a hierarchical tree of categories from a flat list.
        /// </summary>
        public static IReadOnlyList<CategoryNodeDto> BuildTree(IEnumerable<Category> categories)
        {
            // Group categories by ParentCategoryId for quick lookup
            var lookup = categories.GroupBy(c => c.ParentCategoryId ?? 0).ToDictionary(g => g.Key, g => g.ToList());

            // Recursive function to build a node and its children
            CategoryNodeDto BuildNode(Category category)
            {
                var children = lookup.TryGetValue(category.Id, out var childList)
                    ? childList.Select(BuildNode).ToList()
                    : new List<CategoryNodeDto>();

                return new CategoryNodeDto(
                    category.Id,
                    category.Name,
                    category.Description,
                    children
                );
            }

            // Start from root nodes (ParentCategoryId == null)
            return lookup.TryGetValue(0, out var roots) ? roots.Select(BuildNode).ToList() : Array.Empty<CategoryNodeDto>();
        }
    }

}
