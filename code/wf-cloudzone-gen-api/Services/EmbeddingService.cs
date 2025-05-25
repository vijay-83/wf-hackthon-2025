using System;
using System.Threading.Tasks;
namespace wf_cloudzone_gen_api.Services;
public class EmbeddingService
{
    private static readonly Random _random = new();
    public Task<float[]> GetEmbeddingAsync(string input)
    {
        var vector = new float[1536];
        for (int i = 0; i < vector.Length; i++)
            vector[i] = (float)_random.NextDouble();
        return Task.FromResult(vector);
    }
}
