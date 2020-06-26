using TQ.Geocoding.Service.Config;

namespace TQ.Geocoding.Api.Test
{
    public static class TestHelper
    {
        public static ConfigurationSettings GetConfigurationSettings()
        {
            return new ConfigurationSettings()
            {
                MaxRowCount = 100
            };
        }
    }
}
