using Newtonsoft.Json;
using System.Text;
using wf_cloudzone_gen_api.Interfaces;
using wf_cloudzone_gen_api.Models;

namespace wf_cloudzone_gen_api.Repositories
{
    public class QdrantRepository : IQdrantRepository
    {
        private readonly HttpClient _httpClient;
        public QdrantRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task StoreIncidentVector(Incident incident)
        {
            var requestBody = new
            {
                id = incident.Id,
                summary = incident.Summary,
                description = incident.Description,
                priority = incident.Priority,
                status = incident.Status,
                labels = incident.Labels,
                assignee = incident.Assignee,
                payload = incident.Payload // Stores additional key-value pairs
            };

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("https://your-qdrant-instance/vectors", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to store incident vector: {response.StatusCode}");
            }
        }
        public async Task<List<Incident>> SearchIncidentVectors(string query)
        {
            var response = await _httpClient.PostAsync("https://your-qdrant-instance/search",
                new StringContent(JsonConvert.SerializeObject(new { query }),
                                  Encoding.UTF8,
                                  "application/json"));
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Incident>>(result);
        }
    }
}
