using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace OSK.Serialization.Json.SystemTextJson.Internal
{
    internal static class JsonUtilsHelper
    {
        public static void SetDefaultOptions(JsonSerializerOptions options)
        {
            options.WriteIndented = true;
            options.PropertyNameCaseInsensitive = true;
            options.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
        }

        public static JsonSerializerOptions CreateDefaultOptions()
        {
            var options = new JsonSerializerOptions();
            SetDefaultOptions(options);

            return options;
        }
    }
}
