﻿using AutoMapper;
using Interview.Application.Exception;
using Interview.Application.Mapper.DTO;
using Interview.Application.Repositories.Custom;
using Interview.Application.Services.Abstract;
using Interview.Domain.Entities.Models;

namespace Interview.Application.Services.Concrete
{
    public class CandidateServiceManager : ICandidateService
    {

        public readonly IMapper _mapper;



        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;
        private readonly ICandidateDocumentWriteRepository _candidateDocumentWriteRepository;
        private readonly ICandidateDocumentReadRepository _candidateDocumentReadRepository;

        public CandidateServiceManager(IMapper mapper, ICandidateWriteRepository candidateWriteRepository, ICandidateReadRepository candidateReadRepository, ICandidateDocumentWriteRepository candidateDocumentWriteRepository, ICandidateDocumentReadRepository candidateDocumentReadRepository)
        {
            _mapper = mapper;
            _candidateWriteRepository = candidateWriteRepository;
            _candidateReadRepository = candidateReadRepository;
            _candidateDocumentWriteRepository = candidateDocumentWriteRepository;
            _candidateDocumentReadRepository = candidateDocumentReadRepository;
        }





        #region Candidate service manager

        public async Task CandidateCreate(CandidateDTO_forCreate model)
        {



            var entity = _mapper.Map<Candidate>(model);

            var existing = await _candidateDocumentReadRepository.GetByIdAsync(model.CandidateDocumentId.ToString(), false);

            if (existing is null)
            {
                throw new NotFoundException("CandidateDocument not found");

            }

            entity = new Candidate
            {
                CandidateDocumentId = model.CandidateDocumentId,

            };


            await _candidateWriteRepository.AddAsync(entity);

            await _candidateWriteRepository.SaveAsync();
        }

        public async Task<List<CandidateDTO_forGetandGetAll>> GetCandidate()
        {
            List<CandidateDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<CandidateDTO_forGetandGetAll>>(_candidateReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("Candidate not found");
            }

            return datas;
        }

        public async Task<CandidateDTO_forGetandGetAll> GetCandidateById(int id)
        {
            CandidateDTO_forGetandGetAll item = null;


            item = _mapper.Map<CandidateDTO_forGetandGetAll>(await _candidateReadRepository.GetByIdAsync(id.ToString(), false));


            if (item == null)
            {
                throw new NotFoundException("Candidate not found");
            }

            return item;
        }

        public async Task CandidateUpdate(CandidateDTO_forUpdate model)
        {

            var existing = await _candidateReadRepository.GetByIdAsync(model.Id.ToString(), false);

            var existing2 = await _candidateDocumentReadRepository.GetByIdAsync(model.CandidateDocumentId.ToString(), false);

            if (existing is null)
            {
                throw new NotFoundException("Candidate not found");

            }



            if (existing2 is null)
            {
                throw new NotFoundException("CandidateDocument not found");

            }

            var entity = _mapper.Map<Candidate>(model);

            entity = new Candidate
            {
                Id = model.Id,
                CandidateDocumentId = model.CandidateDocumentId,

            };

            _candidateWriteRepository.Update(entity);
            await _candidateWriteRepository.SaveAsync();

        }

        public async Task<CandidateDTO_forGetandGetAll> DeleteCandidateById(int id)
        {

            if (_candidateReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _candidateWriteRepository.RemoveByIdAsync(id.ToString());
                await _candidateWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("Candidate not found");
            }
        }

        #endregion


    }


}