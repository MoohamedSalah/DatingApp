using System.Threading.Tasks;
using DatingApp.API.Model;

namespace DatingApp.API.Data
{
    public interface IAuthRepository
    {
        Task<Users> Register(Users user, string Password);
        Task<Users> Login(string username, string Password);
        Task<bool> UserExists(string username);
    }
}