using BLL.DTO;
using DAL.Models;

namespace GymWebService.Mappings;

public class AutomapperProfile : AutoMapper.Profile
{

    public AutomapperProfile()
    {
        CreateMap<Exercise, ExerciseResponseDTO>().ReverseMap();
        CreateMap<Exercise, ExerciseRequestDTO>().ReverseMap();
        
        CreateMap<User, UserResponseDTO>().ReverseMap();
        CreateMap<User, UserRequestDTO>().ReverseMap();
        
        CreateMap<WorkoutExercise, WorkoutExerciseResponseDTO>().ReverseMap();
        CreateMap<WorkoutExercise, WorkoutExerciseRequestDTO>().ReverseMap();
        
        CreateMap<Workout, WorkoutResponseDTO>().ReverseMap();
        CreateMap<Workout, WorkoutRequestDTO>().ReverseMap();
        
        CreateMap<Workout, WorkoutShortResponseDTO>().ReverseMap();
        CreateMap<Workout, WorkoutRequestDTO>().ReverseMap();
        
        CreateMap<WorkoutTemplate, WorkoutTemplateResponseDTO>().ReverseMap();
        CreateMap<WorkoutTemplateRequestDTO, WorkoutTemplate>().ForMember(destination => destination.UserId, opt => opt.MapFrom(src => src.UserId == 0 ? (int?)null : src.UserId));
    }
}