using System;
using System.Collections.Generic;
using BusinessLogic.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Context;

public partial class FitJourneyDbContext : DbContext
{
    public FitJourneyDbContext()
    {
    }

    public FitJourneyDbContext(DbContextOptions<FitJourneyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Achievement> Achievements { get; set; }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Activityexercise> Activityexercises { get; set; }

    public virtual DbSet<Challenge> Challenges { get; set; }

    public virtual DbSet<Exercise> Exercises { get; set; }

    public virtual DbSet<Goal> Goals { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sport> Sports { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=FitJourney;Username=postgres;Password=123456");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.AchievementId).HasName("achievements_pkey");

            entity.ToTable("achievements");

            entity.Property(e => e.AchievementId).HasColumnName("achievement_id");
            entity.Property(e => e.AchievedDate).HasColumnName("achieved_date");
            entity.Property(e => e.AchievementDescription).HasColumnName("achievement_description");
            entity.Property(e => e.AchievementName)
                .HasMaxLength(255)
                .HasColumnName("achievement_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Achievements)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("achievements_user_id_fkey");
        });

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("activities_pkey");

            entity.ToTable("activities");

            entity.Property(e => e.ActivityId).HasColumnName("activity_id");
            entity.Property(e => e.ActivityDate).HasColumnName("activity_date");
            entity.Property(e => e.ActivityName)
                .HasMaxLength(255)
                .HasColumnName("activity_name");
            entity.Property(e => e.CaloriesBurned).HasColumnName("calories_burned");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");
            entity.Property(e => e.SportId).HasColumnName("sport_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Exercise).WithMany(p => p.Activities)
                .HasForeignKey(d => d.ExerciseId)
                .HasConstraintName("activities_exercise_id_fkey");

            entity.HasOne(d => d.Sport).WithMany(p => p.Activities)
                .HasForeignKey(d => d.SportId)
                .HasConstraintName("activities_sport_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Activities)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("activities_user_id_fkey");
        });

        modelBuilder.Entity<Activityexercise>(entity =>
        {
            entity.HasKey(e => e.ActivityExerciseId).HasName("activityexercises_pkey");

            entity.ToTable("activityexercises");

            entity.Property(e => e.ActivityExerciseId).HasColumnName("activity_exercise_id");
            entity.Property(e => e.ActivityId).HasColumnName("activity_id");
            entity.Property(e => e.Distance).HasColumnName("distance");
            entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");
            entity.Property(e => e.Weight).HasColumnName("weight");

            entity.HasOne(d => d.Activity).WithMany(p => p.Activityexercises)
                .HasForeignKey(d => d.ActivityId)
                .HasConstraintName("activityexercises_activity_id_fkey");

            entity.HasOne(d => d.Exercise).WithMany(p => p.Activityexercises)
                .HasForeignKey(d => d.ExerciseId)
                .HasConstraintName("activityexercises_exercise_id_fkey");
        });

        modelBuilder.Entity<Challenge>(entity =>
        {
            entity.HasKey(e => e.ChallengeId).HasName("challenges_pkey");

            entity.ToTable("challenges");

            entity.Property(e => e.ChallengeId).HasColumnName("challenge_id");
            entity.Property(e => e.Achieved).HasColumnName("achieved");
            entity.Property(e => e.ChallengeDescription).HasColumnName("challenge_description");
            entity.Property(e => e.ChallengeName)
                .HasMaxLength(255)
                .HasColumnName("challenge_name");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.SportId).HasColumnName("sport_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.Exercise).WithMany(p => p.Challenges)
                .HasForeignKey(d => d.ExerciseId)
                .HasConstraintName("challenges_exercise_id_fkey");

            entity.HasOne(d => d.Owner).WithMany(p => p.Challenges)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("challenges_owner_id_fkey");

            entity.HasOne(d => d.Sport).WithMany(p => p.Challenges)
                .HasForeignKey(d => d.SportId)
                .HasConstraintName("challenges_sport_id_fkey");

            entity.HasMany(d => d.Participants).WithMany(p => p.ChallengesNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "Challengeparticipant",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("challengeparticipants_participant_id_fkey"),
                    l => l.HasOne<Challenge>().WithMany()
                        .HasForeignKey("ChallengeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("challengeparticipants_challenge_id_fkey"),
                    j =>
                    {
                        j.HasKey("ChallengeId", "ParticipantId").HasName("challengeparticipants_pkey");
                        j.ToTable("challengeparticipants");
                        j.IndexerProperty<int>("ChallengeId").HasColumnName("challenge_id");
                        j.IndexerProperty<int>("ParticipantId").HasColumnName("participant_id");
                    });
        });

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.ExerciseId).HasName("exercises_pkey");

            entity.ToTable("exercises");

            entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");
            entity.Property(e => e.ExerciseDescription).HasColumnName("exercise_description");
            entity.Property(e => e.ExerciseName)
                .HasMaxLength(255)
                .HasColumnName("exercise_name");
            entity.Property(e => e.SportId).HasColumnName("sport_id");

            entity.HasOne(d => d.Sport).WithMany(p => p.Exercises)
                .HasForeignKey(d => d.SportId)
                .HasConstraintName("exercises_sport_id_fkey");
        });

        modelBuilder.Entity<Goal>(entity =>
        {
            entity.HasKey(e => e.GoalId).HasName("goals_pkey");

            entity.ToTable("goals");

            entity.Property(e => e.GoalId).HasColumnName("goal_id");
            entity.Property(e => e.Achieved).HasColumnName("achieved");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");
            entity.Property(e => e.GoalDescription).HasColumnName("goal_description");
            entity.Property(e => e.GoalName)
                .HasMaxLength(255)
                .HasColumnName("goal_name");
            entity.Property(e => e.SportId).HasColumnName("sport_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.TargetUnit)
                .HasMaxLength(50)
                .HasColumnName("target_unit");
            entity.Property(e => e.TargetValue).HasColumnName("target_value");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Exercise).WithMany(p => p.Goals)
                .HasForeignKey(d => d.ExerciseId)
                .HasConstraintName("goals_exercise_id_fkey");

            entity.HasOne(d => d.Sport).WithMany(p => p.Goals)
                .HasForeignKey(d => d.SportId)
                .HasConstraintName("goals_sport_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Goals)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("goals_user_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Sport>(entity =>
        {
            entity.HasKey(e => e.SportId).HasName("sports_pkey");

            entity.ToTable("sports");

            entity.Property(e => e.SportId).HasColumnName("sport_id");
            entity.Property(e => e.SportName)
                .HasMaxLength(255)
                .HasColumnName("sport_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Dateofbirth).HasColumnName("dateofbirth");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Genre)
                .HasMaxLength(10)
                .HasColumnName("genre");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("users_role_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
