namespace LexisNexis.DAL.Models
{
   public record Category : EntityBase
    {
        public string Name { get; set; } = "";
        public int? ParentId { get; set; }
    }
}
