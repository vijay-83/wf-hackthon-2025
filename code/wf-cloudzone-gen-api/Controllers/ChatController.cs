using Microsoft.AspNetCore.Mvc;
using wf_cloudzone_gen_api.Services;
using System.Threading.Tasks;

namespace QdrantRAGDemo.API.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly QdrantService _qdrantService;

    public ChatController(QdrantService qdrantService)
    {
        _qdrantService = qdrantService;
    }

    [HttpGet]
    public async Task<IActionResult> Ask([FromQuery] string question)
    {
        var answer = await _qdrantService.AnswerAsync(question);
        return Ok(new { question, answer });
    }
}
