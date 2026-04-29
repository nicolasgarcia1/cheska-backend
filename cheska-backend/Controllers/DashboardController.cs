using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Dashboard;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController(GetDashboardStatsUseCase stats) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStats()
    {
        var result = await stats.ExecuteAsync();
        return Ok(result.Data);
    }
}