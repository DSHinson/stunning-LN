using LexisNexis.Common.Filters;

namespace LexisNexis.DAL.Models
{
   public record Category : EntityBase<int>
    {
        [SearchWeight(3.0)]
        public string Name { get; set; }

        [SearchWeight(0.5)]
        public string Description { get; set; }

        public int? ParentCategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
