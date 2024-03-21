using OSK.Serialization.Abstractions.Json;
using OSK.Serialization.Json.SystemTextJson.Internal;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OSK.Serialization.Json.SystemTextJson
{
    public class SystemTextJsonSerializer : IJsonSerializer
    {
        #region Variables

        public static JsonSerializerOptions DefaultOptions { get; private set; } = JsonUtilsHelper.CreateDefaultOptions();

        private readonly JsonSerializerOptions _options;

        #endregion

        #region Constructors

        public SystemTextJsonSerializer()
            : this(DefaultOptions) { }

        public SystemTextJsonSerializer(JsonSerializerOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        #endregion

        #region IJsonSerializer

        public ValueTask<object> DeserializeAsync(byte[] data, Type type, CancellationToken cancellationToken = default)
        {
            using var memoryStream = new MemoryStream(data);
            return JsonSerializer.DeserializeAsync(memoryStream, _options.GetTypeInfo(type), cancellationToken);
        }

        public async ValueTask<byte[]> SerializeAsync(object data, CancellationToken cancellationToken = default)
        {
            using var memoryStream = new MemoryStream();
            var typeInfo = _options.GetTypeInfo(data.GetType());
            await JsonSerializer.SerializeAsync(memoryStream, data, typeInfo, cancellationToken);

            return memoryStream.ToArray();
        }

        #endregion
    }
}
