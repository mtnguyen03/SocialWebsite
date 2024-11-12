
using BusinessObject;
using BusinessObject.Authen;
using Microsoft.AspNetCore.Identity;

namespace BookApi.Repositories
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUp(SignUpModel model);
        public Task<string> SignIn(SignInModel model);
        Task<List<User>> GetUsers();
    }
}
