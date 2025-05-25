using wf_cloudzone_gen_api.Interfaces;
using wf_cloudzone_gen_api.LangChainIntegration;
using wf_cloudzone_gen_api.Models;
using wf_cloudzone_gen_api.Repositories;

namespace wf_cloudzone_gen_api.Services
{
    public class IncidentService : IIncidentService
    {
        private readonly IQdrantRepository _qdrantRepository;
        private readonly IMockServiceNowClient _mockServiceNowClient;
        public IncidentService(IQdrantRepository qdrantRepository, IMockServiceNowClient mockServiceNowClient)
        {
            _qdrantRepository = qdrantRepository;
            _mockServiceNowClient = mockServiceNowClient;
        }
        public async Task<List<Incident>> SearchIncidents(string searchTerm)
        {
            var incidents = await _mockServiceNowClient.GetIncidentsAsync();

            if (!incidents.Any())
            {
                incidents = await _qdrantRepository.SearchIncidentVectors(searchTerm);
            }

            return incidents.Where(i => i.Description.Contains(searchTerm)).ToList();
        }

        public async Task<List<Incident>> GetPaginatedIncidentsAsync(int page, int pageSize)
        {
            var incidents = await _mockServiceNowClient.GetIncidentsAsync();
            return incidents.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

    }
}
