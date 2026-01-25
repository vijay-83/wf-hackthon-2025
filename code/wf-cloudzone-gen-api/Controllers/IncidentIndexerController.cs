using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using wf_cloudzone_gen_api.Interfaces;
using wf_cloudzone_gen_api.Models;

namespace wf_cloudzone_gen_api.Controllers
{

    // Controller: Controllers/IncidentIndexerController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentIndexerController : ControllerBase
    {
        private readonly IIncidentIndexerService _indexer;

        public IncidentIndexerController(IIncidentIndexerService indexer)
        {
            _indexer = indexer;
        }

        [HttpPost("index")]
        public async Task<IActionResult> IndexIncident([FromBody] Incident incident)
        {
            await _indexer.IndexIncidentAsync(incident);
            return Ok("Incident indexed.");
        }

        [HttpPost("index/batch")]
        public async Task<IActionResult> IndexMany([FromBody] List<Incident> incidents)
        {
            await _indexer.IndexManyAsync(incidents);
            return Ok($"{incidents.Count} incidents indexed.");
        }

        [HttpPost("index/upload")]
        public async Task<IActionResult> IndexFromFile([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);

            try
            {
                var serializer = new JsonSerializer();
                var incidents = serializer.Deserialize<List<Incident>>(jsonReader);

                if (incidents == null)
                    return BadRequest("Invalid or empty JSON.");

                await _indexer.IndexManyAsync(incidents);
                return Ok($"{incidents.Count} incidents indexed from file.");
            }
            catch (JsonException ex)
            {
                return BadRequest($"Failed to parse JSON. Error: {ex.Message}");
            }
        }

    }
}
