CREATE DATABASE  Interview 

USE  Interview

-- Bölmə
CREATE TABLE [Sector]   
(

   [IdSector] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [SectorName] NVARCHAR(max) NOT NULL,

)


-- Şöbə
CREATE TABLE [Branch]   
(

   [IdBranch] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [BranchName] NVARCHAR(max) NOT NULL,

)

-- Departament
CREATE TABLE [Department]   
(

   [IdDepartment] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [DepartmentName] NVARCHAR(max) NOT NULL,

)


-- İş səviyyəsi
CREATE TABLE [JobDegree]   
(

   [IdJobDegree] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [Degree] NVARCHAR(max) NOT NULL,

)


-- Vakansiya
CREATE TABLE [Vacancy]   
(

   [IdVacancy] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [Title]  NVARCHAR(max) NOT NULL,
   [Description]  NVARCHAR(max) NOT NULL,

   [JobDegreeId_forVacancy]  int NOT NULL,
   [DepartmentId_forVacancy]  int NOT NULL,

   Constraint [FK_JobDegreeId_forVacancy]  Foreign key ([JobDegreeId_forVacancy]) References  [JobDegree]  ( [IdJobDegree] ) On Delete NO ACTION On Update NO ACTION,
   Constraint [FK_DepartmentId_forVacancy]  Foreign key ([DepartmentId_forVacancy]) References  [Department]  ( [IdDepartment] ) On Delete NO ACTION On Update NO ACTION,

)


-- Namizəd
CREATE TABLE [Candidate]   
(
   [IdCandidate] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
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
   [IdQuestionValue] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [QuestionValue] INT NOT NULL CHECK ([QuestionValue] >= 0 AND [QuestionValue] <= 5),
)


-- Sual səviyyəsi
CREATE TABLE [QuestionLevel]   
(
   [IdQuestionLevel] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [QuestionLevel] NVARCHAR(max) NOT NULL,
)


-- Sual Kateqoriyası
CREATE TABLE [QuestionCategory]   
(
   [IdQuestionCategory] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [QuestionCategory] NVARCHAR(max) NOT NULL,
)


-- Açıq Sual
CREATE TABLE [OpenQuestion]   
(
   [IdOpenQuestion] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [Question] NVARCHAR(max) NOT NULL,
   Result bit DEFAULT 0
)


-- Sual
CREATE TABLE [Question]   
(
   [IdQuestion] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,

   [OpenQuestionId_forQuestion]  int NOT NULL,
   [QuestionValueId_forQuestion]  int NOT NULL,
   [QuestionLevelId_forQuestion]  int NOT NULL,
   [QuestionCategoryId_forQuestion]  int NOT NULL,

   Constraint [FK_OpenQuestionId_forQuestion]  Foreign key ([OpenQuestionId_forQuestion]) References  [OpenQuestion]  ( [IdOpenQuestion] ) On Delete NO ACTION On Update NO ACTION,
   Constraint [FK_QuestionValueId_forQuestion]  Foreign key ([QuestionValueId_forQuestion]) References  [QuestionValue]  ( [IdQuestionValue] ) On Delete NO ACTION On Update NO ACTION,
   Constraint [FK_QuestionLevelId_forQuestion]  Foreign key ([QuestionLevelId_forQuestion]) References  [QuestionLevel]  ( [IdQuestionLevel] ) On Delete NO ACTION On Update NO ACTION,
   Constraint [FK_QuestionCategoryId_forQuestion]  Foreign key ([QuestionCategoryId_forQuestion]) References  [QuestionCategory]  ( [IdQuestionCategory] ) On Delete NO ACTION On Update NO ACTION,
)


-- NamizədSual
CREATE TABLE [CandidateQuestion]   
(
   [IdCandidateQuestion] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,

   [CandidateId_forCandidateQuestion]  int NOT NULL,
   [QuestionId_forCandidateQuestion]  int NOT NULL,

   Constraint [FK_CandidateId_forCandidateQuestion]  Foreign key ([CandidateId_forCandidateQuestion]) References  [Candidate]  ( [IdCandidate] ) On Delete NO ACTION On Update NO ACTION,
   Constraint [FK_QuestionId_forCandidateQuestion]  Foreign key ([QuestionId_forCandidateQuestion]) References  [Question]  ( [IdQuestion] ) On Delete NO ACTION On Update NO ACTION,
)


-- NamizədVakansiya
CREATE TABLE [CandidateVacancy]   
(
   [IdCandidateVacancy] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,

   [CandidateId_forCandidateVacancy]  int NOT NULL,
   [VacancyId_forCandidateVacancy]  int NOT NULL,

   Constraint [FK_CandidateId_forCandidateVacancy]  Foreign key ([CandidateId_forCandidateVacancy]) References  [Candidate]  ( [IdCandidate] ) On Delete NO ACTION On Update NO ACTION,
   Constraint [FK_VacancyId_forCandidateVacancy]  Foreign key ([VacancyId_forCandidateVacancy]) References  [Vacancy]  ( [IdVacancy] ) On Delete NO ACTION On Update NO ACTION,
)
