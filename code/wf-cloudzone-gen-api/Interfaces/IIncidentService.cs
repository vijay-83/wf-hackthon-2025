using wf_cloudzone_gen_api.Models;

namespace wf_cloudzone_gen_api.Interfaces
{
    public interface IIncidentService
    {
        Task<List<Incident>> SearchIncidents(string searchTerm);
        Task<List<Incident>> GetPaginatedIncidentsAsync(int page, int pageSize);
    }
}
