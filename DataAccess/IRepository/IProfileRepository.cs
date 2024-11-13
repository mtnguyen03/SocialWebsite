using BusinessObject;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IProfileRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(string id);
        Task Add(User user);
        Task Update(User user);
        Task Delete(User user);

    }
}
