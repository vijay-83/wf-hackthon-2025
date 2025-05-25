namespace wf_cloudzone_gen_api.Interfaces
{
    public interface IAgentService
    {
        Task<string> GenerateSummary(string text);
        Task<List<string>> GetSuggestions(string text);
        Task<string> GetBugFix(string text);
    }

}
