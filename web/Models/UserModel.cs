namespace SocialFrontEnd.Models
{
    public class UserModel
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Role { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
