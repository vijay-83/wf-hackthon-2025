using wf_cloudzone_gen_api.Interfaces;
using wf_cloudzone_gen_api.LangChainIntegration;
using wf_cloudzone_gen_api.Models;
using wf_cloudzone_gen_api.Repositories;

namespace wf_cloudzone_gen_api.Services
{
    public class IncidentService : IIncidentService
    {
        private readonly IMockServiceNowClient _mockServiceNowClient;
        public IncidentService( IMockServiceNowClient mockServiceNowClient)
        {
            _mockServiceNowClient = mockServiceNowClient;
        }
        public async Task<List<Incident>> GetIncidentsByCountAsync(int itemCount)
        {
            var incidents = await _mockServiceNowClient.GetIncidentsAsync();
            return incidents.Take(itemCount).ToList();
        }


    }
}
