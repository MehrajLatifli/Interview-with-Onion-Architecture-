
#nullable disable
using System;
using System.Collections.Generic;
using Interview.Persistence.ServiceExtensions;
using Interview.Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Interview.Domain.EntityFrameworkConfigurations;

namespace Interview.Persistence.Contexts.InterviewDbContext;

public class InterviewContext : DbContext
{
    public InterviewContext()
    {
    }

    public InterviewContext(DbContextOptions<InterviewContext> options)
        : base(options)
    {
    }

    public  DbSet<Candidate> Candidates { get; set; }

    public  DbSet<CandidateDocument> CandidateDocuments { get; set; }

    public  DbSet<Level> Levels { get; set; }

    public  DbSet<Position> Positions { get; set; }

    public  DbSet<Question> Questions { get; set; }

    public  DbSet<Session> Sessions { get; set; }

    public  DbSet<SessionQuestion> SessionQuestions { get; set; }

    public  DbSet<SessionType> SessionTypes { get; set; }

    public  DbSet<Structure> Structures { get; set; }

    public  DbSet<StructureType> StructureTypes { get; set; }

    public  DbSet<Vacancy> Vacancies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer(ServiceExtension.ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfiguration(new CandidateConfiguration());

        modelBuilder.ApplyConfiguration(new CandidateDocumentConfiguration());

        modelBuilder.ApplyConfiguration(new LevelConfiguration());

        modelBuilder.ApplyConfiguration(new PositionConfiguration());

        modelBuilder.ApplyConfiguration(new QuestionConfiguration());

        modelBuilder.ApplyConfiguration(new SessionConfiguration());

        modelBuilder.ApplyConfiguration(new SessionQuestionConfiguration());

        modelBuilder.ApplyConfiguration(new SessionTypeConfiguration());

        modelBuilder.ApplyConfiguration(new StructureConfiguration());

        modelBuilder.ApplyConfiguration(new StructureTypeConfiguration());

        modelBuilder.ApplyConfiguration(new VacancyConfiguration());


        //modelBuilder.Entity<Candidate>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PK__Candidate");

        //    entity.HasOne(d => d.CandidateDocument).WithMany(p => p.Candidates)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("FK_CandidateDocument_forCandidates");
        //});

        //modelBuilder.Entity<CandidateDocument>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PK__CandidateDocument");
        //});

        //modelBuilder.Entity<Level>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PK__Level");
        //});

        //modelBuilder.Entity<Position>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PK__Position");
        //});

        //modelBuilder.Entity<Question>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PK__Question");

        //    entity.HasOne(d => d.Level).WithMany(p => p.Questions)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("FK_LevelId_forQuestion");

        //    entity.HasOne(d => d.SessionType).WithMany(p => p.Questions)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("FK_SessionTypeId_forQuestion");

        //    entity.HasOne(d => d.Structure).WithMany(p => p.Questions)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("FK_StructureId_forQuestion");
        //});

        //modelBuilder.Entity<Session>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PK__Session");

        //    entity.HasOne(d => d.Candidate).WithMany(p => p.Sessions)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("FK_CandidateId_forSession");

        //    entity.HasOne(d => d.Vacancy).WithMany(p => p.Sessions)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("FK_VacancyId_forSession");
        //});

        //modelBuilder.Entity<SessionQuestion>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PK__SessionQuestion");

        //    entity.HasOne(d => d.Question).WithMany(p => p.SessionQuestions)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("FK_QuestionId_forSessionQuestion");

        //    entity.HasOne(d => d.Session).WithMany(p => p.SessionQuestions)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("FK_SessionId_forSessionQuestion");
        //});

        //modelBuilder.Entity<SessionType>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PK__SessionType");
        //});

        //modelBuilder.Entity<Structure>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PK__Structure");

        //    entity.HasOne(d => d.StructureType).WithMany(p => p.Structures)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("FK_StructureType_forStructure");
        //});

        //modelBuilder.Entity<StructureType>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PK__StructureType");
        //});

        //modelBuilder.Entity<Vacancy>(entity =>
        //{
        //    entity.HasKey(e => e.Id).HasName("PK__Vacancy");

        //    entity.HasOne(d => d.Position).WithMany(p => p.Vacancies)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("FK_PositionId_forVacancy");

        //    entity.HasOne(d => d.Structure).WithMany(p => p.Vacancies)
        //        .OnDelete(DeleteBehavior.ClientSetNull)
        //        .HasConstraintName("FK_StructureId_forVacancy");
        //});

    }

}