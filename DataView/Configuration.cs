using Microsoft.Extensions.Configuration;

namespace DataView
{
    public class Configuration 
    {
        public IConfigurationRoot configuration;

        public Configuration() 
        {
            configuration = new ConfigurationBuilder().AddJsonFile("../../config/config.json", optional: true).Build();
        }
    }
}
