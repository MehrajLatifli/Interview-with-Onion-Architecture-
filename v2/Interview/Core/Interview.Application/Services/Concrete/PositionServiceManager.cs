﻿using AutoMapper;
using Interview.Application.Exception;
using Interview.Application.Mapper.DTO;
using Interview.Application.Repositories.Custom;
using Interview.Application.Services.Abstract;
using Interview.Domain.Entities.Models;

namespace Interview.Application.Services.Concrete
{
    public class PositionServiceManager : IPositionService
    {

        public readonly IMapper _mapper;



        private readonly IPositionWriteRepository _positionWriteRepository;
        private readonly IPositionReadRepository _positionReadRepository;

        public PositionServiceManager(IMapper mapper, IPositionWriteRepository positionWriteRepository, IPositionReadRepository positionReadRepository)
        {
            _mapper = mapper;
            _positionWriteRepository = positionWriteRepository;
            _positionReadRepository = positionReadRepository;
        }






        #region Position service manager

        public async Task PositionCreate(PositionDTO_forCreate model)
        {



            var entity = _mapper.Map<Position>(model);

            entity = new Position
            {
                Name = model.Name,


            };


            await _positionWriteRepository.AddAsync(entity);

            await _positionWriteRepository.SaveAsync();
        }

        public async Task<List<PositionDTO_forGetandGetAll>> GetPosition()
        {
            List<PositionDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<PositionDTO_forGetandGetAll>>(_positionReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("Position not found");
            }

            return datas;
        }

        public async Task<PositionDTO_forGetandGetAll> GetPositionById(int id)
        {
            PositionDTO_forGetandGetAll item = null;


            item = _mapper.Map<PositionDTO_forGetandGetAll>(await _positionReadRepository.GetByIdAsync(id.ToString(), false));


            if (item == null)
            {
                throw new NotFoundException("Position not found");
            }

            return item;
        }

        public async Task PositionUpdate(PositionDTO_forUpdate model)
        {

            var existing = await _positionReadRepository.GetByIdAsync(model.Id.ToString(), false);


            if (existing is null)
            {
                throw new NotFoundException("Position not found");

            }



            if (existing is null)
            {
                throw new NotFoundException("Position not found");

            }

            var entity = _mapper.Map<Position>(model);

            entity = new Position
            {
                Id = model.Id,
                Name = model.Name,

            };

            _positionWriteRepository.Update(entity);
            await _positionWriteRepository.SaveAsync();

        }

        public async Task<PositionDTO_forGetandGetAll> DeletePositionById(int id)
        {

            if (_positionReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _positionWriteRepository.RemoveByIdAsync(id.ToString());
                await _positionWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("Position not found");
            }
        }

        #endregion






    }


}