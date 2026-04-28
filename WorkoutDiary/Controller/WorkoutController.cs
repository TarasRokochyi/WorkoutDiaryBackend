using BLL.DTO;
using BLL.Services.Contracts;
using WorkoutDiary.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorkoutDiary.Controller;

[ApiController]

[Route("/api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class WorkoutController : ControllerBase
{
    private readonly ILogger<WorkoutController> _logger;
    
    private IWorkoutService _workoutService;

    public WorkoutController(IWorkoutService workoutService, ILogger<WorkoutController> logger)
    {
        _workoutService = workoutService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkoutShortResponseDTO>>> GetAllUserWorkouts()
    {
        int userId = HttpContext.GetUserId();
        var result = await _workoutService.GetAllWorkoutsByUserIdAsync(userId);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<WorkoutResponseDTO>> GetUserWorkout(int id)
    {
        var userId = HttpContext.GetUserId();
        var result = await _workoutService.GetUserWorkoutByIdAsync(userId, id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> PostWorkout(WorkoutRequestDTO workoutRequest){
        int userId = HttpContext.GetUserId();
        workoutRequest.UserId = userId;
        var result = await _workoutService.AddWorkoutAsync(workoutRequest);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutUserWorkout(int id, WorkoutRequestDTO workoutRequest)
    {
        var userId = HttpContext.GetUserId();
        workoutRequest.UserId = userId;
        var result = await _workoutService.UpdateUserWorkoutAsync(userId, id, workoutRequest);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUserWorkout(int id)
    {
        var userId = HttpContext.GetUserId();
        await _workoutService.DeleteUserWorkoutAsync(userId, id);
        return NoContent();
    }
    
    
}