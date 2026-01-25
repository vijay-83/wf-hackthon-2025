namespace wf_cloudzone_gen_api.Interfaces
{
    // Optional mock embedding service for now
    public interface IEmbeddingService
    {
        Task<float[]> GetVectorAsync(string text);
        Task<float[]> GetEmbeddingAsync(string input);
    }
}
