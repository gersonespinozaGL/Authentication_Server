using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Models;

namespace Services.UserRepositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();

        public Task<List<User>> CreateUser(User user)
        {
            user.id = System.Guid.NewGuid();
            _users.Add(user);
            return Task.FromResult(_users);
        }

        public Task<User> GetByEmail(string email)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.email == email));
        }

        public Task<User> GetByUsername(string username)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.username == username));
        }

        public Task<List<User>> GetAll()
        {
            return Task.FromResult(_users);
        }

    }
}