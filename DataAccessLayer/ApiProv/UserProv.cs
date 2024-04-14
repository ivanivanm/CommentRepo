using DataAccessLayer.DbProv;
using DataAccessLayer.Models;
using DataBase;
using DataBase.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ApiProv
{
    public class UserProv
    {
        private readonly UserContext _userContext;

        public UserProv(UserContext userContext)
        {
            _userContext = userContext;
        }
        public UserProv()
        {
            AppDbContext db = new AppDbContext();

            _userContext = new UserContext(db);
        }
        public async Task CreateUser(User user)
        {
            if (user == null)
            {
                throw new Exception("user was null");
            }

            UserDbModel newUser = new UserDbModel(user.Name, user.Email, user.Password, user.Role);
            await _userContext.Create(newUser);
        }

        public async Task<User> ReadUser(int key)
        {
            UserDbModel userFromDb = await _userContext.Read(key);
            User user = new()
            {
                Id = userFromDb.Id,
                Name = userFromDb.Name,
                Email = userFromDb.Email,
                Password = userFromDb.Password,
                Role = userFromDb.Role
            };
            return user;
        }

        public async Task<List<User>> ReadAllUsers()
        {
            List<UserDbModel> usersFromDb = await _userContext.ReadAll();
            List<User> users = new List<User>();

            users = usersFromDb.Select(x => new User
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Password = x.Password,
                Role = x.Role
            }).ToList();
            return users;
        }

        public async Task UpdateUser(User user)
        {
            UserDbModel userToBeUpdated = new UserDbModel(user.Name, user.Email, user.Password, user.Role);
            await _userContext.Update(userToBeUpdated);
        }
        public async Task DeleteUser(int key)
        {
            await _userContext.Delete(key);
        }
    }
}
