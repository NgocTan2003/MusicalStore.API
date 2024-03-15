using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Repositories.RepositoryBase;
using MusicalStore.Data.EF;
using MusicalStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Repositories.Implements
{
    public class PermissionRepository : RepositoryBase<Permission>, IPermissionRepository
    {
        private readonly DataContext _context;
        public PermissionRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
