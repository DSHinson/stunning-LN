using LexisNexis.Common.Filters;

namespace LexisNexis.DAL.Models
{
    public record Product : EntityBase<int> , IComparable<Product>
    {

        [SearchWeight(3.0)]
        public required string Name { get; set; }

        public int CategoryId { get; set; }

        [SearchWeight(1.5)]
        public string Description { get; set; }

        [SearchWeight(2.0)]
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Default ordering for products (used by Sort / OrderBy)
        public int CompareTo(Product? other)
        {
            if (other is null)
            {
                return 1;
            }

            // Primary: Name (case-insensitive)
            int nameComparison = string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
            

            if (nameComparison != 0)
            {
                return nameComparison;
            }

            // Secondary: Price
            int priceComparison = Price.CompareTo(other.Price);
            if (priceComparison != 0)
            {
                return priceComparison;
            }

            // Final tie-breaker: Id 
            return Id.CompareTo(other.Id);
        }

    }

    
}
