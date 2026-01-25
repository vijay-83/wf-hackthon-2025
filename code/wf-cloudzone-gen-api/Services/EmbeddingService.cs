using System;
using System.Threading.Tasks;
using wf_cloudzone_gen_api.Interfaces;
namespace wf_cloudzone_gen_api.Services;
public class EmbeddingService : IEmbeddingService
{
    private static readonly Random _random = new();
    public Task<float[]> GetEmbeddingAsync(string input)
    {
        var vector = new float[1536];
        for (int i = 0; i < vector.Length; i++)
            vector[i] = (float)_random.NextDouble();
        return Task.FromResult(vector);
    }

    public Task<float[]> GetVectorAsync(string text)
    {
        var random = new Random();
        float[] vector = new float[1536];
        for (int i = 0; i < vector.Length; i++)
        {
            vector[i] = (float)(random.NextDouble() * 2 - 1); // values from -1 to 1
        }
        return Task.FromResult(vector);
    }
}
