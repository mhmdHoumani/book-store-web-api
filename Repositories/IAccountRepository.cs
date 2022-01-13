using Book_Store_Web_Api.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Book_Store_Web_Api.Repositories
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUpAsync(SignUpModel signUp);
        Task<string> LoginAsync(LoginModel login);
    }
}