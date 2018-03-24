using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

namespace UnityDocsBot.Services
{
    public class DocLookupService
    {
        readonly string lookupFile = "lookup.json";

        public DocLookupService()
        {
            if (!File.Exists(lookupFile))
                File.WriteAllText(lookupFile, "{ }");
        }

        public bool TryGet(string name, out string[] docs)
        {
            docs = new string[0];

            var json = JObject.Parse(File.ReadAllText(lookupFile));

            if (json.TryGetValue(name, out var token))
            {
                docs = ((JArray)token).Select(t => (string)t).ToArray();
                return docs.Length > 0;
            }
            else
            {
                json.Add(name, new JArray());
                File.WriteAllText(lookupFile, json.ToString(Formatting.Indented));
            }

            return false;
        }

        public List<string> GetAllTags()
        {
            var json = JObject.Parse(File.ReadAllText(lookupFile));
            return json.ToObject<Dictionary<string, string[]>>().Where(x => x.Value.Length > 0).Select(x => x.Key).ToList();
        }
    }
}
