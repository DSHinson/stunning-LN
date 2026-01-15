using LexisNexis.Common.CQRS;
using LexisNexis.Common.CQRS.Command;
using LexisNexis.Common.Result;
using LexisNexis.DAL.Models;

namespace LexisNexis.BLL.Products
{
    [EventReplayBehaviorAttribute(EventReplayOptions.MutatesData)]
    public sealed class CreateProductEvent: ICommand<Result<Product>>
    {
        public required string Name { get; init; }
        public string? Description { get; init; }
        public string? SKU { get; init; }
        public decimal Price { get; init; }
        public int Quantity { get; init; }
        public int CategoryId { get; init; }
    }
}
