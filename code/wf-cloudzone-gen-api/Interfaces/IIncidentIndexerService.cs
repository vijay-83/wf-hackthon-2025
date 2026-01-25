using wf_cloudzone_gen_api.Models;

namespace wf_cloudzone_gen_api.Interfaces
{
    public interface IIncidentIndexerService
    {
        Task IndexIncidentAsync(Incident incident);
        Task IndexManyAsync(IEnumerable<Incident> incidents);
    }
}
