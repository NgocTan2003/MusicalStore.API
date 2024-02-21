using MusicalStore.Common.ResponseBase;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.Categories;
using MusicalStore.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategory();
        Task<CategoryDto> GetCategoryById(Guid id);
        Task<CategoryDto> GetCategoryByName(string name);
        Task<bool> CategoryExists(Guid id);
        Task<ResponseMessage> CreateCategory(CreateCategory request);
        Task<ResponseMessage> UpdateCategory(UpdateCategory request);
        Task<ResponseMessage> DeleteCategory(Guid id);
    }
}
