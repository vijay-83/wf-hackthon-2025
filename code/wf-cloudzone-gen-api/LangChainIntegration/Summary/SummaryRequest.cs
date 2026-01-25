namespace wf_cloudzone_gen_api.LangChainIntegration.Summary
{
    public class SummaryRequest
    {
        public required string Query { get; set; } // e.g., "Summarize incidents about AKS CPU"
        public int TopK { get; set; } = 5;
    }
}
