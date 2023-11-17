

using Interview.Application.Mapper.DTO;

namespace Interview.Application.Services.Abstract
{
    public interface IPositionService
    {


        #region Position service

        public Task PositionCreate(PositionDTO_forCreate model);

        public Task<List<PositionDTO_forGetandGetAll>> GetPosition();

        public Task<PositionDTO_forGetandGetAll> GetPositionById(int id);

        public Task PositionUpdate(PositionDTO_forUpdate model);

        public Task<PositionDTO_forGetandGetAll> DeletePositionById(int id);

        #endregion

    }

}
