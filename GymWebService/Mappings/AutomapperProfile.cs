using BLL.DTO;
using BLL.DTO.ChartDTO;
using DAL.Models;
using DAL.Models.Entities;

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
        
        CreateMap<WorkoutExercise, WorkoutExerciseVolumeDTO>()
            .ForMember(dest => dest.Volume, opt => opt.MapFrom(src =>
                src.Exercise.Category.ToLower() == "cardioValue"
                    ? (decimal?)(src.Duration) / (src.Distance)  // or whatever metric makes sense
                    : (decimal?)(src.Reps ?? 0) * (src.Sets ?? 0) * (src.Weight ?? 0)
            ));
        CreateMap<Workout, WorkoutVolumeDTO>();

        CreateMap<WorkoutExerciseMaxWeightChart, WorkoutExerciseMaxWeightDTO>();
        
        CreateMap<ExerciseRecommendation, ExercisesRecommendationDTO>().ReverseMap();

        // CreateMap<IGrouping<string, Exercise>, ExercisesRecommendationDTO>()
        //     .ForMember(dest => dest.equipmentName, opt => opt.MapFrom(src => src.Key))
        //     .ForMember(dest => dest.exercises, opt => opt.MapFrom(src => src.ToList()));
    }
}