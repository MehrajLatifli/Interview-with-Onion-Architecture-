using AutoMapper;
using Interview.Application.Exception;
using Interview.Application.Mapper.DTO;
using Interview.Application.Repositories.Custom;
using Interview.Application.Services.Abstract;
using Interview.Domain.Entities.Models;

namespace Interview.Application.Services.Concrete
{
    public class CategoryServiceManager : ICategoryService
    {

        public readonly IMapper _mapper;



        private readonly ICategoryWriteRepository _categoryWriteRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;

        public CategoryServiceManager(IMapper mapper, ICategoryWriteRepository categoryWriteRepository, ICategoryReadRepository categoryReadRepository)
        {
            _mapper = mapper;
            _categoryWriteRepository = categoryWriteRepository;
            _categoryReadRepository = categoryReadRepository;
        }




        #region Category service manager

        public async Task CategoryCreate(CategoryDTO_forCreate model)
        {



            var entity = _mapper.Map<Category>(model);



            entity = new Category
            {
                Name = entity.Name,

            };


            await _categoryWriteRepository.AddAsync(entity);

            await _categoryWriteRepository.SaveAsync();
        }

        public async Task<List<CategoryDTO_forGetandGetAll>> GetCategory()
        {

            List<CategoryDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<CategoryDTO_forGetandGetAll>>(_categoryReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("Category not found");
            }

            return datas;
        }

        public async Task<CategoryDTO_forGetandGetAll> GetCategoryById(int id)
        {
            CategoryDTO_forGetandGetAll item = null;


            item = _mapper.Map<CategoryDTO_forGetandGetAll>(await _categoryReadRepository.GetByIdAsync(id.ToString(), false));


            if (item == null)
            {
                throw new NotFoundException("Category not found");
            }

            return item;
        }

        public async Task CategoryUpdate(CategoryDTO_forUpdate model)
        {


            var existing = await _categoryReadRepository.GetByIdAsync(model.Id.ToString(), false);

            if (existing is null)
            {
                throw new NotFoundException("Category not found");

            }


            var entity = _mapper.Map<Category>(model);


            entity = new Category
            {
                Id = existing.Id,
                Name = entity.Name,

            };


            _categoryWriteRepository.Update(entity);
            await _categoryWriteRepository.SaveAsync();

        }

        public async Task<CategoryDTO_forGetandGetAll> DeleteCategoryById(int id)
        {

            if (_categoryReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _categoryWriteRepository.RemoveByIdAsync(id.ToString());
                await _categoryWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("Category not found");
            }
        }

        #endregion






    }


}
