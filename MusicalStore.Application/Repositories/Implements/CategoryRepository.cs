using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Repositories.RepositoryBase;
using MusicalStore.Data.EF;
using MusicalStore.Data.Entities;

namespace MusicalStore.Application.Repositories.Implements
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        private readonly DataContext _dataContext;

        public CategoryRepository(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public async Task<Category?> GetCategoryByName(string name)
        {
            return _dataContext.Categories.Where(e => e.CategoryName == name).FirstOrDefault();
        }
    }
}
