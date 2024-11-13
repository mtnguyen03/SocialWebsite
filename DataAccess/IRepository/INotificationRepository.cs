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
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetNotifications();
        Task<IEnumerable<Notification>> GetNotificationsByUserId(string userId);
        Task Add(Notification Notification);
        Task Update(Notification notification);
        Task Delete(int notificationId);
        Task<Notification> GetNotificationById(int notificationId);
    }
}
