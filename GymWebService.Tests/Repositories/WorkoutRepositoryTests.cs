using DAL.Models;
using DAL.Models.Entities;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GymWebService.Tests.Repositories;

public class WorkoutRepositoryTests
{
    private readonly Mock<GymWebServiceContext> _mockContext;
    private readonly Mock<DbSet<Workout>> _mockWorkoutSet;
    private readonly WorkoutRepository _workoutRepository;

    public WorkoutRepositoryTests()
    {
        _mockContext = new Mock<GymWebServiceContext>();

        var workouts = new List<Workout>
        {
            new Workout { WorkoutId = 1, UserId = 10 },
            new Workout { WorkoutId = 2, UserId = 10 },
            new Workout { WorkoutId = 3, UserId = 20 }
        }.AsQueryable();

        _mockWorkoutSet = MockDbSetHelper.CreateMockDbSet(workouts);
        _mockContext.Setup(c => c.Set<Workout>()).Returns(_mockWorkoutSet.Object);

        _workoutRepository = new WorkoutRepository(_mockContext.Object);
    }

    [Fact]
    public async Task GetByUserIdAsync_ReturnsUserWorkouts()
    {
        // Act
        var result = await _workoutRepository.GetByUserIdAsync(10);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, w => Assert.Equal(10, w.UserId));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectWorkout()
    {
        // Act
        var result = await _workoutRepository.GetByIdAsync(2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.WorkoutId);
    }

    [Fact]
    public async Task GetUserWorkoutAsync_ReturnsCorrectWorkoutForUser()
    {
        // Act
        var result = await _workoutRepository.GetUserWorkoutAsync(20, 3);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(20, result.UserId);
        Assert.Equal(3, result.WorkoutId);
    }
}
