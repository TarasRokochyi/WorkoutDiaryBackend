using BLL.DTO;
using BLL.Services.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorkoutDiary.Controller;

[ApiController]
[Route("api/[controller]")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
public class AdminController : ControllerBase
{
    private ILogger<AdminController> _logger;
    private IExerciseService _exerciseService;
    private IUserService _userService;
    private IWorkoutService _workoutService;
    private IWorkoutTemplateService _workoutTemplateService;
    
    public AdminController(IExerciseService exerciseService, 
        IUserService userService,
        IWorkoutService workoutService,
        IWorkoutTemplateService workoutTemplateService,
        ILogger<AdminController> logger)
    {
        _logger = logger;
        _exerciseService = exerciseService;
        _userService = userService;
        _workoutService = workoutService;
        _workoutTemplateService = workoutTemplateService;
    }
    
    // EXERCISE ENDPOINTS
    [HttpPost("addDefaultExercise")]
    public async Task<ActionResult<ExerciseResponseDTO>> PostDefaultExercise(ExerciseRequestDTO exercise)
    {
        exercise.UserId = null;
        var result = await _exerciseService.AddExerciseAsync(exercise);
        return Ok(result);
    }

    [HttpGet("getAllExercises")]
    public async Task<ActionResult<IEnumerable<ExerciseResponseDTO>>> GetAllExercises()
    {
        var result = await _exerciseService.GetAllExercisesAsync();
        return Ok(result);
    }

    [HttpGet("exercise/{id}")]
    public async Task<ActionResult<ExerciseResponseDTO>> GetExercise(int id)
    {
        var result = await _exerciseService.GetExerciseByIdAsync(id);
        return Ok(result);
    }
    
    [HttpPut("updateDefaultExercise/{id}")]
    public async Task<ActionResult<ExerciseResponseDTO>> PutDefaultExercise(int id, ExerciseRequestDTO exercise)
    {
        var result = await _exerciseService.UpdateDefaultExerciseAsync(id, exercise);
        return Ok(result);
    }

    [HttpDelete("deleteDefaultExercise/{id}")]
    public async Task<ActionResult> DeleteDefaultExercise(int id)
    {
        await _exerciseService.DeleteDefaultExerciseAsync(id);
        return NoContent();
    }
    
    
    // USER ENDPOINTS
    [HttpGet("getAllUsers")]
    public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAllUsers()
    {
        var result = await _userService.GetAllUsersAsync();
        return Ok(result);
    }
    
    [HttpPost("userTokens/{id}")]
    public async Task<IActionResult> GetRefreshTokens(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        return Ok(user.RefreshTokens);
    }
    
    
    
    // WORKOUT TEMPLATE ENDPOINTS
    
    [HttpPost("addDefaultTemplate")]
    public async Task<ActionResult> PostDefaultWorkoutTemplate(WorkoutTemplateRequestDTO workoutTemplateRequest)
    {
        workoutTemplateRequest.UserId = null;
        var result = await _workoutTemplateService.AddWorkoutTemplateAsync(workoutTemplateRequest);
        return Ok(result);
    }
    
    [HttpGet("getAllTemplates")]
    public async Task<ActionResult<IEnumerable<WorkoutTemplateResponseDTO>>> GetAllWorkoutTemplates()
    {
        var result = await _workoutTemplateService.GetAllWorkoutTemplatesAsync();
        return Ok(result);
    }
    
    [HttpGet("template/{id}")]
    public async Task<ActionResult<WorkoutTemplateResponseDTO>> GetWorkoutTemplates(int id)
    {
        var result = await _workoutTemplateService.GetWorkoutTemplateByIdAsync(id);
        return Ok(result);
    }
    
    [HttpPut("updateDefault/{id}")]
    public async Task<ActionResult> PutDefaultWorkoutTemplate(int id, WorkoutTemplateRequestDTO workoutTemplateRequest)
    {
        var result = await _workoutTemplateService.UpdateDefaultWorkoutTemplateAsync(id, workoutTemplateRequest);
        return Ok(result);
    }
    
    [HttpDelete("deleteDefault/{id}")]
    public async Task<ActionResult> DeleteDefaultWorkoutTemplate(int id)
    {
        await _workoutTemplateService.DeleteDefaultWorkoutTemplateAsync(id);
        return NoContent();
    }
    
    
    // WORKOUT ENDPOINTS
    
    [HttpGet("getAllWorkouts")]
    public async Task<ActionResult<IEnumerable<WorkoutResponseDTO>>> GetAllWorkouts()
    {
        var result = await _workoutService.GetAllWorkoutsAsync();
        return Ok(result);
    }
    
    [HttpGet("workout/{id}")]
    public async Task<ActionResult<WorkoutResponseDTO>> GetWorkout(int id)
    {
        var result = await _workoutService.GetWorkoutByIdAsync(id);
        return Ok(result);
    }
    
    [HttpDelete("deleteWorkout/{id}")]
    public async Task<ActionResult> DeleteWorkout(int id)
    {
        await _workoutService.DeleteWorkoutAsync(id);
        return NoContent();
    }
}