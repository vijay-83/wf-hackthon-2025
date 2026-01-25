namespace wf_cloudzone_gen_api.LangChainIntegration
{
    public interface ILangChainClient
    {
        Task<string> GetSummaryAsync(string prompt, List<string> context);
        //Task<List<string>> GetSuggestions(string text);
        //Task<string> GetBugFix(string text);
    }
}
