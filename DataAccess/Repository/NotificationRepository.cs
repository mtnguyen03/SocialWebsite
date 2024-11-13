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
    public class NotificationRepository : INotificationRepository
    {
        private readonly SocialDbContext _context;

        public NotificationRepository(SocialDbContext context)
        {
            _context = context;
        }

        public async Task Add(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotifications()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<Notification> GetNotificationById(int notificationId)
        {
            return await _context.Notifications.FindAsync(notificationId);
        }


        public async Task<IEnumerable<Notification>> GetNotificationsByUserId(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserID == userId)
                .ToListAsync();
        }

        public async Task Update(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }
    }

}

