CREATE DATABASE  Interview 

USE  Interview

-- Bölmə
CREATE TABLE [Sector]   
(

   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [SectorName] NVARCHAR(max) NOT NULL,
 
    

)


-- Şöbə
CREATE TABLE [Branch]   
(

   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [BranchName] NVARCHAR(max) NOT NULL,
 

   [SectorId]  int NOT NULL,

   Constraint [FK_SectorId_forBranch]  Foreign key ([SectorId]) References  [Sector]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,

)

-- Departament
CREATE TABLE [Department]   
(

   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [DepartmentName] NVARCHAR(max) NOT NULL,
 

   
   [BranchId]  int NOT NULL,

   Constraint [FK_BranchId_forDepartment]  Foreign key ([BranchId]) References  [Branch]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,

)


-- İş səviyyəsi
CREATE TABLE [JobDegree]   
(

   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [Degree] NVARCHAR(max) NOT NULL,
 

)


-- Vakansiya
CREATE TABLE [Vacancy]   
(

   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [Title]  NVARCHAR(max) NOT NULL,
   [Description]  NVARCHAR(max) NOT NULL,
 

   [JobDegreeId]  int NOT NULL,
   [SectorId]  int NOT NULL,

   Constraint [FK_JobDegreeId_forVacancy]  Foreign key ([JobDegreeId]) References  [JobDegree]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,
   Constraint [FK_DepartmentId_forVacancy]  Foreign key ([SectorId]) References  [Sector]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,

)


-- Namizəd
CREATE TABLE [Candidate]   
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [CandidateSurname] NVARCHAR(max) NOT NULL,
   [CandidateName] NVARCHAR(max) NOT NULL,
   [CandidatePhonenumber] NVARCHAR(max) NOT NULL,
   [CandidateEmail] NVARCHAR(max) NOT NULL,
   [CurriculumVitae] NVARCHAR(max) NOT NULL,
   [Address] NVARCHAR(max) NOT NULL,
 
)


-- Sual Dəyəri
CREATE TABLE [QuestionValue]   
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [QuestionValue] INT NOT NULL CHECK ([QuestionValue] >= 0 AND [QuestionValue] <= 5),
 
)


-- Sual səviyyəsi
CREATE TABLE [QuestionLevel]   
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [QuestionLevel] NVARCHAR(max) NOT NULL,
   [Coefficient] NVARCHAR(max) NOT NULL,
 
)


-- Sual Kateqoriyası
CREATE TABLE [QuestionCategory]   
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [QuestionCategory] NVARCHAR(max) NOT NULL,
 
)


-- Suala ayrılan vaxt
CREATE TABLE [QuestionSession]   
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [StartTime] datetime2 NOT NULL ,
   [FinishTime] datetime2 NOT NULL ,
   Result bit DEFAULT 0
)


-- Sual
CREATE TABLE [Question]   
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
 

   [QuestionSessionId]  int NOT NULL,
   [QuestionValueId]  int NOT NULL,
   [QuestionLevelId]  int NOT NULL,
   [QuestionCategoryId]  int NOT NULL,

   Constraint [FK_QuestionSessionId_forQuestion]  Foreign key ([QuestionSessionId]) References  [QuestionSession]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,
   Constraint [FK_QuestionValueId_forQuestion]  Foreign key ([QuestionValueId]) References  [QuestionValue]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,
   Constraint [FK_QuestionLevelId_forQuestion]  Foreign key ([QuestionLevelId]) References  [QuestionLevel]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,
   Constraint [FK_QuestionCategoryId_forQuestion]  Foreign key ([QuestionCategoryId]) References  [QuestionCategory]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,
)


-- NamizədSual
CREATE TABLE [CandidateQuestion]   
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
 

   [CandidateId]  int NOT NULL,
   [QuestionId]  int NOT NULL,

   Constraint [FK_CandidateId_forCandidateQuestion]  Foreign key ([CandidateId]) References  [Candidate]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,
   Constraint [FK_QuestionId_forCandidateQuestion]  Foreign key ([QuestionId]) References  [Question]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,
)


-- NamizədVakansiya
CREATE TABLE [CandidateVacancy]   
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
 

   [CandidateId]  int NOT NULL,
   [VacancyId]  int NOT NULL,

   Constraint [FK_CandidateId_forCandidateVacancy]  Foreign key ([CandidateId]) References  [Candidate]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,
   Constraint [FK_VacancyId_forCandidateVacancy]  Foreign key ([VacancyId]) References  [Vacancy]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,
)
