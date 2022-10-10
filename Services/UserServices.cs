using Hubtel.Wallets.Api.Data;
using Hubtel.Wallets.Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace Hubtel.Wallets.Api.Services
{
    public class UserServices
    {
        private readonly ApplicationDbContext _dbContext;

        public UserServices(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<User> GetUsers()
        {
            List<User> response = new List<User>();
            var dataList = _dbContext.Users.ToList();
            dataList.ForEach(user => response.Add(new User()
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Token = user.Token,
                PhoneNumber = user.PhoneNumber,
       
            }));

            return response;

        }

        // Get a user by email
        public User GetUser(string email)
        {
            return _dbContext.Users.Where(user => user.Email.Equals(email)).FirstOrDefault();
        }

        public User AddUser(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user;
        }



    }
}
