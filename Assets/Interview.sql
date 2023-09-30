CREATE DATABASE Interview;


USE Interview;


-- Bölmə
CREATE TABLE [Sector]
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [SectorName] NVARCHAR(max) NOT NULL
);


-- Şöbə
CREATE TABLE [Branch]
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [BranchName] NVARCHAR(max) NOT NULL,

   [SectorId] INT NOT NULL,

   CONSTRAINT [FK_SectorId_forBranch] FOREIGN KEY ([SectorId]) REFERENCES [Sector] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);


-- Departament
CREATE TABLE [Department]
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [DepartmentName] NVARCHAR(max) NOT NULL,

   [BranchId] INT NOT NULL,

   CONSTRAINT [FK_BranchId_forDepartment] FOREIGN KEY ([BranchId]) REFERENCES [Branch] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);


-- İş səviyyəsi
CREATE TABLE [JobDegree]
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [Degree] NVARCHAR(max) NOT NULL
);


-- Vakansiya
CREATE TABLE [Vacancy]
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [Title] NVARCHAR(max) NOT NULL,
   [Description] NVARCHAR(max) NOT NULL,

   [JobDegreeId] INT NOT NULL,
   [SectorId] INT NOT NULL,

   CONSTRAINT [FK_JobDegreeId_forVacancy] FOREIGN KEY ([JobDegreeId]) REFERENCES [JobDegree] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
   CONSTRAINT [FK_SectorId_forVacancy] FOREIGN KEY ([SectorId]) REFERENCES [Sector] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);


-- Namizəd
CREATE TABLE [Candidate]
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [CandidateSurname] NVARCHAR(max) NOT NULL,
   [CandidateName] NVARCHAR(max) NOT NULL,
   [CandidatePhonenumber] NVARCHAR(max) NOT NULL,
   [CandidateEmail] NVARCHAR(max) NOT NULL,
   [CurriculumVitae] NVARCHAR(max) NOT NULL,
   [Address] NVARCHAR(max) NOT NULL
);


-- Sual Dəyəri
CREATE TABLE [Value]
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [Value] INT NOT NULL CHECK ([Value] >= 0 AND [Value] <= 5)
);


-- Sual səviyyəsi
CREATE TABLE [Level]
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [Level] NVARCHAR(max) NOT NULL,
   [Coefficient] NVARCHAR(max) NOT NULL
);


-- Sual Kateqoriyası
CREATE TABLE [Category]
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [Category] NVARCHAR(max) NOT NULL
);


-- Sual
CREATE TABLE [Question]
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [QuestionTitle] NVARCHAR(max) NOT NULL,
   [QuestionText] NVARCHAR(max) NOT NULL,

   [LevelId] INT NOT NULL,
   [CategoryId] INT NOT NULL,

   CONSTRAINT [FK_LevelId_forQuestion] FOREIGN KEY ([LevelId]) REFERENCES [Level] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
   CONSTRAINT [FK_CategoryId_forQuestion] FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);


-- Sual üçün Sesiya
CREATE TABLE [Session]
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [SessionTime] datetime2 DEFAULT NULL,

   [QuestionId] INT NOT NULL,
   [ValueId] INT NOT NULL,

   CONSTRAINT [FK_QuestionId_forQuestionSession] FOREIGN KEY ([QuestionId]) REFERENCES [Question] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
   CONSTRAINT [FK_ValueId_forQuestionSession] FOREIGN KEY ([ValueId]) REFERENCES [Value] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);


-- NamizədSual
CREATE TABLE [CandidateQuestion]
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,

   [CandidateId] INT NOT NULL,
   [QuestionId] INT NOT NULL,

   CONSTRAINT [FK_CandidateId_forCandidateQuestion] FOREIGN KEY ([CandidateId]) REFERENCES [Candidate] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
   CONSTRAINT [FK_QuestionId_forCandidateQuestion] FOREIGN KEY ([QuestionId]) REFERENCES [Question] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);


-- NamizədVakansiya
CREATE TABLE [CandidateVacancy]
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,

   [CandidateId] INT NOT NULL,
   [VacancyId] INT NOT NULL,

   CONSTRAINT [FK_CandidateId_forCandidateVacancy] FOREIGN KEY ([CandidateId]) REFERENCES [Candidate] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
   CONSTRAINT [FK_VacancyId_forCandidateVacancy] FOREIGN KEY ([VacancyId]) REFERENCES [Vacancy] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);


-- Namizədin nəticəsi
CREATE TABLE [Result]
(
   [Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
   [Result] FLOAT NOT NULL DEFAULT (0.0),

   [CandidateId] INT NOT NULL,

   CONSTRAINT [FK_CandidateId_forResult] FOREIGN KEY ([CandidateId]) REFERENCES [Candidate] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);
