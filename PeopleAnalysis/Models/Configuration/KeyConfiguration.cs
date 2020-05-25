using System.Collections.Generic;
using System.Linq;

namespace PeopleAnalysis.Models.Configuration
{
    public class KeysConfiguration
    {
        public KeyConfiguration[] Keys { get; set; }
        private Dictionary<string, KeyConfiguration> _keys;
        public KeyConfiguration this[string name] => (_keys ?? (_keys = Keys.ToDictionary(x => x.Name)))[name];
    }

    public class KeyConfiguration
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Confirmation { get; set; }
    }
}
