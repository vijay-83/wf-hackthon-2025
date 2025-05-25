using wf_cloudzone_gen_api.Models;

namespace wf_cloudzone_gen_api.Interfaces
{
    public interface IQdrantRepository
    {
        Task StoreIncidentVector(Incident incident);
        Task<List<Incident>> SearchIncidentVectors(string query);
    }
}
