using Firlansa.WebUI.Models.Entities;
using Firlansa.WebUI.Models.Entities.Membership;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Firlansa.WebUI.Models.DataContexts
{
    public class FirlansaDbContext : IdentityDbContext<FirlansaUser, FirlansaRole, int, FirlansaUserClaim, FirlansaUserRole, FirlansaUserLogin, FirlansaRoleClaim, FirlansaUserToken>
    {
        public FirlansaDbContext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<ProductSpecification> ProductSpecifications { get; set; }
        public DbSet<ProductStatus> ProductStatuses { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Adress> Adresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProductSpecification>(e =>
            {
                e.HasKey(k => new { k.ProductId, k.SizeId,k.ColorId });
            });

            builder.Entity<FirlansaUser>(e =>
            {
                e.ToTable("Users", "Membership");
            });
            builder.Entity<FirlansaRole>(e =>
            {
                e.ToTable("Roles", "Membership");
            });
            builder.Entity<FirlansaUserRole>(e =>
            {
                e.ToTable("UserRoles", "Membership");
            });
            builder.Entity<FirlansaUserClaim>(e =>
            {
                e.ToTable("UserClaims", "Membership");
            });
            builder.Entity<FirlansaRoleClaim>(e =>
            {
                e.ToTable("RoleClaims", "Membership");
            });
            builder.Entity<FirlansaUserLogin>(e =>
            {
                e.ToTable("UserLogins", "Membership");
            });
            builder.Entity<FirlansaUserToken>(e =>
            {
                e.ToTable("UserTokens", "Membership");
            });
        }
    }
}
