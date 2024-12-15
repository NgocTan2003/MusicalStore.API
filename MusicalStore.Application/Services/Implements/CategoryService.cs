using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Common.ResponseBase;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.Categories;

namespace MusicalStore.Application.Services.Implements
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

 
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<List<CategoryDto>> GetAllCategory()
        {
            var categories = await _categoryRepository.GetAll().ToListAsync();
            var categoryDto = _mapper.Map<List<CategoryDto>>(categories);
            return categoryDto;
        }

        public async Task<CategoryDto> GetCategoryById(Guid id)
        {
            var category = await _categoryRepository.FindById(id);
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        public async Task<bool> CategoryExists(Guid id)
        {
            return await _categoryRepository.Exists(id);
        }

        public async Task<CategoryDto> GetCategoryByName(string name)
        {
            var category = await _categoryRepository.GetCategoryByName(name);
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        public async Task<ResponseMessage> CreateCategory(CreateCategory request)
        {
            ResponseMessage responseMessage = new();
            var findName = await _categoryRepository.GetCategoryByName(request.CategoryName);

            if (findName != null)
            {
                responseMessage.Message = "The name of the product has existed";
                responseMessage.StatusCode = 400;
            }
            else
            {
                var category = new Category();
                category.CategoryID = Guid.NewGuid();
                category.CategoryName = request.CategoryName;
                category.DateCreated = DateTime.Now;
                category.CreateBy = "Admin";

                var create = await _categoryRepository.Create(category);
                if (create > 0)
                {
                    responseMessage.Message = "Success";
                    responseMessage.StatusCode = 200;
                }
                else
                {
                    responseMessage.Message = "Fail";
                    responseMessage.StatusCode = 500;
                }
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> UpdateCategory(UpdateCategory request)
        {
            var find = await _categoryRepository.FindById(request.CategoryID);

            find.CategoryName = request.CategoryName;
            find.ModifiedDate = DateTime.Now;
            find.UpdateBy = request.UpdateBy;

            var update = await _categoryRepository.Update(find);

            ResponseMessage responseMessage = new();
            if (update > 0)
            {
                responseMessage.Message = "Success";
                responseMessage.StatusCode = 200;
            }
            else
            {
                responseMessage.Message = "Fail";
                responseMessage.StatusCode = 500;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> DeleteCategory(Guid id)
        {
            ResponseMessage responseMessage = new();

            var exists = await _categoryRepository.Exists(id);

            if (!exists)
            {
                responseMessage.Message = "Not Found";
                responseMessage.StatusCode = 400;

                return responseMessage;
            }
            else
            {
                var delete = await _categoryRepository.Delete(id);

                if (delete > 0)
                {
                    responseMessage.Message = "Success";
                    responseMessage.StatusCode = 200;
                }
                else
                {
                    responseMessage.Message = "Fail";
                    responseMessage.StatusCode = 500;
                }
                return responseMessage;
            }
        }

    }
}

