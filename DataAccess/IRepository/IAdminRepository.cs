using BusinessObject;

namespace DataAccess.IRepository
{
    public interface IAdminRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserByEmail(string email);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(string email);
    }
}
