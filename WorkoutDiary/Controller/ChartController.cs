using BLL.DTO.ChartDTO;
using BLL.Services.Contracts;
using WorkoutDiary.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorkoutDiary.Controller;

[ApiController]
[Route("/api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ChartController : ControllerBase
{
    private readonly ILogger<WorkoutController> _logger;
    
    private IChartService _chartService;
    
 public ChartController(IChartService chartService, ILogger<WorkoutController> logger)
    {
        _chartService = chartService;
        _logger = logger;
    }

    [HttpGet("volume")]
    public async Task<ActionResult<IEnumerable<WorkoutExerciseVolumeDTO>>> GetTotalVolume()
    {
        var userId = HttpContext.GetUserId();
        var result = await _chartService.getTotalVolumeAsync(userId);
        return Ok(result);
    }

    [HttpGet("max-weight")]
    public async Task<ActionResult<IEnumerable<WorkoutExerciseMaxWeightDTO>>> GetMaxWeight()
    {
        var userId = HttpContext.GetUserId();
        var result = await _chartService.getMaxWeightAsync(userId);
        return Ok(result);
    }

    [HttpGet("kpis")]
    public async Task<ActionResult<KPIsDTO>> GetKPIs([FromQuery] int period)
    {
        var userId = HttpContext.GetUserId();
        var result = await _chartService.getKPIsAsync(userId, period);
        return Ok(result);
    }
}
