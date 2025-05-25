using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wf_cloudzone_gen_api.Interfaces;
using wf_cloudzone_gen_api.Models;

namespace wf_cloudzone_gen_api.Controllers
{
    [ApiController]
    [Route("api/agents")]
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _agentService;

        public AgentController(IAgentService agentService)
        {
            _agentService = agentService;
        }

        [HttpPost("summary")]
        public async Task<IActionResult> GenerateSummary([FromBody] AgentRequest request)
        {
            var summary = await _agentService.GenerateSummary(request.Text);
            return Ok(summary);
        }

        [HttpPost("suggest")]
        public async Task<IActionResult> GetSuggestions([FromBody] AgentRequest request)
        {
            var suggestions = await _agentService.GetSuggestions(request.Text);
            return Ok(suggestions);
        }

        [HttpPost("bugfix")]
        public async Task<IActionResult> GetBugFixes([FromBody] AgentRequest request)
        {
            var bugFix = await _agentService.GetBugFix(request.Text);
            return Ok(bugFix);
        }
    }
}
