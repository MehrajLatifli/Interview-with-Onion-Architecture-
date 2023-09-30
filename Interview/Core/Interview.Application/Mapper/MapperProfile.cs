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
            CreateMap<Sector, SectorDTO_forCreate>();
            CreateMap<SectorDTO_forCreate, Sector>();
            CreateMap<Sector, SectorDTO_forUpdate>();
            CreateMap<SectorDTO_forUpdate, Sector>();
            CreateMap<Sector, SectorDTO_forGetandGetAll>()
                       .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                       .ForMember(dest => dest.SectorName, opt => opt.MapFrom(src => src.SectorName));
            CreateMap<SectorDTO_forGetandGetAll, Sector>()
                      .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                      .ForMember(dest => dest.SectorName, opt => opt.MapFrom(src => src.SectorName));

            CreateMap<Branch, BranchDTO_forCreate>();
            CreateMap<BranchDTO_forCreate, Branch>();
            CreateMap<Branch, BranchDTO_forUpdate>();
            CreateMap<BranchDTO_forUpdate, Branch>();
            CreateMap<Branch, BranchDTO_forGetandGetAll>();
            CreateMap<BranchDTO_forGetandGetAll, Branch>();

            CreateMap<Department, DepartmentDTO_forCreate>();
            CreateMap<DepartmentDTO_forCreate, Department>();
            CreateMap<Department, DepartmentDTO_forUpdate>();
            CreateMap<DepartmentDTO_forUpdate, Department>();
            CreateMap<Department, DepartmentDTO_forGetandGetAll>();
            CreateMap<DepartmentDTO_forGetandGetAll, Department>();

  
        }
    }
}
