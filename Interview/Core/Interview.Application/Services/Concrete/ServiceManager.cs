using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Interview.Application.Exception;
using Interview.Application.Mapper.DTO;
using Interview.Application.Repositories.Custom;
using Interview.Application.Services.Abstract;
using Interview.Domain.Entities.Models;
using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;

namespace Interview.Application.Services.Concrete
{
    public class ServiceManager : IService
    {

        public readonly IMapper _mapper;

        private readonly ICandidateDocumentWriteRepository _candidateDocumentWriteRepository;
        private readonly ICandidateDocumentReadRepository _candidateDocumentReadRepository;

        private readonly ICandidateWriteRepository _candidateWriteRepository;
        private readonly ICandidateReadRepository _candidateReadRepository;

        private readonly ILevelWriteRepository _levelWriteRepository;
        private readonly ILevelReadRepository _levelReadRepository;

        private readonly ICategoryWriteRepository _sessionTypeWriteRepository;
        private readonly ICategoryReadRepository _sessionTypeReadRepository;

        private readonly IQuestionWriteRepository _questionWriteRepository;
        private readonly IQuestionReadRepository _questionReadRepository;

        private readonly ISessionQuestionWriteRepository _sessionQuestionWriteRepository;
        private readonly ISessionQuestionReadRepository _sessionQuestionReadRepository;

        private readonly ISessionWriteRepository _sessionWriteRepository;
        private readonly ISessionReadRepository _sessionReadRepository;

        private readonly IStructureWriteRepository _structureWriteRepository;
        private readonly IStructureReadRepository _structureReadRepository;

        private readonly IStructureTypeWriteRepository _structureTypeWriteRepository;
        private readonly IStructureTypeReadRepository _structureTypeReadRepository;

        private readonly IVacancyWriteRepository _vacancyWriteRepository;
        private readonly IVacancyReadRepository _vacancyReadRepository;

        private readonly IPositionWriteRepository _positionWriteRepository;
        private readonly IPositionReadRepository _positionReadRepository;

        public ServiceManager(IMapper mapper, ICandidateDocumentWriteRepository candidateDocumentWriteRepository, ICandidateDocumentReadRepository candidateDocumentReadRepository, ICandidateWriteRepository candidateWriteRepository, ICandidateReadRepository candidateReadRepository, ILevelWriteRepository levelWriteRepository, ILevelReadRepository levelReadRepository, ICategoryWriteRepository sessionTypeWriteRepository, ICategoryReadRepository sessionTypeReadRepository, IQuestionWriteRepository questionWriteRepository, IQuestionReadRepository questionReadRepository, ISessionQuestionWriteRepository sessionQuestionWriteRepository, ISessionQuestionReadRepository sessionQuestionReadRepository, ISessionWriteRepository sessionWriteRepository, ISessionReadRepository sessionReadRepository, IStructureWriteRepository structureWriteRepository, IStructureReadRepository structureReadRepository, IStructureTypeWriteRepository structureTypeWriteRepository, IStructureTypeReadRepository structureTypeReadRepository, IVacancyWriteRepository vacancyWriteRepository, IVacancyReadRepository vacancyReadRepository, IPositionWriteRepository positionWriteRepository, IPositionReadRepository positionReadRepository)
        {
            _mapper = mapper;
            _candidateDocumentWriteRepository = candidateDocumentWriteRepository;
            _candidateDocumentReadRepository = candidateDocumentReadRepository;
            _candidateWriteRepository = candidateWriteRepository;
            _candidateReadRepository = candidateReadRepository;
            _levelWriteRepository = levelWriteRepository;
            _levelReadRepository = levelReadRepository;
            _sessionTypeWriteRepository = sessionTypeWriteRepository;
            _sessionTypeReadRepository = sessionTypeReadRepository;
            _questionWriteRepository = questionWriteRepository;
            _questionReadRepository = questionReadRepository;
            _sessionQuestionWriteRepository = sessionQuestionWriteRepository;
            _sessionQuestionReadRepository = sessionQuestionReadRepository;
            _sessionWriteRepository = sessionWriteRepository;
            _sessionReadRepository = sessionReadRepository;
            _structureWriteRepository = structureWriteRepository;
            _structureReadRepository = structureReadRepository;
            _structureTypeWriteRepository = structureTypeWriteRepository;
            _structureTypeReadRepository = structureTypeReadRepository;
            _vacancyWriteRepository = vacancyWriteRepository;
            _vacancyReadRepository = vacancyReadRepository;
            _positionWriteRepository = positionWriteRepository;
            _positionReadRepository = positionReadRepository;
        }





        #region CandidateDocument service manager

        public async Task CandidateDocumentCreate(CandidateDocumentDTO_forCreate model, string AzureconnectionString)
        {


            string connectionString = AzureconnectionString;

            string azuriteConnectionString = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_AZURE_STORAGE_CONNECTION_STRING");
            if (!string.IsNullOrEmpty(azuriteConnectionString))
            {
                connectionString = azuriteConnectionString;
            }

            string containerName = "cv-files";

            string blobName = model.Name + "_" + model.Email + "_" + Guid.NewGuid().ToString() + Path.GetExtension(model.Cv.FileName);

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            using (Stream stream = model.Cv.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            string Url = blobClient.Uri.ToString();

            var entity = _mapper.Map<CandidateDocument>(model);

            entity = new CandidateDocument
            {
                Surname = model.Surname,
                Name = model.Name,
                Phonenumber = model.Phonenumber,
                Email = model.Email,
                Cv = Url,
                Address = model.Address,
                Patronymic = model.Patronymic,

            };


            await _candidateDocumentWriteRepository.AddAsync(entity);

            await _candidateDocumentWriteRepository.SaveAsync();
        }

        public async Task<List<CandidateDocumentDTO_forGetandGetAll>> GetCandidateDocument()
        {
            List<CandidateDocumentDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<CandidateDocumentDTO_forGetandGetAll>>(_candidateDocumentReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("CandidateDocument not found");
            }

            return datas;
        }

        public async Task<CandidateDocumentDTO_forGetandGetAll> GetCandidateDocumentById(int id)
        {
            CandidateDocumentDTO_forGetandGetAll item = null;


            item = _mapper.Map<CandidateDocumentDTO_forGetandGetAll>(await _candidateDocumentReadRepository.GetByIdAsync(id.ToString(), false));


            if (item == null)
            {
                throw new NotFoundException("CandidateDocument not found");
            }

            return item;
        }

        public async Task CandidateDocumentUpdate(CandidateDocumentDTO_forUpdate model, string AzureconnectionString)
        {

            var existing = await _candidateDocumentReadRepository.GetByIdAsync(model.Id.ToString(), false);


            if (existing is null)
            {
                throw new NotFoundException("CandidateDocument not found");

            }

            string connectionString = AzureconnectionString;

            string azuriteConnectionString = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_AZURE_STORAGE_CONNECTION_STRING");
            if (!string.IsNullOrEmpty(azuriteConnectionString))
            {
                connectionString = azuriteConnectionString;
            }

            string containerName = "cv-files";

            string blobName = model.Name + "_" + model.Email + "_" + Guid.NewGuid().ToString() + Path.GetExtension(model.Cv.FileName);

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            using (Stream stream = model.Cv.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            string Url = blobClient.Uri.ToString();



            if (existing is null)
            {
                throw new NotFoundException("CandidateDocument not found");

            }

            var entity = _mapper.Map<CandidateDocument>(model);

            entity = new CandidateDocument
            {
                Id = model.Id,
                Surname = model.Surname,
                Name = model.Name,
                Phonenumber = model.Phonenumber,
                Email = model.Email,
                Cv = Url,
                Address = model.Address,
                Patronymic = model.Patronymic,

            };

            _candidateDocumentWriteRepository.Update(entity);
            await _candidateDocumentWriteRepository.SaveAsync();

        }

        public async Task<CandidateDocumentDTO_forGetandGetAll> DeleteCandidateDocumentById(int id)
        {

            if (_candidateDocumentReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _candidateDocumentWriteRepository.RemoveByIdAsync(id.ToString());
                await _candidateDocumentWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("CandidateDocument not found");
            }
        }

        #endregion


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


        #region Level service manager

        public async Task LevelCreate(LevelDTO_forCreate model)
        {



            var entity = _mapper.Map<Level>(model);

            entity = new Level
            {
                Name = model.Name,
                Coefficient = model.Coefficient,

            };


            await _levelWriteRepository.AddAsync(entity);

            await _levelWriteRepository.SaveAsync();
        }

        public async Task<List<LevelDTO_forGetandGetAll>> GetLevel()
        {
            List<LevelDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<LevelDTO_forGetandGetAll>>(_levelReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("Level not found");
            }

            return datas;
        }

        public async Task<LevelDTO_forGetandGetAll> GetLevelById(int id)
        {
            LevelDTO_forGetandGetAll item = null;


            item = _mapper.Map<LevelDTO_forGetandGetAll>(await _levelReadRepository.GetByIdAsync(id.ToString(), false));


            if (item == null)
            {
                throw new NotFoundException("Level not found");
            }

            return item;
        }

        public async Task LevelUpdate(LevelDTO_forUpdate model)
        {

            var existing = await _levelReadRepository.GetByIdAsync(model.Id.ToString(), false);


            if (existing is null)
            {
                throw new NotFoundException("Level not found");

            }



            if (existing is null)
            {
                throw new NotFoundException("Level not found");

            }

            var entity = _mapper.Map<Level>(model);

            entity = new Level
            {
                Id = model.Id,
                Name = model.Name,
                Coefficient = model.Coefficient,

            };

            _levelWriteRepository.Update(entity);
            await _levelWriteRepository.SaveAsync();

        }

        public async Task<LevelDTO_forGetandGetAll> DeleteLevelById(int id)
        {

            if (_levelReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _levelWriteRepository.RemoveByIdAsync(id.ToString());
                await _levelWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("Level not found");
            }
        }

        #endregion


        #region StructureType service manager

        public async Task StructureTypeCreate(StructureTypeDTO_forCreate model)
        {



            var entity = _mapper.Map<StructureType>(model);

            entity = new StructureType
            {
                Name = model.Name,


            };


            await _structureTypeWriteRepository.AddAsync(entity);

            await _structureTypeWriteRepository.SaveAsync();
        }

        public async Task<List<StructureTypeDTO_forGetandGetAll>> GetStructureType()
        {
            List<StructureTypeDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<StructureTypeDTO_forGetandGetAll>>(_structureTypeReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("StructureType not found");
            }

            return datas;
        }

        public async Task<StructureTypeDTO_forGetandGetAll> GetStructureTypeById(int id)
        {
            StructureTypeDTO_forGetandGetAll item = null;


            item = _mapper.Map<StructureTypeDTO_forGetandGetAll>(await _structureTypeReadRepository.GetByIdAsync(id.ToString(), false));


            if (item == null)
            {
                throw new NotFoundException("StructureType not found");
            }

            return item;
        }

        public async Task StructureTypeUpdate(StructureTypeDTO_forUpdate model)
        {

            var existing = await _structureTypeReadRepository.GetByIdAsync(model.Id.ToString(), false);


            if (existing is null)
            {
                throw new NotFoundException("StructureType not found");

            }



            if (existing is null)
            {
                throw new NotFoundException("StructureType not found");

            }

            var entity = _mapper.Map<StructureType>(model);

            entity = new StructureType
            {
                Id = model.Id,
                Name = model.Name,

            };

            _structureTypeWriteRepository.Update(entity);
            await _structureTypeWriteRepository.SaveAsync();

        }

        public async Task<StructureTypeDTO_forGetandGetAll> DeleteStructureTypeById(int id)
        {

            if (_structureTypeReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _structureTypeWriteRepository.RemoveByIdAsync(id.ToString());
                await _structureTypeWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("StructureType not found");
            }
        }

        #endregion


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


        #region Structure service manager

        public async Task StructureCreate(StructureDTO_forCreate model)
        {



            var entity = _mapper.Map<Structure>(model);


            var existing = await _structureTypeReadRepository.GetByIdAsync(model.StructureTypeId.ToString(), false);

            if (existing is null)
            {
                throw new NotFoundException("StructureType not found");

            }

            entity = new Structure
            {
                Name = model.Name,
                ParentId = model.ParentId,
                StructureTypeId = model.StructureTypeId,


            };


            await _structureWriteRepository.AddAsync(entity);

            await _structureWriteRepository.SaveAsync();
        }

        public async Task<List<StructureDTO_forGetandGetAll>> GetStructure()
        {
            List<StructureDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<StructureDTO_forGetandGetAll>>(_structureReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("Structure not found");
            }

            return datas;
        }

        public async Task<StructureDTO_forGetandGetAll> GetStructureById(int id)
        {
            StructureDTO_forGetandGetAll item = null;


            item = _mapper.Map<StructureDTO_forGetandGetAll>(await _structureReadRepository.GetByIdAsync(id.ToString(), false));


            if (item == null)
            {
                throw new NotFoundException("Structure not found");
            }

            return item;
        }

        public async Task StructureUpdate(StructureDTO_forUpdate model)
        {


            var existing = await _structureTypeReadRepository.GetByIdAsync(model.StructureTypeId.ToString(), false);

            if (existing is null)
            {
                throw new NotFoundException("StructureType not found");

            }

            var existing2 = await _structureReadRepository.GetByIdAsync(model.Id.ToString(), false);


            if (existing2 is null)
            {
                throw new NotFoundException("Structure not found");

            }

            var entity = _mapper.Map<Structure>(model);

            entity = new Structure
            {
                Id = model.Id,
                Name = model.Name,

            };

            _structureWriteRepository.Update(entity);
            await _structureWriteRepository.SaveAsync();

        }

        public async Task<StructureDTO_forGetandGetAll> DeleteStructureById(int id)
        {

            if (_structureReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _structureWriteRepository.RemoveByIdAsync(id.ToString());
                await _structureWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("Structure not found");
            }
        }

        #endregion


        #region Vacancy service manager

        public async Task VacancyCreate(VacancyDTO_forCreate model)
        {



            var entity = _mapper.Map<Vacancy>(model);

            var existing = await _structureReadRepository.GetByIdAsync(model.StructureId.ToString(), false);
            var existing2 = await _positionReadRepository.GetByIdAsync(model.PositionId.ToString(), false);

            if (existing2 is null)
            {
                throw new NotFoundException("Position not found");

            }

            if (existing is null)
            {
                throw new NotFoundException("Structure not found");

            }


            entity = new Vacancy
            {
                Title = model.Title,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                PositionId = model.PositionId,
                StructureId = model.StructureId,


            };


            await _vacancyWriteRepository.AddAsync(entity);

            await _vacancyWriteRepository.SaveAsync();
        }

        public async Task<List<VacancyDTO_forGetandGetAll>> GetVacancy()
        {
            List<VacancyDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<VacancyDTO_forGetandGetAll>>(_vacancyReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("Vacancy not found");
            }

            return datas;
        }

        public async Task<VacancyDTO_forGetandGetAll> GetVacancyById(int id)
        {
            VacancyDTO_forGetandGetAll item = null;


            item = _mapper.Map<VacancyDTO_forGetandGetAll>(await _vacancyReadRepository.GetByIdAsync(id.ToString(), false));


            if (item == null)
            {
                throw new NotFoundException("Vacancy not found");
            }

            return item;
        }

        public async Task VacancyUpdate(VacancyDTO_forUpdate model)
        {


            var existing = await _vacancyReadRepository.GetByIdAsync(model.Id.ToString(), false);
            var existing2 = await _positionReadRepository.GetByIdAsync(model.PositionId.ToString(), false);
            var existing3 = await _structureReadRepository.GetByIdAsync(model.StructureId.ToString(), false);




            if (existing is null)
            {
                throw new NotFoundException("Vacancy not found");

            }

            if (existing2 is null)
            {
                throw new NotFoundException("Position not found");

            }

            if (existing3 is null)
            {
                throw new NotFoundException("Structure not found");

            }




            var entity = _mapper.Map<Vacancy>(model);



            entity = new Vacancy
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                PositionId = model.PositionId,
                StructureId = model.StructureId,


            };

            _vacancyWriteRepository.Update(entity);
            await _vacancyWriteRepository.SaveAsync();

        }

        public async Task<VacancyDTO_forGetandGetAll> DeleteVacancyById(int id)
        {

            if (_vacancyReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _vacancyWriteRepository.RemoveByIdAsync(id.ToString());
                await _vacancyWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("Vacancy not found");
            }
        }

        #endregion


        #region Session service manager

        public async Task SessionCreate(SessionDTO_forCreate model)
        {



            var entity = _mapper.Map<Session>(model);


            var existing1 = await _vacancyReadRepository.GetByIdAsync(model.VacancyId.ToString(), false);
            var existing2 = await _candidateReadRepository.GetByIdAsync(model.CandidateId.ToString(), false);

            if (existing1 is null)
            {
                throw new NotFoundException("Vacancy not found");

            }

            if (existing2 is null)
            {
                throw new NotFoundException("Candidate not found");

            }

            entity = new Session
            {
                EndValue = entity.EndValue,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                VacancyId = model.VacancyId,
                CandidateId = model.CandidateId,


            };


            await _sessionWriteRepository.AddAsync(entity);

            await _sessionWriteRepository.SaveAsync();
        }

        public async Task<List<SessionDTO_forGetandGetAll>> GetSession()
        {
            List<SessionDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<SessionDTO_forGetandGetAll>>(_sessionReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("Session not found");
            }

            return datas;
        }

        public async Task<SessionDTO_forGetandGetAll> GetSessionById(int id)
        {
            SessionDTO_forGetandGetAll item = null;


            item = _mapper.Map<SessionDTO_forGetandGetAll>(await _sessionReadRepository.GetByIdAsync(id.ToString(), false));


            if (item == null)
            {
                throw new NotFoundException("Session not found");
            }

            return item;
        }

        public async Task SessionUpdate(SessionDTO_forUpdate model)
        {


            var existing = await _sessionReadRepository.GetByIdAsync(model.Id.ToString(), false);
            var existing2 = await _vacancyReadRepository.GetByIdAsync(model.VacancyId.ToString(), false);
            var existing3 = await _candidateReadRepository.GetByIdAsync(model.CandidateId.ToString(), false);




            if (existing is null)
            {
                throw new NotFoundException("Session not found");

            }

            if (existing2 is null)
            {
                throw new NotFoundException("Vacancy not found");

            }

            if (existing3 is null)
            {
                throw new NotFoundException("Candidate not found");

            }




            var entity = _mapper.Map<Session>(model);



            entity = new Session
            {
                Id = model.Id,
                EndValue = entity.EndValue,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                VacancyId = model.VacancyId,
                CandidateId = model.CandidateId,


            };

            _sessionWriteRepository.Update(entity);
            await _sessionWriteRepository.SaveAsync();

        }

        public async Task<SessionDTO_forGetandGetAll> DeleteSessionById(int id)
        {

            if (_sessionReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _sessionWriteRepository.RemoveByIdAsync(id.ToString());
                await _sessionWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("Session not found");
            }
        }



        #endregion


        #region Category service manager

        public async Task CategoryCreate(CategoryDTO_forCreate model)
        {



            var entity = _mapper.Map<Category>(model);



            entity = new Category
            {
                Name = entity.Name,

            };


            await _sessionTypeWriteRepository.AddAsync(entity);

            await _sessionTypeWriteRepository.SaveAsync();
        }

        public async Task<List<CategoryDTO_forGetandGetAll>> GetCategory()
        {
            List<CategoryDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<CategoryDTO_forGetandGetAll>>(_sessionTypeReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("SessionType not found");
            }

            return datas;
        }

        public async Task<CategoryDTO_forGetandGetAll> GetCategoryById(int id)
        {
            CategoryDTO_forGetandGetAll item = null;


            item = _mapper.Map<CategoryDTO_forGetandGetAll>(await _sessionTypeReadRepository.GetByIdAsync(id.ToString(), false));


            if (item == null)
            {
                throw new NotFoundException("SessionType not found");
            }

            return item;
        }

        public async Task CategoryUpdate(CategoryDTO_forUpdate model)
        {


            var existing = await _sessionTypeReadRepository.GetByIdAsync(model.Id.ToString(), false);

            if (existing is null)
            {
                throw new NotFoundException("SessionType not found");

            }


            var entity = _mapper.Map<Category>(model);


            entity = new Category
            {
                Id = existing.Id,
                Name = entity.Name,

            };


            _sessionTypeWriteRepository.Update(entity);
            await _sessionTypeWriteRepository.SaveAsync();

        }

        public async Task<CategoryDTO_forGetandGetAll> DeleteCategoryById(int id)
        {

            if (_sessionTypeReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _sessionTypeWriteRepository.RemoveByIdAsync(id.ToString());
                await _sessionTypeWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("SessionType not found");
            }
        }

        #endregion


        #region Question service manager

        public async Task QuestionCreate(QuestionDTO_forCreate model)
        {



            var entity = _mapper.Map<Question>(model);


            var existing1 = await _sessionTypeReadRepository.GetByIdAsync(model.CategoryId.ToString(), false);
            var existing2 = await _structureReadRepository.GetByIdAsync(model.StructureId.ToString(), false);
            var existing3 = await _levelReadRepository.GetByIdAsync(model.LevelId.ToString(), false);

            if (existing3 is null)
            {
                throw new NotFoundException("Level not found");

            }
            if (existing1 is null)
            {
                throw new NotFoundException("Category not found");

            }

            if (existing2 is null)
            {
                throw new NotFoundException("Structure not found");

            }


            entity = new Question
            {
                Text = entity.Text,
                LevelId = entity.LevelId,
                CategoryId = entity.CategoryId,
                StructureId = model.StructureId,


            };


            await _questionWriteRepository.AddAsync(entity);

            await _questionWriteRepository.SaveAsync();
        }

        public async Task<List<QuestionDTO_forGetandGetAll>> GetQuestion()
        {
            List<QuestionDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("Question not found");
            }

            return datas;
        }



        public async Task<QuestionDTO_forGetandGetAll> GetQuestionById(int id)
        {
            QuestionDTO_forGetandGetAll item = null;


            item = _mapper.Map<QuestionDTO_forGetandGetAll>(await _questionReadRepository.GetByIdAsync(id.ToString(), false));


            if (item == null)
            {
                throw new NotFoundException("Question not found");
            }

            return item;
        }

        public async Task QuestionUpdate(QuestionDTO_forUpdate model)
        {


            var existing = await _questionReadRepository.GetByIdAsync(model.Id.ToString(), false);
            var existing2 = await _sessionTypeReadRepository.GetByIdAsync(model.CategoryId.ToString(), false);
            var existing3 = await _structureReadRepository.GetByIdAsync(model.StructureId.ToString(), false);
            var existing4 = await _levelReadRepository.GetByIdAsync(model.LevelId.ToString(), false);


            if (existing4 is null)
            {
                throw new NotFoundException("Level not found");

            }
            if (existing is null)
            {
                throw new NotFoundException("Question not found");

            }

            if (existing2 is null)
            {
                throw new NotFoundException("CategoryId not found");

            }

            if (existing3 is null)
            {
                throw new NotFoundException("Structure not found");

            }




            var entity = _mapper.Map<Question>(model);



            entity = new Question
            {
                Id = model.Id,
                Text = entity.Text,
                LevelId = entity.LevelId,
                CategoryId = entity.CategoryId,
                StructureId = model.StructureId,


            };

            _questionWriteRepository.Update(entity);
            await _questionWriteRepository.SaveAsync();

        }

        public async Task<QuestionDTO_forGetandGetAll> DeleteQuestionById(int id)
        {

            if (_questionReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _questionWriteRepository.RemoveByIdAsync(id.ToString());
                await _questionWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("Question not found");
            }
        }

        #endregion


        #region SessionQuestion service manager

        public async Task SessionQuestionCreate(SessionQuestionDTO_forCreate model)
        {



            var entity = _mapper.Map<SessionQuestion>(model);


            var existing1 = await _sessionReadRepository.GetByIdAsync(model.SessionId.ToString(), false);
            var existing2 = await _questionReadRepository.GetByIdAsync(model.QuestionId.ToString(), false);

            if (existing1 is null)
            {
                throw new NotFoundException("Session not found");

            }

            if (existing2 is null)
            {
                throw new NotFoundException("Question not found");

            }

            entity = new SessionQuestion
            {
                Value = entity.Value,

                SessionId = entity.SessionId,
                QuestionId = model.QuestionId,


            };


            await _sessionQuestionWriteRepository.AddAsync(entity);

            await _sessionQuestionWriteRepository.SaveAsync();
        }

        public async Task<List<SessionQuestionDTO_forGetandGetAll>> GetSessionQuestion()
        {
            List<SessionQuestionDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<SessionQuestionDTO_forGetandGetAll>>(_sessionQuestionReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("SessionQuestion not found");
            }

            return datas;
        }

        public async Task<SessionQuestionDTO_forGetandGetAll> GetSessionQuestionById(int id)
        {
            SessionQuestionDTO_forGetandGetAll item = null;


            item = _mapper.Map<SessionQuestionDTO_forGetandGetAll>(await _sessionQuestionReadRepository.GetByIdAsync(id.ToString(), false));


            if (item == null)
            {
                throw new NotFoundException("SessionQuestion not found");
            }

            return item;
        }

        public async Task SessionQuestionUpdate(SessionQuestionDTO_forUpdate model)
        {


            var existing = await _sessionQuestionReadRepository.GetByIdAsync(model.Id.ToString(), false);
            var existing2 = await _sessionReadRepository.GetByIdAsync(model.SessionId.ToString(), false);
            var existing3 = await _questionReadRepository.GetByIdAsync(model.QuestionId.ToString(), false);




            if (existing is null)
            {
                throw new NotFoundException("SessionQuestion not found");

            }

            if (existing2 is null)
            {
                throw new NotFoundException("Session not found");

            }

            if (existing3 is null)
            {
                throw new NotFoundException("Question not found");

            }




            var entity = _mapper.Map<SessionQuestion>(model);



            entity = new SessionQuestion
            {
                Value = entity.Value,

                SessionId = entity.SessionId,
                QuestionId = model.QuestionId,


            };

            _sessionQuestionWriteRepository.Update(entity);
            await _sessionQuestionWriteRepository.SaveAsync();

        }

        public async Task<SessionQuestionDTO_forGetandGetAll> DeleteSessionQuestionById(int id)
        {

            if (_sessionQuestionReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _sessionQuestionWriteRepository.RemoveByIdAsync(id.ToString());
                await _sessionQuestionWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("SessionQuestion not found");
            }
        }

        async Task<List<QuestionDTO_forGetandGetAll>> SelectRandomItems(List<QuestionDTO_forGetandGetAll> sourceList, int count, Random random, string levelname)
        {
            if (sourceList.Count <= count)
            {
                //return sourceList;

                throw new NotFoundException($"Not enough questions on {levelname} level.") ;
            }

            var selectedItems = new List<QuestionDTO_forGetandGetAll>();
            var tempList = new List<QuestionDTO_forGetandGetAll>(sourceList);

            for (int i = 0; i < count; i++)
            {
                int randomIndex = random.Next(tempList.Count);
                QuestionDTO_forGetandGetAll randomItem = tempList[randomIndex];
                selectedItems.Add(randomItem);
                tempList.RemoveAt(randomIndex);
            }

            return selectedItems;
        }

        public async Task<List<QuestionDTO_forGetandGetAll>> GetRandomQuestion(int questionCount, int structureId, int positionId, int vacantionId, int sessionId)
        {
 

            List<QuestionDTO_forGetandGetAll> easyList = new List<QuestionDTO_forGetandGetAll>();
            List<QuestionDTO_forGetandGetAll> mediumList = new List<QuestionDTO_forGetandGetAll>();
            List<QuestionDTO_forGetandGetAll> hardList = new List<QuestionDTO_forGetandGetAll>();
            List<QuestionDTO_forGetandGetAll> randomList = new List<QuestionDTO_forGetandGetAll>();

            Random rnd =null;

            await Task.Run(() =>
            {
                if (_mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false)).Count < questionCount)
                {
                    throw new NotFoundException("Not enough questions");
                }
            });

            await Task.Run(() =>
            {
                if (!_mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false)).Any(i => i.StructureId == structureId))
                {
                    throw new NotFoundException("There are no questions about the selected structure.");
                }
            });

            await Task.Run(() =>
            {
                if (!_mapper.Map<List<StructureDTO_forGetandGetAll>>(_structureReadRepository.GetAll(false)).Any(i => i.Id == structureId))
                {

                    throw new NotFoundException("Structure not found");
                }
            });

            await Task.Run(() =>
            {

                if (!_mapper.Map<List<PositionDTO_forGetandGetAll>>(_positionReadRepository.GetAll(false)).Any(i => i.Id == positionId))
                {
                    throw new NotFoundException("Position not found");
                }
            });

            await Task.Run(() =>
            {
                if (!_mapper.Map<List<VacancyDTO_forGetandGetAll>>(_vacancyReadRepository.GetAll(false)).Any(i => i.Id == vacantionId && i.PositionId == positionId && i.StructureId == structureId))
                {
                    throw new NotFoundException("Vacancy not found");
                }
            });

            await Task.Run(() =>
            {
                if (!_mapper.Map<List<SessionDTO_forGetandGetAll>>(_sessionReadRepository.GetAll(false)).Any(i => i.Id == sessionId && i.VacancyId == vacantionId))
                {
                    throw new NotFoundException("Session not found");
                }
            });


            if (_mapper.Map<List<StructureDTO_forGetandGetAll>>(_structureReadRepository.GetAll(false)).Any(i => i.Id == structureId))
            {
                if (_mapper.Map<List<PositionDTO_forGetandGetAll>>(_positionReadRepository.GetAll(false)).Any(i => i.Id == positionId))
                {
                    if (_mapper.Map<List<VacancyDTO_forGetandGetAll>>(_vacancyReadRepository.GetAll(false)).Any(i => i.Id == vacantionId && i.PositionId == positionId))
                    {
                        if (_mapper.Map<List<SessionDTO_forGetandGetAll>>(_sessionReadRepository.GetAll(false)).Any(i => i.Id == sessionId && i.VacancyId == vacantionId))
                        {



                            if (_mapper.Map<PositionDTO_forGetandGetAll>(await _positionReadRepository.GetByIdAsync(positionId.ToString(), false)).Name == "Junior")
                            {
                                rnd = new Random();

                                await Task.Run(() =>
                                {
                                    easyList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1 && i.StructureId == structureId));
                                });

                                await Task.Run(() =>
                                {
                                    mediumList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2 && i.StructureId == structureId));
                                });

                                await Task.Run(() =>
                                {
                                    hardList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3 && i.StructureId == structureId));
                                });

                                randomList.AddRange(await SelectRandomItems(easyList, Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 50 / 100, MidpointRounding.AwayFromZero)), rnd, "easy"));
                                randomList.AddRange(await SelectRandomItems(mediumList, Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 30 / 100, MidpointRounding.AwayFromZero)), rnd, "medium"));
                                randomList.AddRange(await SelectRandomItems(hardList, Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 20 / 100, MidpointRounding.AwayFromZero)), rnd, "difficult"));


                                await Task.Run(() =>
                                {
                                    if (randomList.Count > questionCount)
                                    {
                                        var c = randomList.Count - questionCount;

                                        for (int i = 0; i < c; i++)
                                        {

                                            randomList.RemoveAt(randomList.Count - 1);

                                        }
                                    }
                                });


                                //List<SessionQuestionDTO_forGetandGetAll> sessionQuestions = null;

                                //await Task.Run(() =>
                                //{
                                //    sessionQuestions = _mapper.Map<List<SessionQuestionDTO_forGetandGetAll>>(_sessionQuestionReadRepository.GetAll(false));
                                //});


                                //foreach (var entity in sessionQuestions)
                                //{
                                //    await _sessionQuestionWriteRepository.RemoveByIdAsync(entity.Id.ToString());
                                //}

                                //await _sessionQuestionWriteRepository.SaveAsync();


                                foreach (var entity in randomList)
                                {
                                    var sessionQuestion = new SessionQuestion
                                    {
                                        QuestionId = entity.Id,
                                        SessionId = sessionId
                                    };

                                    await _sessionQuestionWriteRepository.AddAsync(sessionQuestion);
                                }


                                await _sessionQuestionWriteRepository.SaveAsync();



                           

                                //await Task.Run(() =>
                                //{
                                //    easyList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1));

                                //    var easyrandomNumbers = Enumerable.Range(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1).FirstOrDefault().Id, _questionReadRepository.GetAll(false).Where(i => i.LevelId == 1).Count() - 1)
                                //    .OrderBy(x => rnd.Next())
                                //    .Take(Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 50 / 100, MidpointRounding.AwayFromZero)))
                                //    .ToList();


                                //    foreach (var id in easyrandomNumbers)
                                //    {
                                //        foreach (var item in easyList)
                                //        {
                                //            if (item.Id == id)
                                //            {
                                //                randomList.Add(item);
                                //                break;
                                //            }
                                //        }
                                //    }
                                //});


                                //await Task.Run(() =>
                                //{
                                //    mediumList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2));

                                //    var mediumrandomNumbers = Enumerable.Range(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2).FirstOrDefault().Id, _questionReadRepository.GetAll(false).Where(i => i.LevelId == 2).Count() - 1)
                                //   .OrderBy(x => rnd.Next())
                                //   .Take(Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 30 / 100, MidpointRounding.AwayFromZero)))
                                //   .ToList();

                                //    foreach (var id in mediumrandomNumbers)
                                //    {
                                //        foreach (var item in mediumList)
                                //        {
                                //            if (item.Id == id)
                                //            {
                                //                randomList.Add(item);
                                //                break;
                                //            }
                                //        }
                                //    }

                                //});


                                //await Task.Run(() =>
                                //{
                                //    hardList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3));

                                //    var hardrandomNumbers = Enumerable.Range(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3).FirstOrDefault().Id, _questionReadRepository.GetAll(false).Where(i => i.LevelId == 3).Count() - 1)
                                //   .OrderBy(x => rnd.Next())
                                //   .Take(Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 20 / 100, MidpointRounding.AwayFromZero)))
                                //   .ToList();


                                //    foreach (var id in hardrandomNumbers)
                                //    {
                                //        foreach (var item in hardList)
                                //        {
                                //            if (item.Id == id)
                                //            {
                                //                randomList.Add(item);
                                //                break;
                                //            }
                                //        }
                                //    }

                                //});




                            }

                            if (_mapper.Map<PositionDTO_forGetandGetAll>(await _positionReadRepository.GetByIdAsync(positionId.ToString(), false)).Name == "Middle")
                                {
                                    rnd = new Random();


                                    await Task.Run(() =>
                                    {
                                        easyList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1 && i.StructureId==structureId));
                                    });

                                    await Task.Run(() =>
                                    {
                                        mediumList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2 && i.StructureId == structureId));
                                    });

                                    await Task.Run(() =>
                                    {
                                        hardList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3 && i.StructureId == structureId));
                                    });


                                    randomList.AddRange(await SelectRandomItems(easyList, Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 20 / 100, MidpointRounding.AwayFromZero)), rnd, "easy"));
                                    randomList.AddRange(await SelectRandomItems(mediumList, Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 50 / 100, MidpointRounding.AwayFromZero)), rnd, "medium"));
                                    randomList.AddRange(await SelectRandomItems(hardList, Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 30 / 100, MidpointRounding.AwayFromZero)), rnd, "difficult"));


                                //List<SessionQuestionDTO_forGetandGetAll> sessionQuestions = null;

                                //await Task.Run(() =>
                                //{
                                //    sessionQuestions = _mapper.Map<List<SessionQuestionDTO_forGetandGetAll>>(_sessionQuestionReadRepository.GetAll(false));
                                //});


                                //foreach (var entity in sessionQuestions)
                                //{
                                //    await _sessionQuestionWriteRepository.RemoveByIdAsync(entity.Id.ToString());
                                //}

                                //await _sessionQuestionWriteRepository.SaveAsync();


                                await Task.Run(() =>
                                {
                                    if (randomList.Count > questionCount)
                                    {
                                        var c = randomList.Count - questionCount;

                                        for (int i = 0; i < c; i++)
                                        {

                                            randomList.RemoveAt(randomList.Count - 1);

                                        }
                                    }
                                });


                                foreach (var entity in randomList)
                                    {
                                        var sessionQuestion = new SessionQuestion
                                        {
                                            QuestionId = entity.Id,
                                            SessionId = sessionId
                                        };

                                        await _sessionQuestionWriteRepository.AddAsync(sessionQuestion);
                                    }


                                    await _sessionQuestionWriteRepository.SaveAsync();


                                    //await Task.Run(() =>
                                    //{
                                    //    easyList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1));

                                    //    var easyrandomNumbers = Enumerable.Range(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1).FirstOrDefault().Id, _questionReadRepository.GetAll(false).Where(i => i.LevelId == 1).Count() - 1)
                                    //    .OrderBy(x => rnd.Next())
                                    //    .Take(Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 20 / 100, MidpointRounding.AwayFromZero)))
                                    //    .ToList();

                                    //    foreach (var id in easyrandomNumbers)
                                    //    {
                                    //        foreach (var item in easyList)
                                    //        {
                                    //            if (item.Id == id)
                                    //            {
                                    //                randomList.Add(item);
                                    //                break;
                                    //            }
                                    //        }
                                    //    }
                                    //});


                                    //await Task.Run(() =>
                                    //{
                                    //    mediumList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2));

                                    //    var mediumrandomNumbers = Enumerable.Range(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2).FirstOrDefault().Id, _questionReadRepository.GetAll(false).Where(i => i.LevelId == 2).Count() - 1)
                                    //   .OrderBy(x => rnd.Next())
                                    //   .Take(Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 50 / 100, MidpointRounding.AwayFromZero)))
                                    //   .ToList();

                                    //    foreach (var id in mediumrandomNumbers)
                                    //    {
                                    //        foreach (var item in mediumList)
                                    //        {
                                    //            if (item.Id == id)
                                    //            {
                                    //                randomList.Add(item);
                                    //                break;
                                    //            }
                                    //        }
                                    //    }

                                    //});


                                    //await Task.Run(() =>
                                    //{
                                    //    hardList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3));

                                    //    var hardrandomNumbers = Enumerable.Range(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3).FirstOrDefault().Id, _questionReadRepository.GetAll(false).Where(i => i.LevelId == 3).Count() - 1)
                                    //   .OrderBy(x => rnd.Next())
                                    //   .Take(Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 30 / 100, MidpointRounding.AwayFromZero)))
                                    //   .ToList();


                                    //    foreach (var id in hardrandomNumbers)
                                    //    {
                                    //        foreach (var item in hardList)
                                    //        {
                                    //            if (item.Id == id)
                                    //            {
                                    //                randomList.Add(item);
                                    //                break;
                                    //            }
                                    //        }
                                    //    }

                                    //});

                                }

                            if (_mapper.Map<PositionDTO_forGetandGetAll>(await _positionReadRepository.GetByIdAsync(positionId.ToString(), false)).Name == "Senior")
                                {
                                    rnd = new Random();


                                    await Task.Run(() =>
                                    {
                                        easyList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1 && i.StructureId == structureId));
                                    });

                                    await Task.Run(() =>
                                    {
                                        mediumList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2 && i.StructureId == structureId));
                                    });

                                    await Task.Run(() =>
                                    {
                                        hardList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3 && i.StructureId == structureId));
                                    });


                                    randomList.AddRange(await SelectRandomItems(easyList, Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 20 / 100, MidpointRounding.AwayFromZero)), rnd, "easy"));
                                    randomList.AddRange(await SelectRandomItems(mediumList, Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 30 / 100, MidpointRounding.AwayFromZero)), rnd, "medium"));
                                    randomList.AddRange(await SelectRandomItems(hardList, Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 50 / 100, MidpointRounding.AwayFromZero)), rnd, "difficult"));



                                await Task.Run(() =>
                                {
                                    if (randomList.Count > questionCount)
                                    {
                                        var c = randomList.Count - questionCount;

                                        for (int i = 0; i < c; i++)
                                        {

                                            randomList.RemoveAt(randomList.Count - 1);

                                        }
                                    }
                                });


                                //List<SessionQuestionDTO_forGetandGetAll> sessionQuestions = null;

                                //await Task.Run(() =>
                                //{
                                //    sessionQuestions = _mapper.Map<List<SessionQuestionDTO_forGetandGetAll>>(_sessionQuestionReadRepository.GetAll(false));
                                //});


                                //foreach (var entity in sessionQuestions)
                                //{
                                //    await _sessionQuestionWriteRepository.RemoveByIdAsync(entity.Id.ToString());
                                //}

                                //await _sessionQuestionWriteRepository.SaveAsync();


                                foreach (var entity in randomList)
                                    {
                                        var sessionQuestion = new SessionQuestion
                                        {
                                            QuestionId = entity.Id,
                                            SessionId = sessionId
                                        };

                                        await _sessionQuestionWriteRepository.AddAsync(sessionQuestion);
                                    }


                                    await _sessionQuestionWriteRepository.SaveAsync();




                                    //await Task.Run(() =>
                                    //{
                                    //    easyList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1));

                                    //    var easyrandomNumbers = Enumerable.Range(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1).FirstOrDefault().Id, _questionReadRepository.GetAll(false).Where(i => i.LevelId == 1).Count() - 1)
                                    //    .OrderBy(x => rnd.Next())
                                    //    .Take(Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 20 / 100, MidpointRounding.AwayFromZero)))
                                    //    .ToList();

                                    //    foreach (var id in easyrandomNumbers)
                                    //    {
                                    //        foreach (var item in easyList)
                                    //        {
                                    //            if (item.Id == id)
                                    //            {
                                    //                randomList.Add(item);
                                    //                break;
                                    //            }
                                    //        }
                                    //    }
                                    //});


                                    //await Task.Run(() =>
                                    //{
                                    //    mediumList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2));

                                    //    var mediumrandomNumbers = Enumerable.Range(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2).FirstOrDefault().Id, _questionReadRepository.GetAll(false).Where(i => i.LevelId == 2).Count() - 1)
                                    //   .OrderBy(x => rnd.Next())
                                    //   .Take(Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 30 / 100, MidpointRounding.AwayFromZero)))
                                    //   .ToList();

                                    //    foreach (var id in mediumrandomNumbers)
                                    //    {
                                    //        foreach (var item in mediumList)
                                    //        {
                                    //            if (item.Id == id)
                                    //            {
                                    //                randomList.Add(item);
                                    //                break;
                                    //            }
                                    //        }
                                    //    }

                                    //});


                                    //await Task.Run(() =>
                                    //{
                                    //    hardList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3));

                                    //    var hardrandomNumbers = Enumerable.Range(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3).FirstOrDefault().Id, _questionReadRepository.GetAll(false).Where(i => i.LevelId == 3).Count() - 1)
                                    //   .OrderBy(x => rnd.Next())
                                    //   .Take(Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 50 / 100, MidpointRounding.AwayFromZero)))
                                    //   .ToList();


                                    //    foreach (var id in hardrandomNumbers)
                                    //    {
                                    //        foreach (var item in hardList)
                                    //        {
                                    //            if (item.Id == id)
                                    //            {
                                    //                randomList.Add(item);
                                    //                break;
                                    //            }
                                    //        }
                                    //    }

                                    //});

                                }


                            
                        

                        }
                    }

                }

            }
            if (randomList.Count <= 0)
            {
                throw new NotFoundException("Question not found");
            }


                //if (randomList.Count < questionCount)
                //{

                //    throw new NotFoundException("Not enough questions");

                //}



                return randomList;
        }



        #endregion




    }
}
