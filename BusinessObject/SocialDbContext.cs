using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace BusinessObject
{
    public class SocialDbContext : IdentityDbContext<User>
    {
        public SocialDbContext() { }
        public SocialDbContext(DbContextOptions<SocialDbContext> otp) : base(otp) { }

        public override DbSet<User> Users { get; set; }
        public virtual DbSet<Gift> Gifts { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<CommentReply> CommentReplys { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostImage> PostImages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Add Admin Role
            var adminRoleId = Guid.NewGuid().ToString();
            var userRoleId = Guid.NewGuid().ToString();

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = adminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN"
            });

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = userRoleId,
                Name = "User",
                NormalizedName = "USER"
            });

            // Add Admin User
            var adminUserId = Guid.NewGuid().ToString();
            var adminUser = new User
            {
                Id = adminUserId,
                UserName = "admin@example.com",
                NormalizedUserName = "ADMIN@EXAMPLE.COM",
                Email = "admin@example.com",
                FullName = "Admin",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true
            };

            var passwordHasher = new PasswordHasher<User>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin@123");

            builder.Entity<User>().HasData(adminUser);

            // Assign Admin user to Admin role
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = adminUserId,
                RoleId = adminRoleId
            });

            // Add Regular User
            var regularUserId = Guid.NewGuid().ToString();
            var regularUser = new User
            {
                Id = regularUserId,
                UserName = "user@example.com",
                NormalizedUserName = "USER@EXAMPLE.COM",
                Email = "user@example.com",
                FullName = "UserName",
                NormalizedEmail = "USER@EXAMPLE.COM",
                EmailConfirmed = true
            };

            regularUser.PasswordHash = passwordHasher.HashPassword(regularUser, "User@123");

            builder.Entity<User>().HasData(regularUser);

            // Assign Regular user to User role
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = regularUserId,
                RoleId = userRoleId
            });
        }
    }
}
