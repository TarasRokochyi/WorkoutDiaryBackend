using DAL.Models;
using DAL.Models.Entities;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace GymWebService.Tests.Repositories;

public class GenericRepositoryTests
{
    private readonly Mock<GymWebServiceContext> _mockContext;
    private readonly Mock<DbSet<Exercise>> _mockSet;
    private readonly GenericRepository<Exercise> _repository;

    public GenericRepositoryTests()
    {
        _mockContext = new Mock<GymWebServiceContext>();
        _mockSet = new Mock<DbSet<Exercise>>();
        _mockContext.Setup(m => m.Set<Exercise>()).Returns(_mockSet.Object);
        _repository = new GenericRepository<Exercise>(_mockContext.Object);
    }

    [Fact]
    public async Task AddAsync_AddsEntity()
    {
        // Arrange
        var exercise = new Exercise {
            ExerciseId = 2,
            Name = "pull up",
            Category = "Strength",
            Description = "some",
            MuscleGroups = "sldkfjs",
            UserId = null,
        };
        _mockSet.Setup(m => m.AddAsync(exercise, It.IsAny<CancellationToken>()))
                .ReturnsAsync((EntityEntry<Exercise>)Mock.Of<EntityEntry<Exercise>>(e => e.Entity == exercise));

        // Act
        var result = await _repository.AddAsync(exercise);

        // Assert
        Assert.Equal(exercise, result);
        _mockSet.Verify(m => m.AddAsync(exercise, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllEntities()
    {
        // Arrange
        var data = new List<Exercise> {
            new Exercise {
                ExerciseId = 1,
                Name = "dips",
                Category = "Strength",
                Description = "some other",
                MuscleGroups = "sldkfjsjsdfkj",
                UserId = null,
            },
            new Exercise {
                ExerciseId = 2,
                Name = "pull up",
                Category = "Strength",
                Description = "some",
                MuscleGroups = "sldkfjs",
                UserId = null,
            }
        }.AsQueryable();

        _mockSet.As<IQueryable<Exercise>>().Setup(m => m.Provider).Returns(data.Provider);
        _mockSet.As<IQueryable<Exercise>>().Setup(m => m.Expression).Returns(data.Expression);
        _mockSet.As<IQueryable<Exercise>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _mockSet.As<IQueryable<Exercise>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        _mockContext.Setup(c => c.Set<Exercise>()).Returns(_mockSet.Object);

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task DeleteAsync_RemovesEntity()
    {
        // Arrange
        var exercise = new Exercise {
            ExerciseId = 2,
            Name = "pull up",
            Category = "Strength",
            Description = "some",
            MuscleGroups = "sldkfjs",
            UserId = null,
        };

        // Act
        await _repository.DeleteAsync(exercise);

        // Assert
        _mockSet.Verify(m => m.Remove(exercise), Times.Once);
    }
}