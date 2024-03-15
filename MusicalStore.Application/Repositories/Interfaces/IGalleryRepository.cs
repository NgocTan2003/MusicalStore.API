using MusicalStore.Data.Entities;
using MusicalStore.Dtos.Galleries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusicalStore.Application.Repositories.RepositoryBase.IRepositoryBase;

namespace MusicalStore.Application.Repositories.Interfaces
{
    public interface IGalleryRepository : IRepositoryBase<Gallery>
    {
        Task<List<Gallery>> GetGalleryByProduct(Guid ProductId);
    }
}
