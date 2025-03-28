using JWTWithCustomMiddleware.Models;

namespace JWTWithCustomMiddleware.Services
{
   
    public interface IUserService
    {
        Task<UserModel> Login(string userName, string password);
        Task<List<UserModel>> GetUserList();
        Task<UserModel> GetUserById(int id);
    }

    public class UserService : IUserService
    {
        public static List<UserEntity> Users = new List<UserEntity>()
        {
            new UserEntity { Id = 1, UserName = "john.doe@example.com", Password = "123", Role="Admin" },
            new UserEntity { Id = 2, UserName = "jane.smith@example.com", Password = "1234", Role = "User" },
            new UserEntity { Id = 3, UserName = "mike.johnson@example.com", Password = "12345", Role = "SuperAdmin" }
        };

        public async Task<UserModel> Login(string userName, string password)
        {
            UserModel? user = new UserModel();
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                user = Users.Where(x => x.UserName == userName && x.Password == password).Select(x=> new UserModel()
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Role = x.Role
                }).FirstOrDefault();
            }
            return user;
        }

        public async Task<List<UserModel>> GetUserList()
        {
            List<UserModel>? users = new List<UserModel>();
            
            users = Users.Select(x => new UserModel()
            {
                Id = x.Id,
                UserName = x.UserName,
                Role = x.Role
            }).ToList();
            
            return users;
        }

        public async Task<UserModel> GetUserById(int id)
        {
            UserModel? user = new UserModel();
            
            user = Users.Where(x => x.Id == id).Select(x => new UserModel()
            {
                Id = x.Id,
                UserName = x.UserName,
                Role = x.Role
            }).FirstOrDefault();
            
            return user;
        }
    }
}
