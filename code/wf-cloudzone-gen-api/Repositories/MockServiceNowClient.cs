using wf_cloudzone_gen_api.Interfaces;
using wf_cloudzone_gen_api.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace wf_cloudzone_gen_api.Repositories
{

    public class MockServiceNowClient : IMockServiceNowClient
    {
        private readonly string _mockFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "sample_documents.json");
        private readonly HttpClient _httpClient;

        public MockServiceNowClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Incident>> GetIncidentsAsync(bool useLiveServiceNow = false, bool useJira = false)
        {
            if (useLiveServiceNow)
            {
                return await FetchFromServiceNow();
            }
            else if (useJira)
            {
                return await FetchFromJira();
            }
            else
            {
                return await ReadMockJson();
            }
        }

        private async Task<List<Incident>> ReadMockJson()
        {
            if (!File.Exists(_mockFilePath))
            {
                return new List<Incident>(); // Return empty list if file is missing
            }

            try
            {
                var jsonData = await File.ReadAllTextAsync(_mockFilePath);
                return JsonSerializer.Deserialize<List<Incident>>(jsonData) ?? new List<Incident>();
            }
            catch (JsonException)
            {
                return new List<Incident>(); // Handle invalid JSON format
            }
        }

        private async Task<List<Incident>> FetchFromServiceNow()
        {
            var response = await _httpClient.GetAsync("https://your-servicenow-instance/api/incidents");
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Incident>>(result) ?? new List<Incident>();
        }

        private async Task<List<Incident>> FetchFromJira()
        {
            var response = await _httpClient.GetAsync("https://your-jira-instance/api/incidents");
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Incident>>(result) ?? new List<Incident>();
        }
       
    }
}
