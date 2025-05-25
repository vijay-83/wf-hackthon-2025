using Microsoft.AspNetCore.Mvc;
using wf_cloudzone_gen_api.Interfaces;

namespace wf_cloudzone_gen_api.Controllers
{
    [ApiController]
    [Route("api/incidents")]
    public class IncidentController : ControllerBase
    {
        private readonly IIncidentService _incidentService;
        public IncidentController(IIncidentService incidentService)
        {
            _incidentService = incidentService;
        }

        [HttpGet("query")]
        public async Task<IActionResult> GetIncidentsAsync([FromQuery] string searchTerm)
        {
            var incidents = await _incidentService.SearchIncidents(searchTerm);
            return Ok(incidents);
        }

        [HttpGet("grid")]
        public async Task<IActionResult> GetIncidentGrid([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var incidents = await _incidentService.GetPaginatedIncidentsAsync(page, pageSize);
            return Ok(new { data = incidents, total = incidents.Count });
        }

    }
}
