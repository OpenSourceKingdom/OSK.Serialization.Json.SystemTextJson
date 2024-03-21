using System.Text.Json;
using System.Text.Json.Serialization;

namespace OSK.Serialization.Json.SysTxtJson.UnitTests.Helpers
{
    public class CustomJsonConverter : JsonConverter<object>
    {
        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var converter = options.GetConverter(typeof(TestClass)) as JsonConverter<TestClass>;
            return converter.Read(ref reader, typeof(TestClass), options);
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            var converter = options.GetConverter(typeof(TestClass)) as JsonConverter<TestClass>;
            converter.Write(writer, value as TestClass, options);
        }
    }
}
