using AutoMapper;
using Interview.Application.Mapper.DTO;
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

        }
    }
}
