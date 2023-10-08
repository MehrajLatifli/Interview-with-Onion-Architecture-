using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Interview.Application.Exception;
using Interview.Application.Mapper.DTO;
using Interview.Application.Repositories.Custom;
using Interview.Application.Services.Abstract;
using Interview.Domain.Entities.Models;

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

        private readonly ISessionTypeWriteRepository _sessionTypeWriteRepository;
        private readonly ISessionTypeReadRepository _sessionTypeReadRepository;

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

        public ServiceManager(IMapper mapper, ICandidateDocumentWriteRepository candidateDocumentWriteRepository, ICandidateDocumentReadRepository candidateDocumentReadRepository, ICandidateWriteRepository candidateWriteRepository, ICandidateReadRepository candidateReadRepository, ILevelWriteRepository levelWriteRepository, ILevelReadRepository levelReadRepository, ISessionTypeWriteRepository sessionTypeWriteRepository, ISessionTypeReadRepository sessionTypeReadRepository, IQuestionWriteRepository questionWriteRepository, IQuestionReadRepository questionReadRepository, ISessionQuestionWriteRepository sessionQuestionWriteRepository, ISessionQuestionReadRepository sessionQuestionReadRepository, ISessionWriteRepository sessionWriteRepository, ISessionReadRepository sessionReadRepository, IStructureWriteRepository structureWriteRepository, IStructureReadRepository structureReadRepository, IStructureTypeWriteRepository structureTypeWriteRepository, IStructureTypeReadRepository structureTypeReadRepository, IVacancyWriteRepository vacancyWriteRepository, IVacancyReadRepository vacancyReadRepository, IPositionWriteRepository positionWriteRepository, IPositionReadRepository positionReadRepository)
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
                Patronymic=model.Patronymic,

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
                throw new NotFoundException("Candidate not found");

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



    }
}
