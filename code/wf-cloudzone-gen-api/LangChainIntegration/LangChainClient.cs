using Newtonsoft.Json;
using System.Text;
using wf_cloudzone_gen_api.LangChainIntegration.Summary;

namespace wf_cloudzone_gen_api.LangChainIntegration
{
    public class LangChainClient : ILangChainClient
    {
        private readonly HttpClient _httpClient;

        public LangChainClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetSummaryAsync(string prompt, List<string> context)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/langchain/summary", new
            {
                prompt,
                context
            });
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<SummaryResponse>();
            return result?.Summary ?? string.Empty;
        }
    }
}
