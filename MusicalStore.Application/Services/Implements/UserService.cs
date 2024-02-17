using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicalStore.Application.Repositories.Interfaces;
using MusicalStore.Application.Services.Interfaces;
using MusicalStore.Data.EF;
using MusicalStore.Data.Entities;
using MusicalStore.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicalStore.Application.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        //private readonly DataContext _dataContext;

        //public UserService(DataContext dataContext)
        //{
        //    _dataContext = dataContext;
        //}

        public async Task<List<User>> GetAll()
        {
            //var users = await _userRepository.GetAll(); 
            //var usersDto = _mapper.Map<List<UserDto>>(users);
            //return usersDto;

            return await _userRepository.GetAll();
        }

        public async Task<User> GetById(Guid id)
        {
            return await _userRepository.GetById(id);
        }

        public Task<User> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByUsername(string username)
        {
            throw new NotImplementedException();
        }
        public Task<Guid> Create(User user)
        {
            throw new NotImplementedException();
        }

        public Task<int> Delete(User user)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
