using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityDocsBot.Models;

namespace UnityDocsBot.Services
{
    public class DocsMasterUnityRetrievalService
    {
        private readonly HttpClient _client;

        public DocsMasterUnityRetrievalService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ManualEntryModel> GetManualEntryFromLatest(string entryName)
        {
            var result = await _client.GetAsync($"http://docsmaster.net/ManualEntry/UnityDocs/{entryName}");
            return !result.IsSuccessStatusCode ? null : JsonConvert.DeserializeObject<ManualEntryModel>(await result.Content.ReadAsStringAsync());
        }

        public async Task<ManualEntryModel> GetManualEntryFromVersion(string entryName, string version)
        {
            var result = await _client.GetAsync($"http://docsmaster.net/ManualEntry/UnityDocs/{entryName}/{version}");
            return !result.IsSuccessStatusCode ? null : JsonConvert.DeserializeObject<ManualEntryModel>(await result.Content.ReadAsStringAsync());
        }

        public async Task<IEnumerable<string>> GetAllVersionsAsync()
        {
            var result = await _client.GetAsync($"http://docsmaster.net/ManualEntry/UnityDocs");
            return JsonConvert.DeserializeObject<List<string>>(await result.Content.ReadAsStringAsync());
        }
    }
}