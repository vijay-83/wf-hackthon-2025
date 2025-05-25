using wf_cloudzone_gen_api.Models;

namespace wf_cloudzone_gen_api.Interfaces
{
    public interface IMockServiceNowClient
    {
        Task<List<Incident>> GetIncidentsAsync(bool useLiveServiceNow = false, bool useJira = false);
    }
}
