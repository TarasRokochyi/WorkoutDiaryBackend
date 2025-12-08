using System.Text;
using BLL.DTO.Identity;
using BLL.Services;
using BLL.Services.Contracts;
using DAL.Models;
using DAL.Models.Entities;
using DAL.Repositories;
using DAL.Repositories.Contracts;
using DAL.UOW;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GymWebService;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        
        
        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        //builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();
        
        builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
        if (builder.Environment.IsDevelopment())
        {
            Console.WriteLine("In dev");
            builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);
            builder.Services.AddDbContext<GymWebServiceContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DockerPostgreSQLConnection")));
        }
        else if (builder.Environment.IsProduction())
        {
            Console.WriteLine("In prod");
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            builder.Services.AddDbContext<GymWebServiceContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionStringPostgres")));
        }
        
        // Configure IDENTITY
        builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
            {
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456788-._@+/ ";
            }).AddEntityFrameworkStores<GymWebServiceContext>( );
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 1;
            options.Password.RequiredUniqueChars = 1;
        });
        
        // SEED DATABASE
        //var userManager = builder.Services.BuildServiceProvider().GetRequiredService<UserManager<User>>();
        //var roleManager = builder.Services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole<int>>>();
        //await ContextSeeder.SeedEssentialsAsync(userManager, roleManager);
        
        //Configuration from AppSettings
        
        // Adding Authentication - JWT
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
            
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,


                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
            });
        }
        else 
        {
            builder.Services.Configure<JWT>((jwt) =>
            {
                jwt.Audience = Environment.GetEnvironmentVariable("JWT_Audience");
                jwt.Issuer = Environment.GetEnvironmentVariable("JWT_Issuer");
                jwt.Key = Environment.GetEnvironmentVariable("JWT_Key");
                jwt.DurationInMinutes = 90;
            });
        
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidIssuer = Environment.GetEnvironmentVariable("JWT_Issuer"),
                    ValidAudience = Environment.GetEnvironmentVariable("JWT_Audience"),
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_Key")))
                };
            });
        }
        
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        
        // REPOSITORIES
        builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
        builder.Services.AddScoped<IWorkoutRepository, WorkoutRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IWorkoutTemplateRepository, WorkoutTemplateRepository>();
        builder.Services.AddScoped<IWorkoutExerciseRepository, WorkoutExerciseRepository>();
        builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // SERVICES
        builder.Services.AddScoped<IExerciseService, ExerciseService>();
        builder.Services.AddScoped<IWorkoutService, WorkoutService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IWorkoutTemplateService, WorkoutTemplateService>();
        builder.Services.AddScoped<IWorkoutExerciseService, WorkoutExerciseService>();
        builder.Services.AddScoped<IChartService, ChartService>();

        builder.Services.AddHttpClient();
        
        // AUTOMAPPER
        //builder.Services.AddAutoMapper(typeof(Program).Assembly);
        builder.Services.AddAutoMapper(typeof(Program));
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    policy.WithOrigins("http://localhost:4200") // 👈 Use the exact origin, not "*"
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); // 👈 Now allowed because origin is explicit
                }
                else 
                {
                    policy.WithOrigins(Environment.GetEnvironmentVariable("FRONTEND_URL")) // 👈 Use the exact origin, not "*"
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); // 👈 Now allowed because origin is explicit
                }
            });
        });


        var app = builder.Build();
        
        // for angular
        app.UseCors("AllowFrontend");


        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
            app.UseSwagger();
            app.UseSwaggerUI();
        //}

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
