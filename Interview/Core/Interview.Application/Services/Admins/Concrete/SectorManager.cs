using AutoMapper;
using Interview.Application.Mapper.DTO;
using Interview.Application.Repositories.Custom;
using Interview.Application.Services.Admins.Abstract;
using Interview.Domain.Entities.Models;

namespace Interview.Application.Services.Admins.Concrete
{
    public class SectorManager : ISectorService
    {

        private readonly ISectorWriteRepository _sectorWriteRepository;
        public readonly IMapper _mapper;
        public SectorManager(ISectorWriteRepository sectorWriteRepository, IMapper mapper)
        {
            _sectorWriteRepository = sectorWriteRepository;
            _mapper = mapper;
        }
        public async Task SectorCreate(SectorDTO_forCreate model)
        {

            var entity = _mapper.Map<Sector>(model);

            await _sectorWriteRepository.AddAsync(entity);

            await _sectorWriteRepository.SaveAsync();
        }
    }
}
