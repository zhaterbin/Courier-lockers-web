using Courier_lockers.Entities;
using Microsoft.EntityFrameworkCore;

namespace Courier_lockers.Data
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions<ServiceDbContext> options)
         : base(options)
        {
            try
            {
                this.Database.SetCommandTimeout(300);
            }
            catch (Exception ex) { }
        }
        public DbSet<edpmain> edpmains { get; set; }
        public DbSet<Cell> Cells { get; set; }
        public DbSet<OpearterIn> opearterIns { get; set; }
        public DbSet<Storage> storages { get; set; }
        public DbSet<Operaterout> operaterouts { get; set; }

        public DbSet<PriceRuler> priceRulers { get; set; }

        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<Permission> permissions { get; set; }
        public DbSet<UserRole> userRoles { get; set; }
        public DbSet<RolePermission> rolePermissions { get; set; }
        public DbSet<role_user_view> roleUsers { get; set; }
        //表和视图
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<edpmain>().HasKey(x => x.Id);

            modelBuilder.Entity<Cell>().HasKey(x => x.CELL_ID);

            modelBuilder.Entity<OpearterIn>().HasKey(x => x.Operator_Id);

            modelBuilder.Entity<Storage>().HasKey(x => x.STORAGE_ID);

            modelBuilder.Entity<Operaterout>().HasKey(x=>x.Operator_Id);

            modelBuilder.Entity<PriceRuler>().HasKey(x => x.priceId);

            modelBuilder.Entity<User>().HasKey(x => x.Id);

            modelBuilder.Entity<UserRole>().HasNoKey();

            modelBuilder.Entity<RolePermission>().HasNoKey();

            modelBuilder.Entity<role_user_view>().HasNoKey();
        }
    }
}
