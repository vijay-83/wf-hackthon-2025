namespace wf_cloudzone_gen_api.LangChainIntegration.Summary
{
    public interface ISummaryAgent
    {
        Task<SummaryResponse> GenerateSummaryAsync(SummaryRequest request);
    }
}
