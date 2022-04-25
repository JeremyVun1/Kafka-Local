using Api.AdminClient.Models;
using Confluent.Kafka.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Api.AdminClient.Controllers;

[ApiController]
[Route("")]
public class AdminController : ControllerBase
{
    private readonly IKafkaAdminService _adminService;

    public AdminController(IKafkaAdminService adminService)
    {
        this._adminService = adminService;
    }

    [HttpPost]
    [Route("topic")]
    public async Task<IActionResult> CreateTopic([FromBody]CreateTopicRequest request)
    {
        return Ok(await _adminService.CreateTopic(request));
    }

    [HttpGet]
    [Route("topic/{topicName}")]
    public async Task<IActionResult> GetTopic(string topicName)
    {
        return Ok(await _adminService.GetTopic(topicName));
    }

    [HttpDelete]
    [Route("topic/{topicName}")]
    public async Task<IActionResult> DeleteTopic(string topicName)
    {
        return Ok(await _adminService.DeleteTopic(topicName));
    }
}
