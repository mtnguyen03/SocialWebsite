using BusinessObject.Authen;
using BusinessObject;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using DataAccess.Helpers;

namespace BookApi.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountRepository(UserManager<User> userManager
            , SignInManager<User> signInManager
            , RoleManager<IdentityRole> roleManager
            , IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
        }

        public async Task<string> SignIn(SignInModel model)
        {
           
            var user = await userManager.FindByEmailAsync(model.Email);
            var passwordChecks = await userManager.CheckPasswordAsync(user, model.Password);    

            if (user == null || !passwordChecks)
            {
                return string.Empty;
            }
        
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role,role.ToString())); 

            }

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(12),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> SignUp(SignUpModel model)
        {
            var user = new User
            {
                FullName = model.FullName,  
                Email = model.Email,
                UserName = model.Email

            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(AppRole.User))
                {
                    await roleManager.CreateAsync(new IdentityRole(AppRole.User));
                }
                await userManager.AddToRoleAsync(user, AppRole.User);
            }
            return result;
        }
    }
}
