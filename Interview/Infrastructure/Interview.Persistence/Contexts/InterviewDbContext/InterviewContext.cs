
using System;
using System.Collections.Generic;
using Interview.Domain.Entities.IdentityAuth;
using Interview.Domain.Entities.Models;
using Interview.Domain.EntityFrameworkConfigurations;
using Interview.Persistence.ServiceExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Interview.Persistence.Contexts.InterviewDbContext;

public class InterviewContext : IdentityDbContext<CustomUser, CustomRole, int, CustomUserClaim, CustomUserRole, CustomUserLogin, CustomRoleClaim, CustomUserToken>
{
    public InterviewContext()
    {
    }

    public InterviewContext(DbContextOptions<InterviewContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CustomUser>  CustomUsers { get; set; }
    public virtual DbSet<CustomRole>  CustomRoles { get; set; }

    public virtual DbSet<CustomUserLogin>  CustomUserLogins { get; set; }
    public virtual DbSet<CustomRoleClaim>  CustomRoleClaims { get; set; }

    public virtual DbSet<CustomUserToken>  CustomUserTokens { get; set; }

    public virtual DbSet<CustomUserRole>  CustomUserRoles { get; set; }




    public virtual DbSet<Candidate> Candidate { get; set; }

    public virtual DbSet<CandidateDocument> CandidateDocument { get; set; }

    public virtual DbSet<Category> Category { get; set; }

    public virtual DbSet<Level> Level { get; set; }

    public virtual DbSet<Position> Position { get; set; }

    public virtual DbSet<Question> Question { get; set; }

    public virtual DbSet<Session> Session { get; set; }

    public virtual DbSet<SessionQuestion> SessionQuestion { get; set; }

    public virtual DbSet<Structure> Structure { get; set; }

    public virtual DbSet<StructureType> StructureType { get; set; }

    public virtual DbSet<Vacancy> Vacancy { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(ServiceExtension.CustomDbConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityUser<int>>().HasNoKey();
        modelBuilder.Entity<IdentityUserRole<int>>().HasNoKey();
        modelBuilder.Entity<IdentityRole<int>>().HasNoKey();
        modelBuilder.Entity<IdentityUserClaim<int>>().HasNoKey();
        modelBuilder.Entity<IdentityUserLogin<int>>().HasNoKey();
        modelBuilder.Entity<IdentityUserToken<int>>().HasNoKey();
        modelBuilder.Entity<IdentityRoleClaim<int>>().HasNoKey();



        modelBuilder.Ignore<IdentityUser<int>>();
        modelBuilder.Ignore<IdentityUserRole<int>>();
        modelBuilder.Ignore<IdentityRole<int>>();
        modelBuilder.Ignore<IdentityUserClaim<int>>();
        modelBuilder.Ignore<IdentityUserLogin<int>>();
        modelBuilder.Ignore<IdentityUserToken<int>>();
        modelBuilder.Ignore<IdentityRoleClaim<int>>();


        modelBuilder.ApplyConfiguration(new CustomUserConfiguration());
        modelBuilder.ApplyConfiguration(new CustomRoleConfiguration());
        modelBuilder.ApplyConfiguration(new CustomUserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new CustomUserLoginConfiguration());
        modelBuilder.ApplyConfiguration(new CustomUserTokenConfiguration());


        modelBuilder.ApplyConfiguration(new CandidateConfiguration());
        modelBuilder.ApplyConfiguration(new CandidateDocumentConfiguration());
        modelBuilder.ApplyConfiguration(new LevelConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionConfiguration());
        modelBuilder.ApplyConfiguration(new SessionQuestionConfiguration());
        modelBuilder.ApplyConfiguration(new SessionConfiguration());
        modelBuilder.ApplyConfiguration(new StructureConfiguration());
        modelBuilder.ApplyConfiguration(new VacancyConfiguration());
        modelBuilder.ApplyConfiguration(new StructureConfiguration());
        modelBuilder.ApplyConfiguration(new PositionConfiguration());

    }

  
}