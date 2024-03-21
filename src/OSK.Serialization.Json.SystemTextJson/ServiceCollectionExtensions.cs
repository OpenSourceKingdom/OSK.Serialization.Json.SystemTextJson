using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using OSK.Serialization.Abstractions.Json;
using OSK.Serialization.Json.SystemTextJson.Internal;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OSK.Serialization.Json.SystemTextJson
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSystemTextJsonSerialization(this IServiceCollection services)
            => services.AddSystemTextJsonSerialization(JsonUtilsHelper.SetDefaultOptions);

        /// <summary>
        /// Adds a serializer that utilize System.Text.Json as the backing mechanism
        /// </summary>
        /// <param name="services">The services to add the serializer to</param>
        /// <param name="options">The options to create the Json serializer with</param>
        /// <returns>The service collection for chaining</returns>
        /// <remarks>
        /// If the JsonSerializerOptions are set to ReadOnly in the action, then some additional converters may fail
        /// to be added to the options when they are initially retrieved. As this method will set the options to be
        /// readonly, it is not necessary to do so when configuring.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Throws if options are null</exception>
        public static IServiceCollection AddSystemTextJsonSerialization(this IServiceCollection services,
            Action<JsonSerializerOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            services.Configure(options);
            services.TryAddTransient<IJsonSerializer>(provider =>
            {
                var options = provider.GetService<IOptions<JsonSerializerOptions>>();
                if (!options.Value.IsReadOnly)
                {
                    var extraConverters = provider.GetRequiredService<IEnumerable<JsonConverter>>();
                    foreach (var converter in extraConverters)
                    {
                        options.Value.Converters.Add(converter);
                    }
                    options.Value.MakeReadOnly();
                }

                return new SystemTextJsonSerializer(options.Value);
            });

            return services;
        }
    }
}
