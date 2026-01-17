using LexisNexis.Common.Filters;

namespace LexisNexis.DAL.Models
{
   public record Category : EntityBase
    {
        [SearchWeight(3.0)]
        public string Name { get; set; }

        [SearchWeight(0.5)]
        public string Description { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}
