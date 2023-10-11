using Interview.Domain.Entities.Models;

namespace Interview.API.API_Routes
{
    public static class Routes
    {
        public  const string CandidateDocument = "candidatedocument";

        public  const string  CandidateDocumentById = CandidateDocument + "/{id}";


        public const string Candidate = "candidate";

        public const string  CandidateById = Candidate + "/{id}";


        public const string Level = "level";

        public const string  LevelById = Level + "/{id}";


        public const string Category = "category";

        public const string  CategoryById = Category + "/{id}";


        public const string StructureType = "structureType";

        public const string  StructureTypeById = StructureType + "/{id}";


        public const string Structure = "structure";

        public const string  StructureById = Structure + "/{id}";


        public const string Position = "position";

        public const string  PositionById = Position + "/{id}";


        public const string Vacancy = "vacancy";

        public const string  VacancyById = Vacancy + "/{id}";


        public const string Session = "session";

        public const string  SessionById = Session + "/{id}";


        public const string Question = "question";

        public const string  QuestionById = Question + "/{id}";


        public const string SessionQuestion = "sessionQuestion";

        public const string SessionQuestionById = SessionQuestion + "/{id}";

        public const string RandomQuestion = "randomQuestion";

        public const string RandomQuestionById = RandomQuestion + "/{questionCount}/{positionId}/{vacantionId}/{sessionId}";
    }
}
