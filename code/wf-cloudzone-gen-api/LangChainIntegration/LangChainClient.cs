using Newtonsoft.Json;
using System.Text;

namespace wf_cloudzone_gen_api.LangChainIntegration
{
    public class LangChainClient : ILangChainClient
    {
        private readonly HttpClient _httpClient;

        public LangChainClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetSummary(string text)
        {
            var response = await _httpClient.PostAsync("https://your-langchain-api/summary",
                new StringContent(JsonConvert.SerializeObject(new { text }), Encoding.UTF8, "application/json"));

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<string>> GetSuggestions(string text)
        {
            var response = await _httpClient.PostAsync("https://your-langchain-api/suggestions",
                new StringContent(JsonConvert.SerializeObject(new { text }), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<string> GetBugFix(string text)
        {
            var response = await _httpClient.PostAsync("https://your-langchain-api/bugfix",
                new StringContent(JsonConvert.SerializeObject(new { text }), Encoding.UTF8, "application/json"));

            return await response.Content.ReadAsStringAsync();
        }
    }
}
