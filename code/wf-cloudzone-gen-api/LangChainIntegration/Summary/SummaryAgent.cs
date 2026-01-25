using Qdrant.Client;
using Qdrant.Client.Grpc;
using wf_cloudzone_gen_api.Interfaces;
using wf_cloudzone_gen_api.LangChainIntegration.Summary;

namespace wf_cloudzone_gen_api.LangChainIntegration.Summary
{
    public class SummaryAgent : ISummaryAgent
    {
        private readonly QdrantClient _qdrantClient;
        private readonly IEmbeddingService _embeddingService;
        private readonly LangChainClient _langChainClient;

        public SummaryAgent(QdrantClient qdrantClient, IEmbeddingService embeddingService, LangChainClient langChainClient)
        {
            _qdrantClient = qdrantClient;
            _embeddingService = embeddingService;
            _langChainClient = langChainClient;
        }

        public async Task<SummaryResponse> GenerateSummaryAsync(SummaryRequest request)
        {
            var embedding = await _embeddingService.GetEmbeddingAsync(request.Query);


            var searchResult = await _qdrantClient.SearchAsync(collectionName: "incident-index",vector: embedding,
                filter: null, limit: (ulong)request.TopK);

        //    var result = await _client.SearchAsync(
        //    _collection,
        //    embedding.AsMemory(),
        //    null, // No filters—only pure semantic matching
        //    new SearchParams { HnswEf = 512, Exact = false }, // High recall for better results
        //    5, // Retrieve more results for better ranking
        //    0,
        //    new WithPayloadSelector { Enable = true }
        //);

            var contextTexts = searchResult.Select(x => x.Payload["text"].ToString()).ToList();

            //var summary = await _langChainClient.GetSummaryAsync(request.Query, contextTexts);

            return new SummaryResponse { Summary = contextTexts.ToString()};
        }
    }

}
