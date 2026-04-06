using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LumenAPI.Identity.Database
{
    public class UserAccountDbContext : IdentityDbContext<UserAccount>
    {
        public UserAccountDbContext(DbContextOptions<UserAccountDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Additional model configuration can be done here if needed
            builder.HasDefaultSchema("identity");
        }
    }
}