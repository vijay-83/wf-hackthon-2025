using wf_cloudzone_gen_api.Interfaces;
using wf_cloudzone_gen_api.LangChainIntegration;

namespace wf_cloudzone_gen_api.Services
{
    public class AgentService : IAgentService
    {
        private readonly ILangChainClient _langChainClient;
        public AgentService(ILangChainClient langChainClient)
        {
            _langChainClient = langChainClient;
        }
        public async Task<string> GenerateSummary(string text)
        {
            return await _langChainClient.GetSummary(text);
        }
        public async Task<List<string>> GetSuggestions(string text)
        {
            return await _langChainClient.GetSuggestions(text);
        }
        public async Task<string> GetBugFix(string text)
        {
            return await _langChainClient.GetBugFix(text);
        }
    }
}
