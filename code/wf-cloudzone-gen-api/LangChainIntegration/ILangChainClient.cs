namespace wf_cloudzone_gen_api.LangChainIntegration
{
    public interface ILangChainClient
    {
        Task<string> GetSummary(string text);
        Task<List<string>> GetSuggestions(string text);
        Task<string> GetBugFix(string text);
    }
}
