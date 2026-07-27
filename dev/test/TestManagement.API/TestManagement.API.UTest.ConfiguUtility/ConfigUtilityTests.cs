using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Xunit;
using TestManagement.API.Infrastructure.Configuration;

namespace TestManagement.API.Tests
{
    public class ConfigUtilityTests
    {
        [Theory]
        [InlineData("MyKey", "MyValue")]
        [InlineData("a", "a")]
        [InlineData("z", "z")]
        [InlineData("A", "A")]
        [InlineData("Z", "Z")]
        public void GetValue_KeyExists_ReturnsValue(string key, string val)
        {
            var dict = new Dictionary<string, string?> { [key] = val };
            IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
            ConfigUtility configUtility = new ConfigUtility(config);

            var result = configUtility.GetValue(key);

            Assert.Equal(val, result);
        }

        [Theory]
        [InlineData("MyKey", "MyValue")]
        [InlineData("a", "a")]
        [InlineData("z", "z")]
        [InlineData("A", "A")]
        [InlineData("Z", "Z")]
        public void GetValue_KeyExists_RequiredTrue_ReturnsValue(string key, string val)
        {
            var dict = new Dictionary<string, string?> { [key] = val };
            IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
            ConfigUtility configUtility = new ConfigUtility(config);

            var result = configUtility.GetValue(key, required: true);

            Assert.Equal(val, result);
        }

        [Theory]
        [InlineData("MyKey", "MyValue")]
        [InlineData("a", "a")]
        [InlineData("z", "z")]
        [InlineData("A", "A")]
        [InlineData("Z", "Z")]
        public void GetValue_KeyExists_RequiredFalse_ReturnsValue(string key, string val)
        {
            var dict = new Dictionary<string, string?> { [key] = val };
            IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
            ConfigUtility configUtility = new ConfigUtility(config);

            var result = configUtility.GetValue(key, required: false);

            Assert.Equal(val, result);
        }

        [Fact]
        public void GetValue_KeyMissing_RequiredFalse_ReturnsNull()
        {
            var dict = new Dictionary<string, string?>();
            IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
            ConfigUtility configUtility = new ConfigUtility(config);

            var result = configUtility.GetValue("MissingKey", required: false);

            Assert.Null(result);
        }

        [Theory]
        [InlineData("MissginKey")]
        [InlineData("")]
        public void GetValue_KeyMissing_RequiredTrue_ThrowsInvalidOperationException(string key)
        {
            var dict = new Dictionary<string, string?>();
            IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
            ConfigUtility configUtility = new ConfigUtility(config);

            var ex = Assert.Throws<InvalidOperationException>(() => configUtility.GetValue(key, required: true));

            Assert.Contains(key, ex.Message);
        }
    }
}
