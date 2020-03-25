using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;

namespace DataView
{
    public class Configuration 
    {

        public IConfigurationRoot configuration;

        public Configuration() 
        {
            configuration = new ConfigurationBuilder().AddJsonFile("config.json", optional: true).Build();
            
        }
    }
}
