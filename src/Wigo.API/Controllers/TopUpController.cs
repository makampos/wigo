using Microsoft.AspNetCore.Mvc;
using Wigo.Domain.Entities;
using Wigo.Domain.Interfaces;

namespace Wigo.API.Controllers;


[ApiController]
[Route("api/top-up")]
public class TopUpController : ControllerBase
{
    private readonly ITopUpRepository _topUpRepository;

    public TopUpController(ITopUpRepository topUpRepository)
    {
        _topUpRepository = topUpRepository;
    }

    [HttpGet("options")]
    public async Task<ActionResult<IEnumerable<TopUpOption>>> GetTopUpOptions()
    {
        var options = await _topUpRepository.GetTopUpOptionsAsync();
        return Ok(options);
    }
}