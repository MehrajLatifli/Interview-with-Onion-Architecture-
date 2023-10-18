

using Interview.Application.Mapper.DTO;

namespace Interview.Application.Services.Abstract
{
    public interface ILevelService
    {
        #region Level service

        public Task LevelCreate(LevelDTO_forCreate model);

        public Task<List<LevelDTO_forGetandGetAll>> GetLevel();

        public Task<LevelDTO_forGetandGetAll> GetLevelById(int id);

        public Task LevelUpdate(LevelDTO_forUpdate model);

        public Task<LevelDTO_forGetandGetAll> DeleteLevelById(int id);

        #endregion
    }


}
