
using Microsoft.AspNetCore.Mvc;
using wf_cloudzone_gen_api.Services;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace QdrantRAGDemo.API.Controllers;

[ApiController]
[Route("api/index")]
public class IndexController : ControllerBase
{
    private readonly QdrantService _qdrantService;

    public IndexController(QdrantService qdrantService)
    {
        _qdrantService = qdrantService;
    }

    //[HttpPost]
    //public async Task<IActionResult> IndexDocuments()
    //{
    //    var filePath = Path.Combine("data", "sample_documents.json");
    //    if (!System.IO.File.Exists(filePath)) return NotFound("Sample data not found");

    //    var json = await System.IO.File.ReadAllTextAsync(filePath);
    //    var documents = JsonSerializer.Deserialize<List<string>>(json);
    //    await _qdrantService.IndexDocumentsAsync(documents ?? new());
    //    return Ok("Indexed successfully.");
    //}


    //[HttpPost]
    //public async Task<IActionResult> IndexDocuments1()
    //{
    //    var filePath = Path.Combine("data", "sample_documents.json");
    //    if (!System.IO.File.Exists(filePath)) return NotFound("Sample data not found");

    //    var json = await System.IO.File.ReadAllTextAsync(filePath);
    //    var documents = JsonSerializer.Deserialize<List<Issue>>(json);

    //    // Convert List<Issue> to List<string> by extracting issue descriptions
    //    var documentTexts = documents?.Select(issue => issue.Description).ToList() ?? new List<string>();
    //    await _qdrantService.IndexDocumentsAsync(documentTexts);
    //    return Ok("Indexed successfully.");
    //}

    [HttpPost]
    public async Task<IActionResult> IndexDocuments()
    {
        var filePath = Path.Combine("data", "sample_documents.json");
        if (!System.IO.File.Exists(filePath)) return NotFound("Sample data not found");

        try
        {
            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var issues = JsonSerializer.Deserialize<List<Issue>>(json, options);

            if (issues == null || !issues.Any()) return BadRequest("No issues found in the JSON file.");

            await _qdrantService.IndexDocumentsAsync(issues);

            return Ok("Indexed successfully.");
        }
        catch (JsonException ex)
        {
            return BadRequest($"JSON deserialization failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Unexpected error: {ex.Message}");
        }
    }
}

public class Issue
{
    public string Id { get; set; }
    public string Summary { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<string> Labels { get; set; } = new List<string>();
    public string Assignee { get; set; } = string.Empty;
    public Dictionary<string, string> Payload { get; set; } = new Dictionary<string, string>();
}
