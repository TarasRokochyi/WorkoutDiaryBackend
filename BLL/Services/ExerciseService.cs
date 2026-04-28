using System.Net.Http.Headers;
using System.Net.Http.Json;
using AutoMapper;
using BLL.DTO;
using BLL.Services.Contracts;
using DAL.Models;
using DAL.Models.Entities;
using DAL.Repositories;
using DAL.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace BLL.Services;

public class ExerciseService : IExerciseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private HttpClient _httpClient;
    //private readonly IMemoryCache _cache;
    

    public ExerciseService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        HttpClient httpClient
        //IMemoryCache cache
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpClient = httpClient;
        //_cache = cache;
    }
    
    public async Task<IEnumerable<ExerciseResponseDTO>> GetAllExercisesAsync()
    {
        var exercises = await _unitOfWork.ExerciseRepository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<ExerciseResponseDTO>>(exercises);
        return result;
    }

    public async Task<ExerciseResponseDTO> GetExerciseByIdAsync(int id)
    {
        var exercise = await _unitOfWork.ExerciseRepository.GetByIdAsync(id);
        var result = _mapper.Map<ExerciseResponseDTO>(exercise);
        return result;
    }
    
    public async Task<ExerciseResponseDTO> GetUserExerciseByIdAsync(int userId, int id)
    {
        var exercise = await _unitOfWork.ExerciseRepository.GetUserExerciseAsync(userId, id);
        var result = _mapper.Map<ExerciseResponseDTO>(exercise);
        return result;
    }

    public async Task<IEnumerable<ExerciseResponseDTO>> GetExercisesByUserIdAsync(int id)
    {
        var exercises = await _unitOfWork.ExerciseRepository.GetByUserIdAsync(id);
        var result = _mapper.Map<IEnumerable<ExerciseResponseDTO>>(exercises);
        return result;
    }

    public async Task<ExerciseResponseDTO> AddExerciseAsync(ExerciseRequestDTO exercise)
    {
        var exerciseToAdd = _mapper.Map<Exercise>(exercise);
        var exerciseResult = await _unitOfWork.ExerciseRepository.AddAsync(exerciseToAdd);
        await _unitOfWork.CompleteAsync();
        var result = _mapper.Map<ExerciseResponseDTO>(exerciseResult);
        return result;
    }

    public async Task<ExerciseResponseDTO> UpdateUserExerciseAsync(int userId, int id, ExerciseRequestDTO exercise)
    {
        var exerciseToUpdate = await  _unitOfWork.ExerciseRepository.GetUserExerciseAsync(userId, id);
        if (exerciseToUpdate is null)
        {
            throw new Exception("Not Found");
        }
        
        _mapper.Map(exercise, exerciseToUpdate);
        
        var exerciseResult = await _unitOfWork.ExerciseRepository.UpdateAsync(exerciseToUpdate);
        await _unitOfWork.CompleteAsync();
        var result = _mapper.Map<ExerciseResponseDTO>(exerciseResult);
        return result;
    }
    
    public async Task<ExerciseResponseDTO> UpdateDefaultExerciseAsync(int id, ExerciseRequestDTO exercise)
    {
        var exerciseToUpdate = await  _unitOfWork.ExerciseRepository.GetByIdAsync(id);
        if (exerciseToUpdate is null)
        {
            throw new Exception("Not Found");
        }
        
        _mapper.Map(exercise, exerciseToUpdate);
        
        var exerciseResult = await _unitOfWork.ExerciseRepository.UpdateAsync(exerciseToUpdate);
        await _unitOfWork.CompleteAsync();
        var result = _mapper.Map<ExerciseResponseDTO>(exerciseResult);
        return result;
    }

    public async Task DeleteUserExerciseAsync(int userId, int id)
    {
        var exerciseToDelete = await _unitOfWork.ExerciseRepository.GetUserExerciseAsync(userId, id);
        if (exerciseToDelete != null)
        {
            throw new Exception("Not Found");
        }
        await _unitOfWork.ExerciseRepository.DeleteAsync(exerciseToDelete);
        await _unitOfWork.CompleteAsync();
    }
    
    public async Task DeleteDefaultExerciseAsync(int id)
    {
        var exerciseToDelete = await _unitOfWork.ExerciseRepository.GetByIdAsync(id);
        if (exerciseToDelete != null)
        {
            throw new Exception("Not Found");
        }
        await _unitOfWork.ExerciseRepository.DeleteAsync(exerciseToDelete);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<IEnumerable<ExercisesRecommendationDTO>> getExerciseRecommendationAsync(IFormFile image)
    {
        using var content = new MultipartFormDataContent();

        // Convert IFormFile to StreamContent
        using var fileStream = image.OpenReadStream();
        var streamContent = new StreamContent(fileStream);

        // Optional: pass content-type
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);

        // Add file to multipart form
        content.Add(streamContent, "image", image.FileName);

        // Send to external API
        //var response = await _httpClient.PostAsync(Environment.GetEnvironmentVariable("OBJ_DETECTION_URL"), content);
        var response = await _httpClient.PostAsync("http://localhost:8000/detect/", content);
        response.EnsureSuccessStatusCode();

        var yolo_response = await response.Content.ReadFromJsonAsync<YoloResponseDTO>();
        var objects = yolo_response.objects;
        
        var equipment_names = objects.Where(r => r.confidence > 0.5).Select(r => r.label).Distinct().ToList();

        var recommendations = await _unitOfWork.EquipmentRepository.GetExercisesByEquipmentNameList(equipment_names);
        
        // var result = dictionary.Select(t => new ExercisesRecommendationDTO {equipmentName = t.Key, exercises = _mapper.Map<List<ExerciseResponseDTO>>(t.Value)}).ToList();
        var result = _mapper.Map<IEnumerable<ExercisesRecommendationDTO>>(recommendations);

        return result;
    }
}