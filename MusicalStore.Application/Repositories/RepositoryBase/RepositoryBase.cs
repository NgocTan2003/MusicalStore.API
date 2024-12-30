using Microsoft.EntityFrameworkCore;
using MusicalStore.Common.Seedwork;
using MusicalStore.Data.EF;
using MusicalStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusicalStore.Application.Repositories.RepositoryBase.IRepositoryBase;

namespace MusicalStore.Application.Repositories.RepositoryBase
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly DbSet<T> table;
        private readonly DataContext _context;
        private static int PageSize { get; set; } = 5;


        public RepositoryBase(DataContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return table;
        }

        public async Task<bool> Exists(Guid Id)
        {
            var find = await table.FindAsync(Id);
            if (find != null)
            {
                return true;
            }
            return false;
        }

        public async Task<T> FindById(Guid Id)
        {
            var find = await table.FindAsync(Id);
            return find;
        }

        public async Task<int> Create(T entity)
        {
            try
            {
                await table.AddAsync(entity);
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database Update Error: {dbEx.Message}");
                throw; // Tái ném lỗi nếu cần
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; // Tái ném lỗi nếu cần
            }
        }

        public async Task<int> Update(T entity)
        {
            try
            {
                table.Update(entity);
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database Update Error: {dbEx.Message}");
                throw; // Tái ném lỗi nếu cần
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; // Tái ném lỗi nếu cần
            }
        }

        public async Task<int> Delete(Guid Id)
        {
            var find = await table.FindAsync(Id);
            if (find is not null)
            {
                table.Remove(find);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<List<T>> Pagination(int page = 1)
        {
            var result = PaginatedList<T>.Create(GetAll(), page, PageSize);
            return result;
        }
    }
}
