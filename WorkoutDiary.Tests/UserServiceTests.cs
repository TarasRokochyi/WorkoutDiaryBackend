using System.Security.Claims;
using AutoMapper;
using BLL.DTO.Identity;
using BLL.Services;
using DAL.Models;
using DAL.Models.Entities;
using DAL.Repositories.Contracts;
using DAL.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace GymWebService.Tests;

public class UserServiceTests
{
    
    [Fact]
    public async Task RegisterAsync_ShouldCreateUser_WhenEmailNotExists()
    {
        // Arrange
        var userManager = MockUserManager();
        var roleManager = MockRoleManager();

        userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null);

        userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        userManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        var service = CreateService(userManager.Object, roleManager.Object);

        var model = new RegisterModel
        {
            Email = "test@mail.com",
            Username = "test",
            Password = "123456",
            FirstName = "T",
            LastName = "R"
        };

        // Act
        var result = await service.RegisterAsync(model);

        // Assert
        Assert.Contains("User Registered", result);
    }
    
    [Fact]
    public async Task RegisterAsync_ShouldThrow_WhenEmailExists()
    {
        var userManager = MockUserManager();

        userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new User());

        var service = CreateService(userManager.Object);

        var model = new RegisterModel { Email = "test@mail.com" };

        await Assert.ThrowsAsync<Exception>(() => service.RegisterAsync(model));
    }

    [Fact]
    public async Task GetTokenAsync_ShouldReturnToken_WhenCredentialsValid()
    {
        var userManager = MockUserManager();
        
        var userRepo = new Mock<IUserRepository>();

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.UserRepository)
            .Returns(userRepo.Object);

        var user = new User
        {
            Email = "test@mail.com",
            UserName = "test",
            RefreshTokens = new List<RefreshToken>()
        };

        userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        userManager.Setup(x => x.CheckPasswordAsync(user, It.IsAny<string>()))
            .ReturnsAsync(true);

        userManager.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "User" });

        userManager.Setup(x => x.GetClaimsAsync(user))
            .ReturnsAsync(new List<Claim>());

        var service = CreateService(userManager.Object, unitOfWork: unitOfWork.Object);

        var result = await service.GetTokenAsync(new TokenRequestModel
        {
            Email = "test@mail.com",
            Password = "123"
        });

        Assert.True(result.IsAuthenticated);
        Assert.NotNull(result.Token);
    }

    [Fact]
    public async Task GetTokenAsync_ShouldFail_WhenPasswordWrong()
    {
        var userManager = MockUserManager();

        var user = new User { Email = "test@mail.com" };

        userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        userManager.Setup(x => x.CheckPasswordAsync(user, It.IsAny<string>()))
            .ReturnsAsync(false);

        var service = CreateService(userManager.Object);

        var result = await service.GetTokenAsync(new TokenRequestModel
        {
            Email = "test@mail.com",
            Password = "wrong"
        });

        Assert.False(result.IsAuthenticated);
    }

    [Fact]
    public async Task RefreshTokenAsync_ShouldFail_WhenUserNotFound()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        unitOfWork.Setup(x => x.UserRepository.GetUserByTokenAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null);

        var service = CreateService(unitOfWork: unitOfWork.Object);

        var result = await service.RefreshTokenAsync("token");

        Assert.False(result.IsAuthenticated);
        Assert.Contains("Token did not match", result.Message);
    }
    
    [Fact]
    public async Task RefreshTokenAsync_ShouldFail_WhenTokenNotActive()
    {
        var unitOfWork = new Mock<IUnitOfWork>();

        var user = new User
        {
            Email = "test@mail.com",
            UserName = "test",
            RefreshTokens = new List<RefreshToken>
            {
                new RefreshToken
                {
                    Token = "token",
                    Revoked = DateTime.UtcNow // NOT active
                }
            }
        };

        unitOfWork.Setup(x => x.UserRepository.GetUserByTokenAsync("token"))
            .ReturnsAsync(user);

        var service = CreateService(unitOfWork: unitOfWork.Object);

        var result = await service.RefreshTokenAsync("token");

        Assert.False(result.IsAuthenticated);
        Assert.Contains("Not Active", result.Message);
    }
    
    [Fact]
    public async Task RefreshTokenAsync_ShouldGenerateNewToken_WhenValid()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var userManager = MockUserManager();

        var refreshToken = new RefreshToken
        {
            Token = "token",
            Expires = DateTime.UtcNow.AddDays(1),
            Created = DateTime.UtcNow
        };

        var user = new User
        {
            Email = "test@mail.com",
            UserName = "test",
            RefreshTokens = new List<RefreshToken> { refreshToken }
        };

        userManager.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "User" });

        userManager.Setup(x => x.GetClaimsAsync(user))
            .ReturnsAsync(new List<Claim>());

        unitOfWork.Setup(x => x.UserRepository.GetUserByTokenAsync("token"))
            .ReturnsAsync(user);

        var service = CreateService(
            unitOfWork: unitOfWork.Object,
            userManager: userManager.Object
        );

        var result = await service.RefreshTokenAsync("token");

        Assert.True(result.IsAuthenticated);
        Assert.NotNull(result.Token);
        Assert.NotNull(result.RefreshToken);

        // verify DB update
        unitOfWork.Verify(x => x.UserRepository.UpdateAsync(user), Times.Once);
        unitOfWork.Verify(x => x.CompleteAsync(default(CancellationToken)), Times.Once);
    }


    private Mock<UserManager<User>> MockUserManager()
    {
        var store = new Mock<IUserStore<User>>();
        return new Mock<UserManager<User>>(
            store.Object, null, null, null, null, null, null, null, null);
    }

    private Mock<RoleManager<IdentityRole<int>>> MockRoleManager()
    {
        var store = new Mock<IRoleStore<IdentityRole<int>>>();
        return new Mock<RoleManager<IdentityRole<int>>>(
            store.Object, null, null, null, null);
    }
    
    private UserService CreateService(
        UserManager<User> userManager = null,
        RoleManager<IdentityRole<int>> roleManager = null,
        IUnitOfWork unitOfWork = null)
    {
        var mapper = new Mock<IMapper>().Object;
        //var unitOfWork = new Mock<IUnitOfWork>().Object;

        var jwtOptions = Options.Create(new JWT
        {
            Key = "supersecretkeysupersecretkeysuperlongsuperkey",
            Issuer = "test",
            Audience = "test",
            DurationInMinutes = 60
        });

        return new UserService(
            unitOfWork ?? new Mock<IUnitOfWork>().Object,
            mapper,
            userManager ?? MockUserManager().Object,
            jwtOptions,
            roleManager ?? MockRoleManager().Object
        );
    }


}