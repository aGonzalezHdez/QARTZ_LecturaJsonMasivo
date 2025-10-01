using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LibreriaClasesAPIExpertti.Utilities.Converters
{
    public class DataSetConverter : JsonConverter<DataSet>
    {
        public override DataSet Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, DataSet value,
            JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (DataTable table in value.Tables)
            {
                writer.WritePropertyName(table.TableName);
                JsonSerializer.Serialize(writer, table, options);
            }
            writer.WriteEndObject();
        }
    }
}
