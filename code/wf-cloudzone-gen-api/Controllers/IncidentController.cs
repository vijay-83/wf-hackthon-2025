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
  
        [HttpGet("grid")]
        public async Task<IActionResult> GetIncidentGrid([FromQuery] int itemCount =50)
        {
            var incidents = await _incidentService.GetIncidentsByCountAsync(itemCount);
            return Ok(new { data = incidents, total = incidents.Count });
        }

    }
}
