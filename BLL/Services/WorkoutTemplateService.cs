using AutoMapper;
using BLL.DTO;
using BLL.Services.Contracts;
using DAL.Models;
using DAL.Models.Entities;
using DAL.UOW;

namespace BLL.Services;

public class WorkoutTemplateService : IWorkoutTemplateService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    //private readonly IMemoryCache _cache;
    

    public WorkoutTemplateService(
        IUnitOfWork unitOfWork,
        IMapper mapper
        //IMemoryCache cache
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        //_cache = cache;
    }
    
    public async Task<IEnumerable<WorkoutTemplateResponseDTO>> GetAllWorkoutTemplatesAsync()
    {
        var templates = await _unitOfWork.WorkoutTemplateRepository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<WorkoutTemplateResponseDTO>>(templates);
        return result;
    }

    public async Task<WorkoutTemplateResponseDTO> GetWorkoutTemplateByIdAsync(int id)
    {
        var template = await _unitOfWork.WorkoutTemplateRepository.GetByIdAsync(id);
        var result = _mapper.Map<WorkoutTemplateResponseDTO>(template);
        return result;
    }
    
    public async Task<WorkoutTemplateResponseDTO> GetUserWorkoutTemplateByIdAsync(int userId, int id)
    {
        var template = await _unitOfWork.WorkoutTemplateRepository.GetUserTemplateAsync(userId, id);
        var result = _mapper.Map<WorkoutTemplateResponseDTO>(template);
        return result;
    }

    public async Task<IEnumerable<WorkoutTemplateResponseDTO>> GetAllWorkoutTemplatesByUserIdAsync(int userId)
    {
        var templates = await _unitOfWork.WorkoutTemplateRepository.GetByUserId(userId);
        var result = _mapper.Map<IEnumerable<WorkoutTemplateResponseDTO>>(templates);
        return result;
    }

    public async Task<WorkoutTemplateResponseDTO> AddWorkoutTemplateAsync(WorkoutTemplateRequestDTO template)
    {
        var workoutTemplate = _mapper.Map<WorkoutTemplate>(template);
        var addedWorkoutTemplate = await _unitOfWork.WorkoutTemplateRepository.AddAsync(workoutTemplate);
        await _unitOfWork.CompleteAsync();
        var result = _mapper.Map<WorkoutTemplateResponseDTO>(addedWorkoutTemplate);
        return result;
    }

    public async Task<WorkoutTemplateResponseDTO> UpdateUserWorkoutTemplateAsync(int userId, int id, WorkoutTemplateRequestDTO template)
    {
        var templateToUpdate = await _unitOfWork.WorkoutTemplateRepository.GetUserTemplateAsync(userId, id);
        if (templateToUpdate != null)
        {
            throw new Exception("Not Found");
        }
        
        _mapper.Map(template, templateToUpdate);
        
        var updatedWorkoutTemplate = await _unitOfWork.WorkoutTemplateRepository.UpdateAsync(templateToUpdate);
        await _unitOfWork.CompleteAsync();
        var result = _mapper.Map<WorkoutTemplateResponseDTO>(updatedWorkoutTemplate);
        return result;
    }
    
    public async Task<WorkoutTemplateResponseDTO> UpdateDefaultWorkoutTemplateAsync(int id, WorkoutTemplateRequestDTO template)
    {
        var templateToUpdate = await _unitOfWork.WorkoutTemplateRepository.GetByIdAsync(id);
        if (templateToUpdate != null)
        {
            throw new Exception("Not Found");
        }
        
        _mapper.Map(template, templateToUpdate);
        
        var updatedWorkoutTemplate = await _unitOfWork.WorkoutTemplateRepository.UpdateAsync(templateToUpdate);
        await _unitOfWork.CompleteAsync();
        var result = _mapper.Map<WorkoutTemplateResponseDTO>(updatedWorkoutTemplate);
        return result;
    }

    public async Task DeleteUserWorkoutTemplateAsync(int userId, int id)
    {
        var templateToUpdate = await _unitOfWork.WorkoutTemplateRepository.GetUserTemplateAsync(userId, id);
        if (templateToUpdate != null)
        {
            throw new Exception("Not Found");
        }
        
        await _unitOfWork.WorkoutTemplateRepository.DeleteByIdAsync(id);
        await _unitOfWork.CompleteAsync();
    }
    
    public async Task DeleteDefaultWorkoutTemplateAsync(int id)
    {
        var templateToDelete = await _unitOfWork.WorkoutTemplateRepository.GetByIdAsync(id);
        if (templateToDelete != null)
        {
            throw new Exception("Not Found");
        }
        
        await _unitOfWork.WorkoutTemplateRepository.DeleteAsync(templateToDelete);
        await _unitOfWork.CompleteAsync();
    }
}