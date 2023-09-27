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
 

   [SectorId_forBranch]  int NOT NULL,

   Constraint [FK_SectorId_forBranch]  Foreign key ([SectorId_forBranch]) References  [Sector]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,

)

-- Departament
CREATE TABLE [Department]   
(

   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [DepartmentName] NVARCHAR(max) NOT NULL,
 

   
   [BranchId_forDepartment]  int NOT NULL,

   Constraint [FK_BranchId_forDepartment]  Foreign key ([BranchId_forDepartment]) References  [Branch]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,

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
 

   [JobDegreeId_forVacancy]  int NOT NULL,
   [DepartmentId_forVacancy]  int NOT NULL,

   Constraint [FK_JobDegreeId_forVacancy]  Foreign key ([JobDegreeId_forVacancy]) References  [JobDegree]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,
   Constraint [FK_DepartmentId_forVacancy]  Foreign key ([DepartmentId_forVacancy]) References  [Department]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,

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
 
)


-- Sual Kateqoriyası
CREATE TABLE [QuestionCategory]   
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [QuestionCategory] NVARCHAR(max) NOT NULL,
 
)


-- Açıq Sual
CREATE TABLE [OpenQuestion]   
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [Question] NVARCHAR(max) NOT NULL,
 
   Result bit DEFAULT 0
)


-- Sual
CREATE TABLE [Question]   
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
 

   [OpenQuestionId_forQuestion]  int NOT NULL,
   [QuestionValueId_forQuestion]  int NOT NULL,
   [QuestionLevelId_forQuestion]  int NOT NULL,
   [QuestionCategoryId_forQuestion]  int NOT NULL,

   Constraint [FK_OpenQuestionId_forQuestion]  Foreign key ([OpenQuestionId_forQuestion]) References  [OpenQuestion]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,
   Constraint [FK_QuestionValueId_forQuestion]  Foreign key ([QuestionValueId_forQuestion]) References  [QuestionValue]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,
   Constraint [FK_QuestionLevelId_forQuestion]  Foreign key ([QuestionLevelId_forQuestion]) References  [QuestionLevel]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,
   Constraint [FK_QuestionCategoryId_forQuestion]  Foreign key ([QuestionCategoryId_forQuestion]) References  [QuestionCategory]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,
)


-- NamizədSual
CREATE TABLE [CandidateQuestion]   
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
 

   [CandidateId_forCandidateQuestion]  int NOT NULL,
   [QuestionId_forCandidateQuestion]  int NOT NULL,

   Constraint [FK_CandidateId_forCandidateQuestion]  Foreign key ([CandidateId_forCandidateQuestion]) References  [Candidate]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,
   Constraint [FK_QuestionId_forCandidateQuestion]  Foreign key ([QuestionId_forCandidateQuestion]) References  [Question]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,
)


-- NamizədVakansiya
CREATE TABLE [CandidateVacancy]   
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
 

   [CandidateId_forCandidateVacancy]  int NOT NULL,
   [VacancyId_forCandidateVacancy]  int NOT NULL,

   Constraint [FK_CandidateId_forCandidateVacancy]  Foreign key ([CandidateId_forCandidateVacancy]) References  [Candidate]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,
   Constraint [FK_VacancyId_forCandidateVacancy]  Foreign key ([VacancyId_forCandidateVacancy]) References  [Vacancy]  ( [Id] ) On Delete NO ACTION On Update NO ACTION,
)
