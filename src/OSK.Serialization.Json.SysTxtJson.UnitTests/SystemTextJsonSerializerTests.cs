using OSK.Serialization.Json.SystemTextJson;
using OSK.Serialization.Json.SysTxtJson.UnitTests.Helpers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace OSK.Serialization.Json.SysTxtJson.UnitTests
{
    public class SystemTextJsonSerializerTests : SerializerTests
    {
        #region Constructors

        public SystemTextJsonSerializerTests()
            : base(new SystemTextJsonSerializer(CreateOptions()))
        {
        }

        #endregion

        #region SerializerTests Overrides

        protected override Task<DeserializationTestParameters> GetDeserializationTestParametersAsync()
        {
            var jsonData = "{\"Name\":\"HelloWorld\",\"Data\":[{\"Index\":117,\"Data\":{\"A\":42,\"B\":\"ABC\",\"C\":null}}]}";

            return Task.FromResult(new DeserializationTestParameters()
            {
                Data = Encoding.UTF8.GetBytes(jsonData),
                ExpectedResult = new TestMessage()
                {
                    Name = "HelloWorld",
                    Data = new List<TestData>()
                    {
                        new TestData()
                        {
                            Index = 117,
                            Data = new TestClass()
                            {
                                A = 42,
                                B = "ABC",
                                C = null
                            }
                        }
                    }
                }
            });
        }

        #endregion

        #region Helpers

        private static JsonSerializerOptions CreateOptions()
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            };

            options.Converters.Add(new CustomJsonConverter());
            options.MakeReadOnly();
            return options;
        }

        #endregion
    }
}
