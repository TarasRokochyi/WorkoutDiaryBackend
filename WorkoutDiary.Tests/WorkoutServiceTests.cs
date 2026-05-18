using AutoMapper;
using BLL.DTO;
using BLL.Services;
using DAL.Models.Entities;
using DAL.Repositories.Contracts;
using DAL.UOW;
using Moq;

namespace GymWebService.Tests;

public class WorkoutServiceTests
{
    
    [Fact]
    public async Task GetAllWorkoutsAsync_ShouldReturnMappedWorkouts()
    {
        // Arrange
        var workouts = new List<Workout>
        {
            new Workout { WorkoutId = 1 },
            new Workout { WorkoutId = 2 }
        };

        var mapped = new List<WorkoutResponseDTO>
        {
            new WorkoutResponseDTO(),
            new WorkoutResponseDTO()
        };

        var repoMock = new Mock<IWorkoutRepository>();
        repoMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(workouts);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(x => x.WorkoutRepository)
            .Returns(repoMock.Object);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(x => x.Map<IEnumerable<WorkoutResponseDTO>>(workouts))
            .Returns(mapped);

        var service = CreateService(unitOfWorkMock, mapperMock);

        // Act
        var result = await service.GetAllWorkoutsAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }
    
    [Fact]
    public async Task GetWorkoutByIdAsync_ShouldReturnWorkout()
    {
        var workout = new Workout { WorkoutId = 1 };

        var dto = new WorkoutResponseDTO();

        var repoMock = new Mock<IWorkoutRepository>();
        repoMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(workout);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(x => x.WorkoutRepository)
            .Returns(repoMock.Object);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(x => x.Map<WorkoutResponseDTO>(workout))
            .Returns(dto);

        var service = CreateService(unitOfWorkMock, mapperMock);

        var result = await service.GetWorkoutByIdAsync(1);

        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task AddWorkoutAsync_ShouldAddWorkout()
    {
        var request = new WorkoutRequestDTO();

        var workout = new Workout();

        var response = new WorkoutResponseDTO();

        var repoMock = new Mock<IWorkoutRepository>();

        repoMock.Setup(x => x.AddAsync(workout))
            .ReturnsAsync(workout);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(x => x.WorkoutRepository)
            .Returns(repoMock.Object);

        var mapperMock = new Mock<IMapper>();

        mapperMock.Setup(x => x.Map<Workout>(request))
            .Returns(workout);

        mapperMock.Setup(x => x.Map<WorkoutResponseDTO>(workout))
            .Returns(response);

        var service = CreateService(unitOfWorkMock, mapperMock);

        var result = await service.AddWorkoutAsync(request);

        Assert.NotNull(result);

        repoMock.Verify(x => x.AddAsync(workout), Times.Once);

        unitOfWorkMock.Verify(x => x.CompleteAsync(default(CancellationToken)), Times.Once);
    }
    
    [Fact]
    public async Task UpdateUserWorkoutAsync_ShouldThrow_WhenWorkoutNotFound()
    {
        var repoMock = new Mock<IWorkoutRepository>();

        repoMock.Setup(x => x.GetUserWorkoutAsync(1, 1))
            .ReturnsAsync((Workout)null);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(x => x.WorkoutRepository)
            .Returns(repoMock.Object);

        var mapperMock = new Mock<IMapper>();

        var service = CreateService(unitOfWorkMock, mapperMock);

        await Assert.ThrowsAsync<Exception>(() =>
            service.UpdateUserWorkoutAsync(1, 1, new WorkoutRequestDTO()));
    }
    
    [Fact]
    public async Task UpdateUserWorkoutAsync_ShouldUpdateWorkout()
    {
        var workout = new Workout();

        var dto = new WorkoutResponseDTO();

        var repoMock = new Mock<IWorkoutRepository>();

        repoMock.Setup(x => x.GetUserWorkoutAsync(1, 1))
            .ReturnsAsync(workout);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(x => x.WorkoutRepository)
            .Returns(repoMock.Object);

        var mapperMock = new Mock<IMapper>();

        mapperMock.Setup(x => x.Map<WorkoutResponseDTO>(workout))
            .Returns(dto);

        var service = CreateService(unitOfWorkMock, mapperMock);

        var result = await service.UpdateUserWorkoutAsync(
            1,
            1,
            new WorkoutRequestDTO());

        Assert.NotNull(result);

        repoMock.Verify(x => x.UpdateAsync(workout), Times.Once);

        unitOfWorkMock.Verify(x => x.CompleteAsync(default(CancellationToken)), Times.Once);
    }
    
    [Fact]
    public async Task DeleteUserWorkoutAsync_ShouldThrow_WhenWorkoutNotFound()
    {
        var repoMock = new Mock<IWorkoutRepository>();

        repoMock.Setup(x => x.GetUserWorkoutAsync(1, 1))
            .ReturnsAsync((Workout)null);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(x => x.WorkoutRepository)
            .Returns(repoMock.Object);

        var mapperMock = new Mock<IMapper>();

        var service = CreateService(unitOfWorkMock, mapperMock);

        await Assert.ThrowsAsync<Exception>(() =>
            service.DeleteUserWorkoutAsync(1, 1));
    }
    
    [Fact]
    public async Task DeleteUserWorkoutAsync_ShouldDeleteWorkout()
    {
        var workout = new Workout();

        var repoMock = new Mock<IWorkoutRepository>();

        repoMock.Setup(x => x.GetUserWorkoutAsync(1, 1))
            .ReturnsAsync(workout);

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(x => x.WorkoutRepository)
            .Returns(repoMock.Object);

        var mapperMock = new Mock<IMapper>();

        var service = CreateService(unitOfWorkMock, mapperMock);

        await service.DeleteUserWorkoutAsync(1, 1);

        repoMock.Verify(x => x.DeleteAsync(workout), Times.Once);

        unitOfWorkMock.Verify(x => x.CompleteAsync(default(CancellationToken)), Times.Once);
    }
    
    
    
    
    private WorkoutService CreateService(
        Mock<IUnitOfWork> unitOfWorkMock,
        Mock<IMapper> mapperMock)
    {
        return new WorkoutService(
            unitOfWorkMock.Object,
            mapperMock.Object
        );
    }
}