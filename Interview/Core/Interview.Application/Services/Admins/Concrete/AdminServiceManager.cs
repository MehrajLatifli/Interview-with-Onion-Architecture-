using AutoMapper;
using Interview.Application.Exception;
using Interview.Application.Mapper.DTO;
using Interview.Application.Repositories.Custom;
using Interview.Application.Services.Admins.Abstract;
using Interview.Domain.Entities.Models;

namespace Interview.Application.Services.Admins.Concrete
{
    public class AdminServiceManager : IAdminService
    {

        private readonly ISectorWriteRepository _sectorWriteRepository;
        private readonly ISectorReadRepository _sectorReadRepository;
        public readonly IMapper _mapper;
        public AdminServiceManager(ISectorWriteRepository sectorWriteRepository, IMapper mapper, ISectorReadRepository sectorReadRepository)
        {
            _sectorWriteRepository = sectorWriteRepository;
            _mapper = mapper;
            _sectorReadRepository = sectorReadRepository;
        }
        public async Task SectorCreate(SectorDTO_forCreate model)
        {

            var entity = _mapper.Map<Sector>(model);

            await _sectorWriteRepository.AddAsync(entity);

            await _sectorWriteRepository.SaveAsync();
        }

        public async Task<List<SectorDTO_forGetandGetAll>> GetSector()
        {
            List<SectorDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<SectorDTO_forGetandGetAll>>(_sectorReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("Sector not found");
            }

            return datas;
        }

        public async Task<SectorDTO_forGetandGetAll> GetSectorById(int id)
        {
            SectorDTO_forGetandGetAll item = null;

        
            item = _mapper.Map<SectorDTO_forGetandGetAll>(await _sectorReadRepository.GetByIdAsync(id.ToString(), false));
  

            if (item == null)
            {
                throw new NotFoundException("Sector not found");
            }

            return item;
        }

        public async Task SectorUpdate(SectorDTO_forUpdate model)
        {

            var existing = await _sectorReadRepository.GetByIdAsync(model.Id.ToString(), false);


            if (existing is null)
            {
                throw new NotFoundException("Sector not found");

            }


            var update = new Sector
            {
                Id = model.Id,
                SectorName = model.SectorName,

            };

            _sectorWriteRepository.Update(update);
            await _sectorWriteRepository.SaveAsync();

        }

        public async Task<SectorDTO_forGetandGetAll> DeleteSectorById(int id)
        {

            if (_sectorReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _sectorWriteRepository.RemoveByIdAsync(id.ToString());
                await _sectorWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("Sector not found");
            }
        }
    }
}
