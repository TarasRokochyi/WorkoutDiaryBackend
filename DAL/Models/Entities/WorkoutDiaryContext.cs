using DAL.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.Entities;

public partial class WorkoutDiaryContext : IdentityDbContext<
    User, 
    IdentityRole<int>, 
    int, 
    IdentityUserClaim<int>,
    IdentityUserRole<int>,
    IdentityUserLogin<int>,
    IdentityRoleClaim<int>,
    IdentityUserToken<int>>
{
    public WorkoutDiaryContext()
    {
    }

    public WorkoutDiaryContext(DbContextOptions<WorkoutDiaryContext> options)
        : base(options)
    {
        //Database.EnsureDeleted();
        //Database.EnsureCreated();
        //Database.Migrate();
    }

    public virtual DbSet<Exercise> Exercises { get; set; }
    
    public virtual DbSet<Equipment> Equipments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Workout> Workouts { get; set; }

    public virtual DbSet<WorkoutExercise> WorkoutExercises { get; set; }

    public virtual DbSet<WorkoutTemplate> WorkoutTemplates { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?Linkid=723263.
    //    => optionsBuilder.UseNpgsql("Host=localhost;Port=5434;Database=WorkoutDiary;Username=pgadmin;Password=pgadmin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.ExerciseId).HasName("exercises_pkey");

            entity.ToTable("exercises");

            entity.Property(e => e.ExerciseId).HasColumnName("exerciseid");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.MuscleGroups)
                .HasMaxLength(255)
                .HasColumnName("musclegroups");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UserId).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Exercises)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("exercises_userid_fkey");
            entity.HasMany(e => e.Equipments).WithMany(eq => eq.Exercises)
                .UsingEntity("ExerciseEquipment");
        });
        
        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentId).HasName("equipment_pkey");

            entity.ToTable("equipments");

            entity.Property(e => e.EquipmentId).HasColumnName("equipmentid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp with time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasColumnName("gender");
            entity.Property(e => e.Height)
                .HasPrecision(10, 2)
                .HasColumnName("height");
            entity.Property(e => e.Level)
                .HasMaxLength(50)
                .HasColumnName("level");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Weight)
                .HasPrecision(10, 2)
                .HasColumnName("weight");
        });

        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(e => e.WorkoutId).HasName("workouts_pkey");

            entity.ToTable("workouts");

            entity.Property(e => e.WorkoutId).HasColumnName("workoutid");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp with time zone")
                .HasColumnName("date");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.UserId).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Workouts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("workouts_userid_fkey");
        });

        modelBuilder.Entity<WorkoutExercise>(entity =>
        {
            entity.HasKey(e => e.WorkoutExerciseId).HasName("workoutexercises_pkey");

            entity.ToTable("workoutexercises");

            entity.Property(e => e.WorkoutExerciseId).HasColumnName("workoutexerciseid");
            entity.Property(e => e.Distance)
                .HasPrecision(10, 2)
                .HasColumnName("distance");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.ExerciseId).HasColumnName("exerciseid");
            entity.Property(e => e.Reps).HasColumnName("reps");
            entity.Property(e => e.Sets).HasColumnName("sets");
            entity.Property(e => e.Weight)
                .HasPrecision(10, 2)
                .HasColumnName("weight");
            entity.Property(e => e.WorkoutId).HasColumnName("workoutid");
            entity.Property(e => e.WorkoutTemplateId).HasColumnName("workouttemplateid");

            entity.HasOne(d => d.Exercise).WithMany(p => p.WorkoutExercises)
                .HasForeignKey(d => d.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("workoutexercises_exerciseid_fkey");

            entity.HasOne(d => d.Workout).WithMany(p => p.WorkoutExercises)
                .HasForeignKey(d => d.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("workoutexercises_workoutid_fkey");
            
            entity.HasOne(d => d.WorkoutTemplate).WithMany(p => p.WorkoutExercises)
                .HasForeignKey(d => d.WorkoutTemplateId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("workoutexercises_workouttemplateid_fkey");
        });

        modelBuilder.Entity<WorkoutTemplate>(entity =>
        {
            entity.HasKey(e => e.TemplateId).HasName("workouttemplates_pkey");

            entity.ToTable("workouttemplates");

            entity.Property(e => e.TemplateId).HasColumnName("templateid");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UserId).HasColumnName("userid");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Notes).HasColumnName("notes");

            entity.HasOne(d => d.User).WithMany(p => p.WorkoutTemplates)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("workouttemplates_userid_fkey");
        });
        
        modelBuilder.Entity<IdentityRole<int>>().HasData(new List<IdentityRole<int>>
        {
            new IdentityRole<int> {
                Id = 1,
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole<int> {
                Id = 2,
                Name = "User",
                NormalizedName = "USER"
            }
        });
        
        var hasher = new PasswordHasher<User>();
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1, // primary key
                FirstName = "main",
                LastName = "admin",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                PasswordHash = hasher.HashPassword(null, "admin")
            },
            new User
            {
                Id = 2, // primary key
                FirstName = "default",
                LastName = "user",
                UserName = AuthorizationConst.default_username,
                NormalizedUserName = AuthorizationConst.default_username.ToUpper(),
                Email = AuthorizationConst.default_email,
                NormalizedEmail = AuthorizationConst.default_email.ToUpper(),
                PasswordHash = hasher.HashPassword(null, AuthorizationConst.default_password)
            }
        );
        
        modelBuilder.Entity<IdentityUserRole<int>>().HasData(
            new IdentityUserRole<int>
            {
                RoleId = 1, // for admin username
                UserId = 1,// for admin role
            },
            new IdentityUserRole<int>
            {
                RoleId = 2, // for admin username
                UserId = 2, // for admin role
            }
        );

        modelBuilder.Entity<Equipment>().HasData(
            new Equipment
            {
                EquipmentId = 1,
                Name = "pull up bar"
            },
            new Equipment
            {
                EquipmentId = 2,
                Name = "parallel bars"
            },
            new Equipment
            {
                EquipmentId = 3,
                Name = "barbell"
            },
            new Equipment
            {
                EquipmentId = 4,
                Name = "dumbbell"
            },
            new Equipment
            {
                EquipmentId = 5,
                Name = "bench"
            },
            new Equipment
            {
                EquipmentId = 6,
                Name = "resistance band"
            });

        modelBuilder.Entity<Exercise>().HasData(
            new Exercise{
                ExerciseId = 1,
                UserId = null,
                Name = "Pull ups",
                Category = "Strength",
                MuscleGroups = "back, arms",
                Description = ""
            },
            new Exercise{
                ExerciseId = 2,
                UserId = null,
                Name = "Squats",
                Category = "Strength",
                MuscleGroups = "Quadriceps, Glutes",
                Description = ""
            },
            new Exercise{
                ExerciseId = 3,
                UserId = null,
                Name = "Dips",
                Category = "Strength",
                MuscleGroups = "Triceps, Chest, Shoulders",
                Description = ""
            },
            new Exercise{
                ExerciseId = 4,
                UserId = null,
                Name = "Biceps curls",
                Category = "Strength",
                MuscleGroups = "Biceps",
                Description = ""
            },
            new Exercise{
                ExerciseId = 5,
                UserId = null,
                Name = "Deadlift",
                Category = "Strength",
                MuscleGroups = "Hamstring, Glutes, Quadriceps",
                Description = ""
            },
            new Exercise{
                ExerciseId = 6,
                UserId = null,
                Name = "Running",
                Category = "Cardio",
                MuscleGroups = "legs",
                Description = ""
            },
            new Exercise{
                ExerciseId = 7,
                UserId = null,
                Name = "Swimming",
                Category = "Cardio",
                MuscleGroups = "Upper body",
                Description = ""
            },
            new Exercise{
                ExerciseId = 8,
                UserId = null,
                Name = "Bicycle",
                Category = "Cardio",
                MuscleGroups = "Quadriceps",
                Description = ""
            },
            new Exercise{
                ExerciseId = 9,
                UserId = null,
                Name = "Walking",
                Category = "Cardio",
                MuscleGroups = "Whole body",
                Description = ""
            },
            new Exercise{
                ExerciseId = 10,
                UserId = null,
                Name = "Rowing",
                Category = "Cardio",
                MuscleGroups = "Whole body",
                Description = ""
            },
            new Exercise{
                ExerciseId = 11,
                UserId = null,
                Name = "Bench press",
                Category = "Strength",
                MuscleGroups = "Chest, Triceps",
                Description = ""
            },
            new Exercise{
                ExerciseId = 12,
                UserId = null,
                Name = "Bent-over row",
                Category = "Strength",
                MuscleGroups = "Chest, Triceps",
                Description = ""
            },
            new Exercise{
                ExerciseId = 13,
                UserId = null,
                Name = "Romanian deadlifts",
                Category = "Strength",
                MuscleGroups = "Hamstring, Glutes",
                Description = ""
            },
            new Exercise{
                ExerciseId = 14,
                UserId = null,
                Name = "Split squats",
                Category = "Strength",
                MuscleGroups = "Quadriceps",
                Description = ""
            },
            new Exercise{
                ExerciseId = 15,
                UserId = null,
                Name = "Bulgarian split squats",
                Category = "Strength",
                MuscleGroups = "Quadriceps, Glutes",
                Description = ""
            },
            new Exercise{
                ExerciseId = 16,
                UserId = null,
                Name = "Standing overhead press",
                Category = "Strength",
                MuscleGroups = "Shoulders, triceps",
                Description = ""
            },
            new Exercise{
                ExerciseId = 17,
                UserId = null,
                Name = "Hammer curls",
                Category = "Strength",
                MuscleGroups = "Shoulders, triceps",
                Description = ""
            },
            new Exercise{
                ExerciseId = 18,
                UserId = null,
                Name = "Pistol squats",
                Category = "Strength",
                MuscleGroups = "Quadriceps",
                Description = ""
            },
            new Exercise{
                ExerciseId = 19,
                UserId = null,
                Name = "Wrist curls",
                Category = "Strength",
                MuscleGroups = "Forearm",
                Description = ""
            },
            new Exercise{
                ExerciseId = 20,
                UserId = null,
                Name = "Muscle ups",
                Category = "Strength",
                MuscleGroups = "Upper body",
                Description = ""
            },
            new Exercise{
                ExerciseId = 21,
                UserId = null,
                Name = "L-sit",
                Category = "Static",
                MuscleGroups = "Upper body",
                Description = ""
            },
            new Exercise{
                ExerciseId = 22,
                UserId = null,
                Name = "Dead hang",
                Category = "Static",
                MuscleGroups = "Upper body",
                Description = ""
            },
            new Exercise{
                ExerciseId = 23,
                UserId = null,
                Name = "Plank",
                Category = "Static",
                MuscleGroups = "core",
                Description = ""
            },
            new Exercise{
                ExerciseId = 24,
                UserId = null,
                Name = "Hand stand",
                Category = "Static",
                MuscleGroups = "core",
                Description = ""
            },
            new Exercise{
                ExerciseId = 25,
                UserId = null,
                Name = "Front leaver",
                Category = "Static",
                MuscleGroups = "upper body",
                Description = ""
            },
            new Exercise{
                ExerciseId = 26,
                UserId = null,
                Name = "Planche",
                Category = "Static",
                MuscleGroups = "upper body",
                Description = ""
            },
            new Exercise{
                ExerciseId = 27,
                UserId = null,
                Name = "Wall sit",
                Category = "Static",
                MuscleGroups = "legs",
                Description = ""
            },
            new Exercise{
                ExerciseId = 28,
                UserId = null,
                Name = "Push-ups",
                Category = "Strength",
                MuscleGroups = "upper body",
                Description = ""
            }
            
        );
        
        modelBuilder.Entity("ExerciseEquipment").HasData(
            // 1. Pull ups
            new { ExercisesExerciseId = 1, EquipmentsEquipmentId = 1 },
            new { ExercisesExerciseId = 1, EquipmentsEquipmentId = 6 },

            // 2. Squats
            new { ExercisesExerciseId = 2, EquipmentsEquipmentId = 3 },
            new { ExercisesExerciseId = 2, EquipmentsEquipmentId = 6 },

            // 3. Dips
            new { ExercisesExerciseId = 3, EquipmentsEquipmentId = 2 },
            new { ExercisesExerciseId = 3, EquipmentsEquipmentId = 6 },

            // 4. Biceps curls
            new { ExercisesExerciseId = 4, EquipmentsEquipmentId = 4 },
            new { ExercisesExerciseId = 4, EquipmentsEquipmentId = 6 },
            new { ExercisesExerciseId = 4, EquipmentsEquipmentId = 3 },

            // 5. Deadlift
            new { ExercisesExerciseId = 5, EquipmentsEquipmentId = 3 },
            new { ExercisesExerciseId = 5, EquipmentsEquipmentId = 4 },

            // 6. Running (no equipment)

            // 7. Swimming (no equipment)

            // 8. Bicycle (no matching equipment)

            // 9. Walking (no equipment)

            // 10. Rowing
            new { ExercisesExerciseId = 10, EquipmentsEquipmentId = 6 },

            // 11. Bench press
            new { ExercisesExerciseId = 11, EquipmentsEquipmentId = 5 },
            new { ExercisesExerciseId = 11, EquipmentsEquipmentId = 3 },
            new { ExercisesExerciseId = 11, EquipmentsEquipmentId = 4 },

            // 12. Bent-over row
            new { ExercisesExerciseId = 12, EquipmentsEquipmentId = 3 },
            new { ExercisesExerciseId = 12, EquipmentsEquipmentId = 4 },
            new { ExercisesExerciseId = 12, EquipmentsEquipmentId = 6 },

            // 13. Romanian deadlifts
            new { ExercisesExerciseId = 13, EquipmentsEquipmentId = 3 },
            new { ExercisesExerciseId = 13, EquipmentsEquipmentId = 4 },

            // 14. Split squats
            new { ExercisesExerciseId = 14, EquipmentsEquipmentId = 4 },
            new { ExercisesExerciseId = 14, EquipmentsEquipmentId = 6 },

            // 15. Bulgarian split squats
            new { ExercisesExerciseId = 15, EquipmentsEquipmentId = 5 },
            new { ExercisesExerciseId = 15, EquipmentsEquipmentId = 4 },

            // 16. Standing overhead press
            new { ExercisesExerciseId = 16, EquipmentsEquipmentId = 3 },
            new { ExercisesExerciseId = 16, EquipmentsEquipmentId = 4 },
            new { ExercisesExerciseId = 16, EquipmentsEquipmentId = 6 },

            // 17. Hammer curls
            new { ExercisesExerciseId = 17, EquipmentsEquipmentId = 4 },
            new { ExercisesExerciseId = 17, EquipmentsEquipmentId = 6 },

            // 18. Pistol squats
            new { ExercisesExerciseId = 18, EquipmentsEquipmentId = 6 },
            new { ExercisesExerciseId = 18, EquipmentsEquipmentId = 4 },

            // 19. Wrist curls
            new { ExercisesExerciseId = 19, EquipmentsEquipmentId = 4 },
            new { ExercisesExerciseId = 19, EquipmentsEquipmentId = 3 },

            // 20. Muscle ups
            new { ExercisesExerciseId = 20, EquipmentsEquipmentId = 1 },
            new { ExercisesExerciseId = 20, EquipmentsEquipmentId = 6 },
            
            // 21. L-sit
            new {ExercisesExerciseId = 21, EquipmentsEquipmentId = 2},
            
            // 22. Dead hang
            new {ExercisesExerciseId = 22, EquipmentsEquipmentId = 1},
            new {ExercisesExerciseId = 22, EquipmentsEquipmentId = 6},
            
            // 23. Plank
            new {ExercisesExerciseId = 23, EquipmentsEquipmentId = 2},
            
            // 24. Hand stand
            new {ExercisesExerciseId = 24, EquipmentsEquipmentId = 2},
            
            // 25. Front leaver
            new {ExercisesExerciseId = 25, EquipmentsEquipmentId = 1},
            new {ExercisesExerciseId = 25, EquipmentsEquipmentId = 6},
            
            // 26. Planche
            new {ExercisesExerciseId = 26, EquipmentsEquipmentId = 2},
            new {ExercisesExerciseId = 26, EquipmentsEquipmentId = 6},
            
            // 27. Wall sit
            new {ExercisesExerciseId = 27, EquipmentsEquipmentId = 4},
            
            // 28. Push-ups
            new {ExercisesExerciseId = 28, EquipmentsEquipmentId = 2}
        );


        modelBuilder.Entity<WorkoutTemplate>().HasData(
            new WorkoutTemplate
            {
                TemplateId = 1,
                Description = "some description",
                Duration = 90,
                Name = "pull day",
                Notes = "some notes",
                UserId = null,
            }
        );
        modelBuilder.Entity<WorkoutExercise>().HasData(
            new WorkoutExercise
             {
                 WorkoutExerciseId = 1,
                 ExerciseId = 1,
                 Reps = 5,
                 Sets = 5,
                 Weight = 20,
                 WorkoutTemplateId = 1,
             },
             new WorkoutExercise{
                 WorkoutExerciseId = 2,
                 ExerciseId = 2,
                 Reps = 5,
                 Sets = 5,
                 Weight = 40,
                 WorkoutTemplateId = 1,
             }
        );
        
        

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
