using AutoMapper;
using Interview.Application.Mapper.AuthDTO;
using Interview.Application.Mapper.DTO;
using Interview.Domain.Entities.AuthModels;
using Interview.Domain.Entities.Models;
using NuGet.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interview.Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<CandidateDocument, CandidateDocumentDTO_forCreate>();
            CreateMap<CandidateDocumentDTO_forCreate, CandidateDocument>();
            CreateMap<CandidateDocument, CandidateDocumentDTO_forUpdate>();
            CreateMap<CandidateDocumentDTO_forUpdate, CandidateDocument>();
            CreateMap<CandidateDocument, CandidateDocumentDTO_forGetandGetAll>();
            CreateMap<CandidateDocumentDTO_forGetandGetAll, CandidateDocument>();

            CreateMap<Candidate, CandidateDTO_forCreate>();
            CreateMap<CandidateDTO_forCreate, Candidate>();
            CreateMap<Candidate, CandidateDTO_forUpdate>();
            CreateMap<CandidateDTO_forUpdate, Candidate>();
            CreateMap<Candidate, CandidateDTO_forGetandGetAll>();
            CreateMap<CandidateDTO_forGetandGetAll, Candidate>();

            CreateMap<Level, LevelDTO_forCreate>();
            CreateMap<LevelDTO_forCreate, Level>();
            CreateMap<Level, LevelDTO_forUpdate>();
            CreateMap<LevelDTO_forUpdate, Level>();
            CreateMap<Level, LevelDTO_forGetandGetAll>();
            CreateMap<LevelDTO_forGetandGetAll, Level>();

            CreateMap<Category, CategoryDTO_forCreate>();
            CreateMap<CategoryDTO_forCreate, Category>();
            CreateMap<Category, CategoryDTO_forUpdate>();
            CreateMap<CategoryDTO_forUpdate, Category>();
            CreateMap<Category, CategoryDTO_forGetandGetAll>();
            CreateMap<CategoryDTO_forGetandGetAll, Category>();

            CreateMap<Question, QuestionDTO_forCreate>();
            CreateMap<QuestionDTO_forCreate, Question>();
            CreateMap<Question, QuestionDTO_forUpdate>();
            CreateMap<QuestionDTO_forUpdate, Question>();
            CreateMap<Question, QuestionDTO_forGetandGetAll>();
            CreateMap<QuestionDTO_forGetandGetAll, Question>();

            CreateMap<SessionQuestion, SessionQuestionDTO_forCreate>();
            CreateMap<SessionQuestionDTO_forCreate, SessionQuestion>();
            CreateMap<SessionQuestion, SessionQuestionDTO_forUpdate>();
            CreateMap<SessionQuestionDTO_forUpdate, SessionQuestion>();
            CreateMap<SessionQuestion, SessionQuestionDTO_forGetandGetAll>();
            CreateMap<SessionQuestionDTO_forGetandGetAll, SessionQuestion>();

            CreateMap<Session, SessionDTO_forCreate>();
            CreateMap<SessionDTO_forCreate, Session>();
            CreateMap<Session, SessionDTO_forUpdate>();
            CreateMap<SessionDTO_forUpdate, Session>();
            CreateMap<Session, SessionDTO_forGetandGetAll>();
            CreateMap<SessionDTO_forGetandGetAll, Session>();

            CreateMap<Structure, StructureDTO_forCreate>();
            CreateMap<StructureDTO_forCreate, Structure>();
            CreateMap<Structure, StructureDTO_forUpdate>();
            CreateMap<StructureDTO_forUpdate, Structure>();
            CreateMap<Structure, StructureDTO_forGetandGetAll>();
            CreateMap<StructureDTO_forGetandGetAll, Structure>();

            CreateMap<StructureType, StructureTypeDTO_forCreate>();
            CreateMap<StructureTypeDTO_forCreate, StructureType>();
            CreateMap<StructureType, StructureTypeDTO_forUpdate>();
            CreateMap<StructureTypeDTO_forUpdate, StructureType>();
            CreateMap<StructureType, StructureTypeDTO_forGetandGetAll>();
            CreateMap<StructureTypeDTO_forGetandGetAll, StructureType>();

            CreateMap<Vacancy, VacancyDTO_forCreate>();
            CreateMap<VacancyDTO_forCreate, Vacancy>();
            CreateMap<Vacancy, VacancyDTO_forUpdate>();
            CreateMap<VacancyDTO_forUpdate, Vacancy>();
            CreateMap<Vacancy, VacancyDTO_forGetandGetAll>();
            CreateMap<VacancyDTO_forGetandGetAll, Vacancy>();

            CreateMap<Position, PositionDTO_forCreate>();
            CreateMap<PositionDTO_forCreate, Position>();
            CreateMap<Position, PositionDTO_forUpdate>();
            CreateMap<PositionDTO_forUpdate, Position>();
            CreateMap<Position, PositionDTO_forGetandGetAll>();
            CreateMap<PositionDTO_forGetandGetAll, Position>();




            CreateMap<Login, LoginDTO>();
            CreateMap<LoginDTO, Login>();

            CreateMap<LoginResponse, LoginResponseDTO>();
            CreateMap<LoginResponseDTO, LoginResponse>();

            CreateMap<Register, RegisterDTO>();
            CreateMap<RegisterDTO, Register>();

            CreateMap<UpdatePassword, UpdatePasswordDTO>();
            CreateMap<UpdatePasswordDTO, UpdatePassword>();

            CreateMap<UpdateProfile, UpdateProfileDTO>();
            CreateMap<UpdateProfileDTO, UpdateProfile>();

            CreateMap<RoleAccessType, RoleAccessTypeDTO>();
            CreateMap<RoleAccessTypeDTO, RoleAccessType>();
        }
    }
}
