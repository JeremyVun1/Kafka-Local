using Microsoft.AspNetCore.Mvc;
using Producer.Models;
using Producer.Services;

namespace Producer.Controllers;

[ApiController]
public class ProducerController : ControllerBase
{
    private readonly IProducerService _service;

    public ProducerController(IProducerService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("event")]
    public async Task<IActionResult> Produce([FromBody] ProduceRequest request)
    {
        var result = await _service.Produce(request);
        return Ok(result);
    }
}
