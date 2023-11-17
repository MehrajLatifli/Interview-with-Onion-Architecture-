

using Interview.Application.Mapper.DTO;

namespace Interview.Application.Services.Abstract
{
    public interface ICategoryService
    {
        #region Category service

        public Task CategoryCreate(CategoryDTO_forCreate model);

        public Task<List<CategoryDTO_forGetandGetAll>> GetCategory();

        public Task<CategoryDTO_forGetandGetAll> GetCategoryById(int id);

        public Task CategoryUpdate(CategoryDTO_forUpdate model);

        public Task<CategoryDTO_forGetandGetAll> DeleteCategoryById(int id);

        #endregion
    }


}
