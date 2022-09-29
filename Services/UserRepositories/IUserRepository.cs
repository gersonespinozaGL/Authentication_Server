using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using System;

namespace Services.UserRepositories
{
    public interface IUserRepository
    {
        Task<User> getByEmail(string email);
        Task<User> getByUsername(string username);
        Task<User> getById(Guid id);
        Task<List<User>> createUser(User user);
        Task<List<User>> getAll();
    }
}