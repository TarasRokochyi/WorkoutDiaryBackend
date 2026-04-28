using BLL.DTO;
using BLL.Services.Contracts;
using DAL.Repositories.Contracts;
using WorkoutDiary.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorkoutDiary.Controller;

[ApiController]

[Route("/api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class WorkoutTemplateController : ControllerBase
{
    
    private readonly ILogger<WorkoutTemplateController> _logger;
    
    private IWorkoutTemplateService _workoutTemplateService;

    public WorkoutTemplateController(IWorkoutTemplateService workoutTemplateService, ILogger<WorkoutTemplateController> logger)
    {
        _workoutTemplateService = workoutTemplateService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkoutTemplateResponseDTO>>> GetUserWorkoutTemplates()
    {
        int userId = HttpContext.GetUserId();
        var result = await _workoutTemplateService.GetAllWorkoutTemplatesByUserIdAsync(userId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkoutTemplateResponseDTO>> GetUserWorkoutTemplate(int id)
    {
        var userId = HttpContext.GetUserId();
        var result = await _workoutTemplateService.GetUserWorkoutTemplateByIdAsync(userId, id);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult> PostWorkoutTemplate(WorkoutTemplateRequestDTO workoutTemplateRequest)
    {
        workoutTemplateRequest.UserId = HttpContext.GetUserId();
        var result = await _workoutTemplateService.AddWorkoutTemplateAsync(workoutTemplateRequest);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutUserWorkoutTemplate(int id, WorkoutTemplateRequestDTO workoutTemplateRequest)
    {
        var userId = HttpContext.GetUserId();
        var result = await _workoutTemplateService.UpdateUserWorkoutTemplateAsync(userId, id, workoutTemplateRequest);
        return Ok(result);
    }
    

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUserWorkoutTemplate(int id)
    {
        var userId = HttpContext.GetUserId();
        await _workoutTemplateService.DeleteUserWorkoutTemplateAsync(userId, id);
        return NoContent();
    }
    
}