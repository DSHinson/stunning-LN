using LexisNexis.DAL.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LexisNexis.API.Helpers
{
    public sealed class ProductJsonConverter : JsonConverter<Product>
    {
        public override Product? Read( ref Utf8JsonReader reader,Type typeToConvert,JsonSerializerOptions options)
        {
            throw new NotSupportedException("Deserialization is not supported.");
        }

        public override void Write(Utf8JsonWriter writer,Product value,JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteNumber("Id", value.Id);
            writer.WriteString("Name", value.Name);
            writer.WriteString("Description", value.Description);
            writer.WriteString("SKU", value.SKU);
            writer.WriteString("CategoryId", value.CategoryId.ToString());
            writer.WriteNumber("Price", value.Price);
            writer.WriteString("Quantity", value.Quantity.ToString());
            writer.WriteString("CreatedAt", value.CreatedAt);
            writer.WriteString("UpdatedAt", value.UpdatedAt);

            writer.WriteEndObject();
        }
    }

}
