using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Models;
using System;

namespace Services.UserRepositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>();

        public Task<List<User>> createUser(User user)
        {
            user.id = System.Guid.NewGuid();
            _users.Add(user);
            return Task.FromResult(_users);
        }

        public Task<User> getByEmail(string email)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.email == email));
        }

        public Task<User> getByUsername(string username)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.username == username));
        }

        public Task<List<User>> getAll()
        {
            return Task.FromResult(_users);
        }

        public Task<User> getById(Guid id)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.id == id));
        }
    }
}