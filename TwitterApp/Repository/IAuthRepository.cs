using TwitterApp.Dtos;
using TwitterApp.Models;

namespace TwitterApp.Repository
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<bool> UserExists(string username);
        Task<bool> EmailExists(string email);
        Task<ServiceResponse<List<UserListDto>>> UserList();
        Task<ServiceResponse<List<UserListDto>>> SearchUser(string username);
    }
}
