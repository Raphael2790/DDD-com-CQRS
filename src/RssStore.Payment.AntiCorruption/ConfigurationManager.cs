using RssStore.Payment.AntiCorruption.Interfaces;
using System;
using System.Linq;

namespace RssStore.Payment.AntiCorruption
{
    public class ConfigurationManager : IConfigurationManager
    {
        public string GetValue(string node)
        {
            return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890", 10)
                .Select(x => x[new Random().Next(x.Length)]).ToArray());
        }
    }
}
