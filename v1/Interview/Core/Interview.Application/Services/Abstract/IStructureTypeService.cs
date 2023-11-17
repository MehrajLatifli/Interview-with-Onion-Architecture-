

using Interview.Application.Mapper.DTO;

namespace Interview.Application.Services.Abstract
{
    public interface IStructureTypeService
    {
        #region StructureType service

        public Task StructureTypeCreate(StructureTypeDTO_forCreate model);

        public Task<List<StructureTypeDTO_forGetandGetAll>> GetStructureType();

        public Task<StructureTypeDTO_forGetandGetAll> GetStructureTypeById(int id);

        public Task StructureTypeUpdate(StructureTypeDTO_forUpdate model);

        public Task<StructureTypeDTO_forGetandGetAll> DeleteStructureTypeById(int id);

        #endregion
    }


}
