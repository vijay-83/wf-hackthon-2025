using Qdrant.Client;
using Qdrant.Client.Grpc;
using QdrantRAGDemo.API.Controllers;
using System.Text.RegularExpressions;
using wf_cloudzone_gen_api.Interfaces;

namespace wf_cloudzone_gen_api.Services;

public class QdrantService
{
    private readonly QdrantClient _client;
    private readonly IEmbeddingService _embeddingService;
    private readonly string _collection = "rag_collection";

    public QdrantService(QdrantClient client, IEmbeddingService embeddingService)
    {
        _client = client;
        _embeddingService = embeddingService;
    }

    //public async Task IndexDocumentsAsync(List<Issue> issues)
    //{
    //    // Drop existing collection and recreate
    //    await _client.DeleteCollectionAsync(_collection);
    //    await _client.CreateCollectionAsync(_collection, new VectorParams
    //    {
    //        Size = 1536,
    //        Distance = Distance.Cosine
    //    });

    //    var points = new List<PointStruct>();
    //    ulong idCounter = 1;

    //    foreach (var issue in issues)
    //    {
    //        var embedding = await _embeddingService.GetEmbeddingAsync(issue.Description);

    //        var point = new PointStruct
    //        {
    //            Id = new PointId { Num = idCounter++ },
    //            Vectors = new Vectors
    //            {
    //                Vector = new Vector { Data = { embedding } }
    //            },
    //            Payload =
    //            {
    //                { "text", new Qdrant.Client.Grpc.Value { StringValue = issue.Description } },
    //                { "issue_id", new Qdrant.Client.Grpc.Value { StringValue = issue.Id.ToString() } },
    //                { "status", new Qdrant.Client.Grpc.Value { StringValue = issue.Status } }
    //            }
    //        };

    //        points.Add(point);
    //    }

    //    await _client.UpsertAsync(_collection, points);
    //}

    public async Task IndexDocumentsAsync(List<Issue> issues)
    {
        // Drop existing collection and recreate
        await _client.DeleteCollectionAsync(_collection);
        await _client.CreateCollectionAsync(_collection, new VectorParams
        {
            Size = 1536,
            Distance = Distance.Cosine
        });

        var points = new List<PointStruct>();
        ulong idCounter = 1;

        foreach (var issue in issues)
        {
            var embeddingText = $"{issue.Summary}. {issue.Description} . {issue.Id}. ";
            var embedding = await _embeddingService.GetEmbeddingAsync(embeddingText);
            Console.WriteLine($"Embedding for Issue {issue.Id}: {string.Join(", ", embedding)}");

            var point = new PointStruct
            {
                Id = new PointId { Num = idCounter++ },
                Vectors = new Vectors
                {
                    Vector = new Vector { Data = { embedding } }
                },
                Payload =
        {
            { "issue_id", new Qdrant.Client.Grpc.Value { StringValue = issue.Id.ToString() } },
            { "status", new Qdrant.Client.Grpc.Value { StringValue = issue.Status } },
            { "priority", new Qdrant.Client.Grpc.Value { StringValue = issue.Priority } },
            { "labels", new Qdrant.Client.Grpc.Value { StringValue = string.Join(", ", issue.Labels) } },
            { "summary", new Qdrant.Client.Grpc.Value { StringValue = issue.Summary } },
            { "description", new Qdrant.Client.Grpc.Value { StringValue = issue.Description } }
        }
            };

            points.Add(point);
        }

        await _client.UpsertAsync(_collection, points);
    }



    public async Task<string> AnswerAsync(string query)
    {
        // Generate embedding **without forcing keyword filtering**
        var embedding = await _embeddingService.GetEmbeddingAsync(query);

        // 🔍 **Search using only embedding similarity—NO rigid filtering**
        var result = await _client.SearchAsync(
            _collection,
            embedding.AsMemory(),
            null, // No filters—only pure semantic matching
            new SearchParams { HnswEf = 512, Exact = false }, // High recall for better results
            5, // Retrieve more results for better ranking
            0,
            new WithPayloadSelector { Enable = true }
        );

        if (!result.Any()) return $"No relevant issues found for '{query}'.";

        var response = result.Select(r =>
            $"Issue ID: {r.Payload["issue_id"].StringValue}\n" +
            $"Status: {r.Payload["status"].StringValue}\n" +
            $"Priority: {r.Payload["priority"].StringValue}\n" +
            $"Labels: {r.Payload["labels"].StringValue}\n" +
            $"Description: {r.Payload["summary"].StringValue}\n"
        );

        return string.Join("\n\n", response);
    }

    //public async Task<string> AnswerAsync(string query)
    //{
    //    var embedding = await _embeddingService.GetEmbeddingAsync(query);

    //    // 🔹 Ensure search uses proper filtering strategy
    //    List<Condition> conditions = new();

    //    // 🔍 Try semantic search on summary & description
    //    conditions.Add(new Condition
    //    {
    //        Field = new FieldCondition
    //        {
    //            Key = "summary",
    //            Match = new Qdrant.Client.Grpc.Match { Text = query }
    //        }
    //    });

    //    conditions.Add(new Condition
    //    {
    //        Field = new FieldCondition
    //        {
    //            Key = "description",
    //            Match = new Qdrant.Client.Grpc.Match { Text = query }
    //        }
    //    });

    //    var filter = new Filter { Should = { conditions } }; // Use "Should" to allow broader matches

    //    // 🔍 **Enhance Retrieval Parameters**
    //    var result = await _client.SearchAsync(
    //        _collection,
    //        embedding.AsMemory(),
    //        filter,
    //        new SearchParams { HnswEf = 512, Exact = false }, // Raise recall
    //        5, // Retrieve more results for better ranking
    //        0,
    //        new WithPayloadSelector { Enable = true }
    //    );

    //    if (!result.Any()) return $"No relevant issues found for '{query}'.";

    //    var response = result.Select(r =>
    //        $"Issue ID: {r.Payload["issue_id"].StringValue}\n" +
    //        $"Status: {r.Payload["status"].StringValue}\n" +
    //        $"Priority: {r.Payload["priority"].StringValue}\n" +
    //        $"Labels: {r.Payload["labels"].StringValue}\n" +
    //        $"Description: {r.Payload["summary"].StringValue}\n"
    //    );

    //    return string.Join("\n\n", response);
    //}

    //public async Task<string> AnswerAsync(string query)
    //{
    //    var embedding = await _embeddingService.GetEmbeddingAsync(query);

    //    // Extract issue ID if included in query
    //    var issueIdMatch = Regex.Match(query, @"issue\s*(\d+)", RegexOptions.IgnoreCase);
    //    string issueId = issueIdMatch.Success ? issueIdMatch.Groups[1].Value : null;

    //    List<Condition> conditions = new();

    //    // 🔹 **Exact issue ID match if present in query**
    //    if (issueId != null)
    //    {
    //        conditions.Add(new Condition
    //        {
    //            Field = new FieldCondition
    //            {
    //                Key = "issue_id",
    //                Match = new Qdrant.Client.Grpc.Match
    //                {
    //                    Text = issueId // Exact issue match
    //                }
    //            }
    //        });
    //    }
    //    else
    //    {
    //        // 🔹 **General keyword-based filtering for better relevance**
    //        conditions.Add(new Condition
    //        {
    //            Field = new FieldCondition
    //            {
    //                Key = "summary",
    //                Match = new Qdrant.Client.Grpc.Match
    //                {
    //                    Text = query // Match summary for better issue correlation
    //                }
    //            }
    //        });

    //        conditions.Add(new Condition
    //        {
    //            Field = new FieldCondition
    //            {
    //                Key = "description",
    //                Match = new Qdrant.Client.Grpc.Match
    //                {
    //                    Text = query // Match description for enhanced search results
    //                }
    //            }
    //        });
    //    }

    //    var filter = new Filter { Should = { conditions } };

    //    var result = await _client.SearchAsync(
    //        _collection,
    //        embedding.AsMemory(),
    //        filter,
    //        new SearchParams { HnswEf = 512, Exact = issueId != null }, // Adjust match precision dynamically
    //        10,
    //        0,
    //        new WithPayloadSelector { Enable = true }
    //    );

    //    if (!result.Any()) return $"No relevant issues found for '{query}'.";

    //    var response = result.Select(r =>
    //        $"Issue ID: {r.Payload["issue_id"].StringValue}\n" +
    //        $"Status: {r.Payload["status"].StringValue}\n" +
    //        $"Priority: {r.Payload["priority"].StringValue}\n" +
    //        $"Labels: {r.Payload["labels"].StringValue}\n" +
    //        $"Description: {r.Payload["summary"].StringValue}\n"
    //    );

    //    return string.Join("\n\n", response);
    //}

    //public async Task<string> AnswerAsync(string query)
    //{
    //    var embedding = await _embeddingService.GetEmbeddingAsync(query);

    //    var result = await _client.SearchAsync(
    //        _collection,
    //        embedding.AsMemory(),
    //        null,
    //        null,
    //        3,
    //        3,
    //        new WithPayloadSelector { Enable = true }
    //    );

    //    var response = result.Select(r =>
    //        $"Issue ID: {r.Payload["issue_id"].StringValue}\n" +
    //        $"Status: {r.Payload["status"].StringValue}\n" +
    //        $"Description: {r.Payload["text"].StringValue}\n"
    //    );

    //    return string.Join("\n\n", response);
    //}

    //public async Task<string> AnswerAsync(string query)
    //{
    //    var embedding = await _embeddingService.GetEmbeddingAsync(query);

    //    // Perform search WITHOUT forcing exact matches
    //    var result = await _client.SearchAsync(
    //        _collection,
    //        embedding.AsMemory(),
    //        null, // No strict filtering to allow broader matches
    //        new SearchParams { HnswEf = 128, Exact = false }, // Enhance semantic search
    //        5, // Get top 5 closest matches
    //        0, // No offset
    //        new WithPayloadSelector { Enable = true }
    //    );

    //    if (!result.Any()) return $"No relevant issues found for '{query}'.";

    //    var response = result.Select(r =>
    //        $"Issue ID: {r.Payload["issue_id"].StringValue}\n" +
    //        $"Status: {r.Payload["status"].StringValue}\n" +
    //        $"Priority: {r.Payload["priority"].StringValue}\n" +
    //        $"Labels: {r.Payload["labels"].StringValue}\n" +
    //        $"Description: {r.Payload["text"].StringValue}\n"
    //    );

    //    return string.Join("\n\n", response);
    //}

    //public async Task<string> AnswerAsync(string query)
    //{
    //    // Extract issue number from query using regex or parsing logic
    //    var issueIdMatch = Regex.Match(query, @"\d+");
    //    if (!issueIdMatch.Success) return "No valid issue ID found in query.";

    //    var issueId = issueIdMatch.Value;

    //    var embedding = await _embeddingService.GetEmbeddingAsync(query);

    //    var filter = new Filter
    //    {
    //        Must =
    //{
    //    new Condition
    //    {
    //        Field = new FieldCondition
    //        {
    //            Key = "issue_id",
    //            Match = new Qdrant.Client.Grpc.Match
    //            {
    //                Text = issueId // Use `Text` if issue ID is stored as a string
    //            }
    //        }
    //    }
    //}
    //    };

    //    var result = await _client.SearchAsync(
    //        _collection,
    //        embedding.AsMemory(),
    //        filter, // Ensure exact match for issue ID
    //        new SearchParams { HnswEf = 128, Exact = true }, // Optimize search accuracy
    //        1, // Limit to 1 result since we want an exact issue match
    //        0, // No offset
    //        new WithPayloadSelector { Enable = true }
    //    );

    //    if (!result.Any()) return $"No details found for issue ID {issueId}.";

    //    var match = result.First();
    //    return $"Issue ID: {match.Payload["issue_id"].StringValue}\n" +
    //           $"Status: {match.Payload["status"].StringValue}\n" +
    //           $"Priority: {match.Payload["priority"].StringValue}\n" +
    //           $"Labels: {match.Payload["labels"].StringValue}\n" +
    //           $"Description: {match.Payload["text"].StringValue}\n";
    //}

    //public async Task<string> AnswerAsync(string query)
    //{
    //    var embedding = await _embeddingService.GetEmbeddingAsync(query);

    //    var filter = new Filter
    //    {
    //        Should =
    //    {
    //        new Condition
    //        {
    //            Field = new FieldCondition
    //            {
    //                Key = "text", // Search within the issue descriptions
    //                Match = new Qdrant.Client.Grpc.Match
    //                {
    //                    Text = query // Allow keyword search
    //                }
    //            }
    //        }
    //    }
    //    };

    //    var result = await _client.SearchAsync(
    //        _collection,
    //        embedding.AsMemory(),
    //        filter,
    //        new SearchParams { HnswEf = 128, Exact = false }, // Supports semantic matching
    //        5, // Retrieve multiple relevant results
    //        0,
    //        new WithPayloadSelector { Enable = true }
    //    );

    //    if (!result.Any()) return $"No relevant issues found for '{query}'.";

    //    var response = result.Select(r =>
    //        $"Issue ID: {r.Payload["issue_id"].StringValue}\n" +
    //        $"Status: {r.Payload["status"].StringValue}\n" +
    //        $"Priority: {r.Payload["priority"].StringValue}\n" +
    //        $"Labels: {r.Payload["labels"].StringValue}\n" +
    //        $"Description: {r.Payload["text"].StringValue}\n"
    //    );

    //    return string.Join("\n\n", response);
    //}

}