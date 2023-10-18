

using Interview.Application.Mapper.DTO;

namespace Interview.Application.Services.Abstract
{
    public interface IStructureService
    {
        #region Structure service

        public Task StructureCreate(StructureDTO_forCreate model);

        public Task<List<StructureDTO_forGetandGetAll>> GetStructure();

        public Task<StructureDTO_forGetandGetAll> GetStructureById(int id);

        public Task StructureUpdate(StructureDTO_forUpdate model);

        public Task<StructureDTO_forGetandGetAll> DeleteStructureById(int id);

        #endregion
    }


}
