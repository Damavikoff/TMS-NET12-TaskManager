using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Utils
{
    public class JsonHandler
    {
        public static string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static T? Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }

    public class DateTimeFormatConverter : JsonConverter<DateTime>
    {
        private readonly string format;

        public DateTimeFormatConverter(string format)
        {
            this.format = format;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(
                reader.GetString() ?? String.Empty,
                this.format,
                CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer, nameof(writer));

            writer.WriteStringValue(value
                .ToUniversalTime()
                .ToString(
                    this.format,
                    CultureInfo.InvariantCulture));
        }
    }

    public sealed class JsonDateTimeFormat : JsonConverterAttribute
    {
        private readonly string format;

        public JsonDateTimeFormat(string format)
        {
            this.format = format;
        }

        public string Format => this.format;

        public override JsonConverter? CreateConverter(Type typeToConvert)
        {
            return new DateTimeFormatConverter(this.format);
        }
    }
}
