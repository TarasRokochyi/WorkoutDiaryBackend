CREATE TABLE Users (
    UserId SERIAL PRIMARY KEY,
    Name VARCHAR(255),
    Level VARCHAR(50), -- Beginner, Intermediate, Advanced
    Gender VARCHAR(50),
    Weight Decimal(10,2),
    Height Decimal(10,2),
    Age INT,
    CreatedAt TIMESTAMP with time zone DEFAULT NOW()
);

CREATE TABLE Workouts (
    WorkoutId SERIAL PRIMARY KEY,
    UserId INT REFERENCES Users(UserId) ON DELETE CASCADE,
    Name VARCHAR(255), -- e.g., "Leg Day", "Full Body Strength"
    Date TIMESTAMP with time zone DEFAULT NOW(),
    Duration INT, -- In minutes
    Notes TEXT
);

CREATE TABLE Exercises (
    ExerciseId SERIAL PRIMARY KEY,
    UserId INT NULL REFERENCES Users(UserId) ON DELETE CASCADE,
    Name VARCHAR(255), -- e.g., "Squat", "Bench Press"
    Category VARCHAR(50), -- e.g., Strength, Cardio
    MuscleGroups VARCHAR(255), -- e.g., "Legs, Glutes"
    Description TEXT,
    Difficulty VARCHAR(50) -- Beginner, Intermediate, Advanced
);

CREATE TABLE WorkoutExercises (
    WorkoutExerciseId SERIAL PRIMARY KEY,
    WorkoutId INT REFERENCES Workouts(WorkoutId) ON DELETE CASCADE,
    ExerciseId INT REFERENCES Exercises(ExerciseId) ON DELETE CASCADE,
    Type VARCHAR(50), -- emom, tabata, comfort rest, ...
    Sets INT,
    Reps INT,
    Weight DECIMAL(10,2), -- If applicable
    Distance DECIMAL(10,2), -- For cardio (e.g., running distance)
    Duration INT, -- Time spent on exercise in seconds
);

CREATE TABLE WorkoutTemplates (
    TemplateId SERIAL PRIMARY KEY,
    UserId INT NULL REFERENCES Users(UserId) ON DELETE CASCADE,
    Name VARCHAR(255), -- e.g., "5x5 Strength Training"
    Description TEXT
);

