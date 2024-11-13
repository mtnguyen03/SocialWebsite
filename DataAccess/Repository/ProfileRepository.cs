using BusinessObject;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly SocialDbContext _context;
    
        public ProfileRepository(SocialDbContext context)
        {
            _context = context;
        }

        public async Task Add(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(User user)
        {
            var oldUser = _context.Users.SingleOrDefault(x => x.Id == user.Id);
            if (oldUser != null)
            {
                _context.Users.Remove(oldUser);
                await _context.SaveChangesAsync();

            }
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var list = await _context.Users.ToListAsync();
            return list;
        }

        public async Task Update(User user)
        {
            var oldUser = _context.Users.SingleOrDefault(x => x.Id == user.Id);
            if (oldUser != null)
            {
                oldUser.Address = user.Address;
                oldUser.BirthDay = user.BirthDay;
                oldUser.Email = user.Email;
                oldUser.UserName = user.UserName;
                oldUser.Email = user.Email;
                oldUser.PhoneNumber = user.PhoneNumber;
                oldUser.FullName = user.FullName;
                oldUser.PhotoUrl = user.PhotoUrl;
                oldUser.UpdatedAt = DateTime.Now;
                _context.Users.Update(oldUser);
                await _context.SaveChangesAsync();
            }
        }
    }
}
