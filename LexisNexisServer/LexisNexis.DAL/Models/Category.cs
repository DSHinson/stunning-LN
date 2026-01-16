namespace LexisNexis.DAL.Models
{
   public record Category : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
