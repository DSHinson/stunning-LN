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

            writer.WriteNumber("id", value.Id);
            writer.WriteString("name", value.Name);
            writer.WriteNumber("price", value.Price);
            writer.WriteBoolean("inStock", value.Quantity > 0);

            writer.WriteStartObject("audit");
            writer.WriteString("createdAt", value.CreatedAt);
            writer.WriteString("updatedAt", value.UpdatedAt);
            writer.WriteEndObject();

            writer.WriteEndObject();
        }
    }

}
