using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services.UserRepositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmail(string email);
        Task<User> GetByUsername(string username);
        Task<List<User>> CreateUser(User user);
        Task<List<User>> GetAll();
    }
}