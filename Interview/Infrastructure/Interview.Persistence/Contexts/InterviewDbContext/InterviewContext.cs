﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Interview.Domain.Entities.Models;
using Interview.Persistence.ServiceExtensions;
using Microsoft.EntityFrameworkCore;

namespace Interview.Persistence.Contexts.InterviewDbContext;

public partial class InterviewContext : DbContext
{
    public InterviewContext()
    {
    }

    public InterviewContext(DbContextOptions<InterviewContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<CandidateQuestion> CandidateQuestions { get; set; }

    public virtual DbSet<CandidateVacancy> CandidateVacancies { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<JobDegree> JobDegrees { get; set; }

    public virtual DbSet<Level> Levels { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Sector> Sectors { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Vacancy> Vacancies { get; set; }

    public virtual DbSet<Value> Values { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(ServiceExtension.ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Branch__3214EC07A6A5C030");

            entity.HasOne(d => d.Sector).WithMany(p => p.Branches)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SectorId_forBranch");
        });

        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Candidat__3214EC07392BCA18");
        });

        modelBuilder.Entity<CandidateQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Candidat__3214EC07058C4302");

            entity.HasOne(d => d.Candidate).WithMany(p => p.CandidateQuestions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CandidateId_forCandidateQuestion");

            entity.HasOne(d => d.Question).WithMany(p => p.CandidateQuestions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionId_forCandidateQuestion");
        });

        modelBuilder.Entity<CandidateVacancy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Candidat__3214EC079F7FA392");

            entity.HasOne(d => d.Candidate).WithMany(p => p.CandidateVacancies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CandidateId_forCandidateVacancy");

            entity.HasOne(d => d.Vacancy).WithMany(p => p.CandidateVacancies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VacancyId_forCandidateVacancy");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC0751326446");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC07599A2732");

            entity.HasOne(d => d.Branch).WithMany(p => p.Departments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BranchId_forDepartment");
        });

        modelBuilder.Entity<JobDegree>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobDegre__3214EC07732C6FCC");
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Level__3214EC07D6E00948");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC077A7CBCEC");

            entity.HasOne(d => d.Category).WithMany(p => p.Questions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoryId_forQuestion");

            entity.HasOne(d => d.Level).WithMany(p => p.Questions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LevelId_forQuestion");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Result__3214EC0730C0701E");

            entity.HasOne(d => d.Candidate).WithMany(p => p.Results)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CandidateId_forResult");
        });

        modelBuilder.Entity<Sector>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sector__3214EC070C13900C");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Session__3214EC07B312D4FA");

            entity.HasOne(d => d.Question).WithMany(p => p.Sessions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionId_forQuestionSession");

            entity.HasOne(d => d.Value).WithMany(p => p.Sessions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ValueId_forQuestionSession");
        });

        modelBuilder.Entity<Vacancy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vacancy__3214EC0725ECE4C7");

            entity.HasOne(d => d.JobDegree).WithMany(p => p.Vacancies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobDegreeId_forVacancy");

            entity.HasOne(d => d.Sector).WithMany(p => p.Vacancies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SectorId_forVacancy");
        });

        modelBuilder.Entity<Value>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Value__3214EC07DE1599BF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}